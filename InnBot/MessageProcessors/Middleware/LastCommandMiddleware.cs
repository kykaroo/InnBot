using InnBot.UserDataSaving;

namespace InnBot.MessageProcessors.Middleware;

public class LastCommandMiddleware(IRepository repository) : CommandMiddleware
{
    public override Task<string[]> Execute(long fromId, string[] messageText, Func<long, string[], Task<string[]>> onNext)
    {
        if (messageText[0] != "/last")
        {
            var text = string.Join(' ', messageText);
            repository.SetLastCommand(fromId, text);
        }

        return onNext.Invoke(fromId, messageText);
    }
}