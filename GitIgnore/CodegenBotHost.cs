using System;
using System.Runtime.InteropServices;
using System.Text.Json;
using CodegenBot;
using Extism;

namespace GitIgnore;

public class CodegenBotHost
{
    [DllImport("extism", EntryPoint = "cgb_graphql")]
    public static extern ulong ExternGraphQL(ulong offset);

    public static string GraphQL(GraphQLRequest request)
    {
        var json = request.ToJsonString();
        using var block = Pdk.Allocate(json);
        var ptr = ExternGraphQL(block.Offset);
        var response = MemoryBlock.Find(ptr).ReadString();
        return response;
    }

    [DllImport("extism", EntryPoint = "cgb_log")]
    public static extern void ExternLog(ulong offset);

    public static void Log(LogEvent logEvent)
    {
        var json = JsonSerializer.Serialize(logEvent, LogEventJsonContext.Default.LogEvent);
        using var block = Pdk.Allocate(json);
        ExternLog(block.Offset);
    }
}
