namespace InnBot.MessageProcessors.Middleware;

public abstract class CommandMiddleware
{
    public abstract Task<string[]> Execute(long fromId, string[] messageText, Func<long, string[], Task<string[]>> onNext);
}