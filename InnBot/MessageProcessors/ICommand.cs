namespace InnBot.MessageProcessors;

public interface ICommand
{
    string CommandName { get; }

    Task<string[]> ProcessMessage(long fromId, string[] messageText);
    
}