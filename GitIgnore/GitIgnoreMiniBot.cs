using System.Threading;
using System.Threading.Tasks;
using System.IO;
using CodegenBot;
using System.Linq;

namespace GitIgnore;

public class GitIgnoreMiniBot() : IMiniBot
{
    public void Execute()
    {
        var configuration = GraphQLOperations.GetConfiguration();

        var outputPaths = configuration.Configuration.OutputPath?.ToList();
        if (outputPaths is null)
        {
            outputPaths = new();
        }
        if (outputPaths.Count == 0)
        {
            outputPaths.Add(".");
        }
        
        foreach (var outputPath in outputPaths)
        {
            var path = ".gitignore";
        
            if (!string.IsNullOrWhiteSpace(outputPath))
            {
                path = Path.Combine(outputPath, path).Replace("\\", "/");
            }

            path = Path.GetFullPath(path).Trim('/');
            
            Imports.Log(new LogEvent()
            {
                Level = LogEventLevel.Information, Message = "Adding {Path}",
                Args = [path]
            });
            
            GraphQLOperations.AddFile(
                path,
                $$"""
                  {{CaretRef.New(new CaretTag("location", path))}}

                  """
            );
        }
    }
}
