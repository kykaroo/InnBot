using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;

namespace InnBot;

public class TelegramBot(TelegramBotClient telegramBotClient, MessageHandler messageHandler, ILogger<TelegramBot> logger) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Application started");

            telegramBotClient.StartReceiving(messageHandler, cancellationToken: cancellationToken);

            logger.LogInformation("Ready to receiving messages");

            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Application error");
            throw;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await telegramBotClient.CloseAsync(cancellationToken: cancellationToken);
        
        logger.LogInformation("Stop receiving messages");
    }
}