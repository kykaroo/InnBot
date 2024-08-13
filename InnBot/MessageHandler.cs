using System.Net;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace InnBot;

public class MessageHandler(string hostInfo, string ApiKey) : IUpdateHandler
{
    private readonly IRepository _repository = new DictionaryRepository();


    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message?.From == null || update.Message.Text == null)
        {
            return;
        }

        var text = update.Message.Text.Trim().ToLower().Split();
        var replyText = GetReplyText(update.Message.From.Id, text);

        foreach (var reply in replyText)
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id,
                reply, cancellationToken: cancellationToken);
        } 
    }

    private string[] GetReplyText(long fromId, string[] messageText)
    {
        switch (messageText[0])
        {
            case "/start":
                _repository.SetLastCommand(fromId, "/start");
                return ["Приветствую! Для вывода справки введите /help"];
            
            case "/help":
                _repository.SetLastCommand(fromId, "/help");
                return
                [
                    "/start - начало общения с ботом \n" +
                              "/hello - вывести имя, фамилию email и ссылку на github хоста \n" +
                              "/inn - получить наименования и адреса компаний по ИНН (можно использовать несколько ИНН через пробел) \n" +
                              "/last - повторить последнее действие бота \n" +
                              "/help - справка о доступных командах \n"
                ];
            
            case "/hello":
                _repository.SetLastCommand(fromId, "/hello");
                return [hostInfo];
            
            case "/inn": 
                _repository.SetLastCommand(fromId, "/inn");

                if (messageText.Length < 2)
                {
                    return ["Введите ИИН после команды /inn"];
                }

                var results = messageText.Where((t, i) => i != 0).Select(GetInnInfo).ToList();

                return results.ToArray();
            
            case "/last":
                var lastCommand = _repository.GetLastCommand(fromId);

                return string.IsNullOrEmpty(lastCommand) 
                    ? ["Вы еще не вводили никаких команд"]
                    : GetReplyText(fromId, [lastCommand]) ;
        }

        return [];
    }

    private string GetInnInfo(string messageText)
    {
        var request = (HttpWebRequest)WebRequest.Create("https://api-fns.ru/api/search?q=" + messageText + ApiKey);
        request.Method = "GET";
        var response = request.GetResponse();
                
        var result = string.Empty;

        using var streamReader = new StreamReader(response.GetResponseStream());
        
        result = streamReader.ReadToEnd();
        
        //TODO Десериализация объекта

        return result;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception);
        return Task.CompletedTask;
    }
}