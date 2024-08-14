namespace InnBot.MessageProcessors;

public class StartCommand : ICommand
{
    public string CommandName => "/start";

    public Task<string[]> ProcessMessage(long fromId, string[] messageText)
    {
        return Task.FromResult<string[]>(["Приветствую! Для вывода справки введите /help"]);
    }
}