using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GitIgnore;

public class SimpleGraphQLServer
{
    private readonly Func<string, string> _processGraphQLRequest;
    private TcpListener _listener;
    private CancellationTokenSource _cts;

    public string Url { get; private set; }

    public SimpleGraphQLServer(Func<string, string> processGraphQLRequest)
    {
        _processGraphQLRequest = processGraphQLRequest;
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        _listener = new TcpListener(IPAddress.Loopback, 0);
        _listener.Start();
        var port = ((IPEndPoint)_listener.LocalEndpoint).Port;
        Url = $"http://localhost:{port}";
        Console.WriteLine($"GraphQL Server started at {Url}");

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        try
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                var client = await _listener.AcceptTcpClientAsync(_cts.Token);
                _ = HandleClientAsync(client); // Fire and forget
            }
        }
        catch (OperationCanceledException)
        {
            // Server was stopped
        }
        finally
        {
            _listener.Stop();
        }
    }

    public void Stop()
    {
        _cts?.Cancel();
        _listener?.Stop();
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        using (client)
        using (var stream = client.GetStream())
        {
            var buffer = new byte[1024];
            var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            var request = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            if (request.StartsWith("POST /graphql"))
            {
                var contentLength = GetContentLength(request);
                var content = await ReadRequestBodyAsync(stream, contentLength);
                var graphqlResponse = _processGraphQLRequest(content);
                await SendResponseAsync(stream, "200 OK", "application/json", graphqlResponse);
            }
            else
            {
                await SendResponseAsync(stream, "404 Not Found", "text/plain", "Not Found");
            }
        }
    }

    private static int GetContentLength(string request)
    {
        var lines = request.Split('\n');
        foreach (var line in lines)
        {
            if (line.StartsWith("Content-Length:", StringComparison.OrdinalIgnoreCase))
            {
                return int.Parse(line.Split(':')[1].Trim());
            }
        }
        return 0;
    }

    private static async Task<string> ReadRequestBodyAsync(NetworkStream stream, int contentLength)
    {
        var bodyBuffer = new byte[contentLength];
        var bytesRead = 0;
        while (bytesRead < contentLength)
        {
            bytesRead += await stream.ReadAsync(bodyBuffer, bytesRead, contentLength - bytesRead);
        }
        return Encoding.ASCII.GetString(bodyBuffer);
    }

    private static async Task SendResponseAsync(
        NetworkStream stream,
        string status,
        string contentType,
        string content
    )
    {
        var response = $"HTTP/1.1 {status}\r\nContent-Type: {contentType}\r\n\r\n{content}";
        var responseBytes = Encoding.ASCII.GetBytes(response);
        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
    }
}
