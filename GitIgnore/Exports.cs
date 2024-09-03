using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using CodegenBot;
using Extism;

namespace GitIgnore;

public class Functions
{
    public static void Main()
    {
        // Note: a `Main` method is required for the app to compile
    }

    [UnmanagedCallersOnly(EntryPoint = "entry_point")]
    public static int Run()
    {
        try
        {
            // Create all our minibots here
            IMiniBot[] miniBots =
            [
                new GitIgnoreMiniBot(),
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
                    Imports.Log(
                        new LogEvent()
                        {
                            Level = LogEventLevel.Error,
                            Message =
                                "Failed to run minibot {MiniBot}: {ExceptionType} {Message}, {StackTrace}",
                            Args =
                            [
                                miniBot.GetType().Name,
                                e.GetType().Name,
                                e.Message,
                                e.StackTrace,
                            ],
                        }
                    );
                }
            }

            return 0;
        }
        catch (Exception e)
        {
            Imports.Log(
                new LogEvent()
                {
                    Level = LogEventLevel.Error,
                    Message = "Failed to initialize bot: {ExceptionType} {Message}, {StackTrace}",
                    Args = [e.GetType().Name, e.Message, e.StackTrace],
                }
            );
            Pdk.SetError($"{e.GetType()}: {e.Message}");
            return 0;
        }
    }
}
