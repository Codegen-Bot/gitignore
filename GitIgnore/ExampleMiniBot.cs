using System.Threading;
using System.Threading.Tasks;

namespace GitIgnore;

public class ExampleMiniBot() : IMiniBot
{
    public void Execute()
    {
        var configuration = GraphQLOperations.GetConfiguration();

        GraphQLOperations.AddFile(
            $"{configuration.Configuration.OutputPath}",
            $$"""
            This file was generated by a C# bot.
            
            """
        );
    }
}