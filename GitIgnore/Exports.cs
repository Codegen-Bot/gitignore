using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using CodegenBot;
using Extism;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GitIgnore;

/// <summary>
/// This class contains all the static methods that codegen.bot calls. See also the Imports class,
/// which contains static methods that we can call from within a bot that are implemented by codegen.bot.
/// </summary>
public class Exports
{
    private static WebApplication? _app;

    public static int Main(string[] args)
    {
        var consumedUrl = BotDebugUtility.FindBotDebugUrl(Environment.CurrentDirectory);
        var httpClient = new HttpClient() { BaseAddress = new Uri(consumedUrl) };
        var graphQLClient = new SyncHttpGraphQLClient(httpClient, consumedUrl);

        var builder = WebApplication.CreateBuilder([]);

        builder.WebHost.UseUrls($"http://127.0.0.1:0");

        builder.Logging.ClearProviders();

        _app = builder.Build();

        _app.MapPost(
            "/graphql",
            async (HttpContext context) =>
            {
                using var reader = new StreamReader(context.Request.Body);
                var content = await reader.ReadToEndAsync();
                var response = ProcessGraphQLRequest(content, graphQLClient);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(response);
            }
        );

        _app.Start();

        var server = _app.Services.GetRequiredService<IServer>();
        var addressFeature = server.Features.Get<IServerAddressesFeature>();
        var providedUrl = addressFeature!.Addresses.First();

        graphQLClient.MarkAsReady(Process.GetCurrentProcess().Id, providedUrl);

        var result = RunBot(graphQLClient, null);
    }

    [UnmanagedCallersOnly(EntryPoint = "stop_running")]
    public static int StopRunning()
    {
        _app?.Lifetime.StopApplication();
    }

    private static GraphQLServer? _graphqlServer;

    public static int RunBot(IGraphQLClient graphQLClient, ICodegenBotImports? imports)
    {
        try
        {
            // Create all our minibots here
            IMiniBot[] miniBots =
            [
                // TODO - remove the ExampleMiniBot entry from this list because it creates a hello world file
                // that won't be useful in real life, and could cause problems if you're writing to configuration.OutputPath elsewhere,
                // or if you're assuming configuration.OutputPath is a directory and you're writing to files under it.
                new ExampleMiniBot(graphQLClient),
            ];

            // Run each minibot in order
            foreach (var miniBot in miniBots)
            {
                try
                {
                    miniBot.Execute();
                }
                catch (Exception e)
                {
                    if (imports is not null)
                    {
                        imports.Log(
                            new LogEvent()
                            {
                                Level = LogEventLevel.Critical,
                                Message =
                                    "Failed to run minibot {MiniBot}: {ExceptionType} {Message}, {StackTrace}",
                                Args =
                                [
                                    miniBot.GetType().Name,
                                    e.GetType().Name,
                                    e.Message,
                                    e.StackTrace ?? "",
                                ],
                            }
                        );
                    }
                    else
                    {
                        graphQLClient.Log(
                            // Only a critical error will cause codegen.bot to realize that the generated code should not be used
                            LogSeverity.CRITICAL,
                            "Failed to run minibot {MiniBot}: {ExceptionType} {Message}, {StackTrace}",
                            [
                                miniBot.GetType().Name,
                                e.GetType().Name,
                                e.Message,
                                e.StackTrace ?? "",
                            ]
                        );
                    }
                }
            }

            return 0;
        }
        catch (Exception e)
        {
            if (imports is not null)
            {
                imports.Log(
                    new LogEvent()
                    {
                        Level = LogEventLevel.Critical,
                        Message = "Failed to run bot: {ExceptionType} {Message}, {StackTrace}",
                        Args = [e.GetType().Name, e.Message, e.StackTrace ?? ""],
                    }
                );
                Pdk.SetError($"{e.GetType()}: {e.Message} {e.StackTrace}");
            }
            else
            {
                graphQLClient.Log(
                    // Only a critical error will cause codegen.bot to realize that the generated code should not be used
                    LogSeverity.CRITICAL,
                    "Failed to run bot: {ExceptionType} {Message}, {StackTrace}",
                    [e.GetType().Name, e.Message, e.StackTrace ?? ""]
                );
                Console.WriteLine($"{e.GetType()}: {e.Message}");
            }
            return 0;
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "entry_point")]
    public static int Run()
    {
        var imports = new CodegenBotImports();
        var graphQLClient = new WasmGraphQLClient(imports);

        return RunBot(graphQLClient, imports);
    }

    public static string ProcessGraphQLRequest(string request, IGraphQLClient graphqlClient)
    {
        try
        {
            if (_graphqlServer is null)
            {
                var services = new ServiceCollection();
                services.AddSingleton<IGraphQLClient>(graphQLClient);

                var serviceProvider = services.BuildServiceProvider();
                _graphqlServer = new GraphQLServer(serviceProvider);
            }

            return _graphqlServer.ProcessGraphQLRequest(request);
        }
        catch (Exception e)
        {
            if (imports is not null)
            {
                imports.Log(
                    new LogEvent()
                    {
                        Level = LogEventLevel.Critical,
                        Message =
                            "Bot failed to run handle GraphQL request: {ExceptionType} {Message}, {StackTrace}",
                        Args = [e.GetType().Name, e.Message, e.StackTrace ?? ""],
                    }
                );
                Pdk.SetError($"{e.GetType()}: {e.Message} {e.StackTrace}");
            }
            else
            {
                graphQLClient.Log(
                    // Only a critical error will cause codegen.bot to realize that the generated code should not be used
                    LogSeverity.CRITICAL,
                    "Bot failed to handle GraphQL request: {ExceptionType} {Message}, {StackTrace}",
                    [e.GetType().Name, e.Message, e.StackTrace ?? ""]
                );
                Console.WriteLine($"{e.GetType()}: {e.Message}");
            }
            return "";
        }
    }

    [UnmanagedCallersOnly(EntryPoint = "handle_request")]
    public static int HandleRequest()
    {
        var imports = new CodegenBotImports();
        var graphQLClient = new WasmGraphQLClient(imports);

        var request = Pdk.GetInputString();

        var result = ProcessGraphQLRequest(request, graphQLClient);

        Pdk.SetOutput(result);

        return 0;
    }
}
