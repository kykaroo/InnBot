using Microsoft.Extensions.Logging;

namespace InnBot.MessageProcessors.Middleware;

public class LoggerMiddleware(ILogger<LoggerMiddleware> logger) : CommandMiddleware
{
    public override async Task<string[]> Execute(long fromId, string[] messageText, Func<long, string[], Task<string[]>> onNext)
    {
        var answer = await onNext.Invoke(fromId, messageText);

        logger.LogInformation("Сообщение от пользователя ({UserId})\nСодержание сообщения: {Message}\nОтвет: {Answer}", fromId,
            string.Join(' ', messageText), string.Join(' ', answer));
        
        return answer;
    }
}