namespace InnBot.UserDataSaving;

public interface IRepository
{
    public void SetLastCommand(long fromId, string commandString);
    public string GetLastCommand(long fromId);
}