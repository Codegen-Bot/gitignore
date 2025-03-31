using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading;
using CodegenBot;
using Extism;

namespace GitIgnore;

/// <summary>
/// This class contains all the static methods that codegen.bot calls. See also the Imports class,
/// which contains static methods that we can call from within a bot that are implemented by codegen.bot.
/// </summary>
public class Exports
{
    public static int Main(string[] args)
    {
        var botDebug = BotUtility.GetDebugConfiguration(Environment.CurrentDirectory);
        var consumedUrl = botDebug!.ConsumeGraphQLUrl;
        var httpClient = new HttpClient() { BaseAddress = new Uri(consumedUrl) };
        var graphQLClient = new SyncHttpGraphQLClient(httpClient, consumedUrl);

        graphQLClient.MarkAsReady(Process.GetCurrentProcess().Id, null);

        var result = RunBot(graphQLClient, null);
        return result;
    }

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
}
