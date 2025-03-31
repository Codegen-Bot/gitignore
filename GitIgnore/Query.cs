using Microsoft.Extensions.DependencyInjection;

namespace GitIgnore;

public partial class Query
{
    private readonly ListOfGitIgnoreFiles _listOfGitIgnoreFiles = services.GetRequiredService<ListOfGitIgnoreFiles>();
    
    public partial bool GetHasIgnoreFile(string? folder)
    {
        return _listOfGitIgnoreFiles.DoesGitIgnoreExist(folder ?? "");
    }
}
