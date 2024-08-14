using InnBot.MessageProcessors;
using InnBot.MessageProcessors.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace InnBot;

public class MessageHandler(IServiceProvider serviceProvider) : IUpdateHandler
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
            Console.WriteLine(e);
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