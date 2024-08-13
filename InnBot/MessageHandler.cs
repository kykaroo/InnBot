using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace InnBot;

public class MessageHandler(string hostInfo) : IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message == null)
        {
            return;
        }
        
        switch (update.Message.Text)
        {
            case "/start":
                await botClient.SendTextMessageAsync(update.Message.Chat.Id,
                    "Приветствую! Для вывода справки введите /help", cancellationToken: cancellationToken);
                break;
            
            case "/help":
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, 
                    "/start - начало общения с ботом \n" + 
                    "/hello - вывести имя, фамилию email и ссылку на github хоста \n" +
                    "/inn - получить наименования и адреса компаний по ИНН (можно использовать несколько ИНН через пробел) \n" +
                    "/last - повторить последнее действие бота \n" +
                    "/help - справка о доступных командах \n", 
                    cancellationToken: cancellationToken);
                break;
            
            case "/hello":
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, hostInfo,
                    cancellationToken: cancellationToken);
                break;
            
            case "/inn":
                //TODO Реализовать функционал
                break;
            
            case "/last":
                //TODO Реализовать функционал
                break;
        }
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}