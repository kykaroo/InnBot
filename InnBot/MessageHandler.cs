using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace InnBot;

public class MessageHandler(string hostInfo) : IUpdateHandler
{
    private readonly IRepository _repository = new DictionaryRepository();
    
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message?.From == null || update.Message.Text == null)
        {
            return;
        }

        var replyText = GetReplyText(update.Message.From.Id, update.Message.Text);

        if (string.IsNullOrEmpty(replyText))
        {
            return;
        }
        
        await botClient.SendTextMessageAsync(update.Message.Chat.Id,
            replyText, cancellationToken: cancellationToken); 
    }

    private string GetReplyText(long fromId, string messageText)
    {
        switch (messageText)
        {
            case "/start":
                _repository.SetLastCommand(fromId, "/start");
                return "Приветствую! Для вывода справки введите /help";
            
            case "/help":
                _repository.SetLastCommand(fromId, "/help");
                return "/start - начало общения с ботом \n" +
                       "/hello - вывести имя, фамилию email и ссылку на github хоста \n" +
                       "/inn - получить наименования и адреса компаний по ИНН (можно использовать несколько ИНН через пробел) \n" +
                       "/last - повторить последнее действие бота \n" +
                       "/help - справка о доступных командах \n";
            
            case "/hello":
                _repository.SetLastCommand(fromId, "/hello");
                return hostInfo;
            
            case "/inn":
                _repository.SetLastCommand(fromId, "/inn");
                //TODO Реализовать функционал
                break;
            
            case "/last":
                var lastCommand = _repository.GetLastCommand(fromId);

                return string.IsNullOrEmpty(lastCommand) 
                    ? "Вы еще не вводили никаких команд"
                    : GetReplyText(fromId, lastCommand) ;
        }

        return string.Empty;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception);
        return Task.CompletedTask;
    }
}