namespace InnBot.UserDataSaving;

public class DictionaryRepository : IRepository
{
    private readonly Dictionary<long, string> _usersLastCommands = new();
    
    public void SetLastCommand(long fromId, string commandString)
    {
        _usersLastCommands.Remove(fromId);
        _usersLastCommands.Add(fromId, commandString);
    }

    public string GetLastCommand(long fromId)
    {
        return _usersLastCommands.TryGetValue(fromId, out var commandString) 
            ? commandString 
            : string.Empty;
    }
}