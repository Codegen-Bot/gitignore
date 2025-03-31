using System.Collections.Generic;

namespace GitIgnore;

public class ListOfGitIgnoreFiles
{
    private readonly HashSet<string> _state = new();
    
    public bool DoesGitIgnoreExist(string folder)
    {
        return _state.Contains(folder);
    }

    public void AddGitIgnore(string folder)
    {
        _state.Add(folder);
    }
}