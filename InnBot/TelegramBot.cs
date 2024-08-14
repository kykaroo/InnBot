using Microsoft.Extensions.Hosting;
using Telegram.Bot;

namespace InnBot;

public class TelegramBot(TelegramBotClient telegramBotClient, MessageHandler messageHandler) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        telegramBotClient.StartReceiving(messageHandler, cancellationToken: cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return telegramBotClient.CloseAsync(cancellationToken: cancellationToken);
    }
}