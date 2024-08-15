using InnBot.MessageProcessors;
using InnBot.MessageProcessors.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace InnBot;

public class MessageHandler(IServiceProvider serviceProvider, ILogger<MessageHandler> logger) : IUpdateHandler
{
    private IReadOnlyList<CommandMiddleware>? _middlewares;
    private IReadOnlyList<ICommand>? _commands;
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            _commands ??= serviceProvider.GetServices<ICommand>().ToList();
            _middlewares ??= serviceProvider.GetServices<CommandMiddleware>().ToList();

            if (update.Message?.From == null || update.Message.Text == null)
            {
                return;
            }

            var text = update.Message.Text.Trim().ToLower().Split();

            var chain = HandleMessage;

            for (var i = _middlewares.Count - 1; i >= 0; i--)
            {
                var next = chain;
                var current = _middlewares[i];
                chain = (fromId, args) => current.Execute(fromId, args, next);
            }

            foreach (var reply in await chain(update.Message.From.Id, text))
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    reply, cancellationToken: cancellationToken);
            }
        }
        catch (Exception e)
        {
            logger.LogError(e, "Ошибка при обработке сообщения от пользователя {UserName} ({UserId})\nСодержание сообщения: {Message}",
                update.Message.From.Username, update.Message.From.Id,update.Message.Text);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id,
                $"Некорректная команда: \"{update.Message.Text}\"\nДля вывода справки о доступных командах введите команду \"/help\" ", cancellationToken: cancellationToken);
        }
    }

    private Task<string[]> HandleMessage(long fromId, string[] messageText)
    {
        return _commands.First(x => x.CommandName == messageText[0]).ProcessMessage(fromId, messageText);
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception);
        return Task.CompletedTask;
    }
}