using System.Collections.Generic;
using CodegenBot;
using Microsoft.Extensions.DependencyInjection;

namespace GitIgnore;

public partial class Mutation
{
    private readonly IGraphQLClient _graphqlClient = services.GetRequiredService<IGraphQLClient>();
    private readonly ListOfGitIgnoreFiles _listOfGitIgnoreFiles = services.GetRequiredService<ListOfGitIgnoreFiles>();
    
    public partial bool AddIgnorePattern(string? folder, IReadOnlyList<string> patterns)
    {
        folder ??= "";
        var addedGitIgnoreFile = false;

        if (!_listOfGitIgnoreFiles.DoesGitIgnoreExist(folder))
        {
            addedGitIgnoreFile = true;
            _listOfGitIgnoreFiles.AddGitIgnore(folder);
            _graphqlClient.AddFile($"{folder}/.gitignore", 
                $$"""
                {{CaretRef.New(new CaretTag("location", $"{folder}/.gitignore"), new CaretTag("filename", ".gitignore"))}}
                
                """);
        }

        foreach (var x in patterns)
        {
            var pattern = x;
            if (!pattern.EndsWith("\n"))
            {
                pattern = pattern + "\n";
            }
        
            _graphqlClient.AddSimpleKeyedTextByTags([
                new CaretTagInput() { Name = "location", Value = $"{folder}/.gitignore" },
                new CaretTagInput() { Name = "filename", Value = ".gitignore" },
            ], pattern);
        }
        
        return addedGitIgnoreFile;
    }
}