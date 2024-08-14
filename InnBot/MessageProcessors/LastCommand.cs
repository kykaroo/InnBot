using InnBot.UserDataSaving;
using Microsoft.Extensions.DependencyInjection;

namespace InnBot.MessageProcessors;

public class LastCommand(IRepository repository, IServiceProvider serviceProvider) : ICommand
{
    public string CommandName => "/last";
 
    private IReadOnlyList<ICommand>? _commands;

    public Task<string[]> ProcessMessage(long fromId, string[] messageText)
    {
        var lastCommand = repository.GetLastCommand(fromId).Split(' ');
        _commands ??= serviceProvider.GetServices<ICommand>().ToList();

        return string.IsNullOrEmpty(lastCommand[0])
            ? Task.FromResult<string[]>(["Вы еще не вводили никаких команд"])
            : _commands.First(x => x.CommandName == lastCommand[0]).ProcessMessage(fromId, lastCommand);
    }
}