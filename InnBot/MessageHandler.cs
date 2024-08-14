using System.Net;
using InnBot.InnApiClasses;
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
        var replyText = await GetReplyText(update.Message.From.Id, text);

        foreach (var reply in replyText)
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id,
                reply, cancellationToken: cancellationToken);
        } 
    }

    private async Task<string[]> GetReplyText(long fromId, string[] messageText)
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

                var results = messageText.Where((t, i) => i != 0).Select(GetInnInfo).ToArray();

                return await Task.WhenAll(results);
            
            case "/last":
                var lastCommand = _repository.GetLastCommand(fromId);

                return string.IsNullOrEmpty(lastCommand) 
                    ? ["Вы еще не вводили никаких команд"]
                    : await GetReplyText(fromId, [lastCommand]) ;
        }

        return [];
    }

    private async Task<string> GetInnInfo(string messageText)
    {
        QueryResult? queryResult;
        
        try
        {
            var result = await GetTextResponse(messageText);

            queryResult = JsonConvert.DeserializeObject<QueryResult>(result);

            if (queryResult == null || !queryResult.ItemsDto.Any())
            {
                return $"Компания с ИНН \"{messageText}\" не найдена";
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "Ошибка при запросе сторонему сервису";
        }

        var text = string.Empty;

        var legalEntity = queryResult.ItemsDto[0].LegalEntity;
        
        if (legalEntity != null)
        {
            text = $"{legalEntity.FullName} \n {legalEntity.Inn}";
        }
        
        var individualEntrepreneur = queryResult.ItemsDto[0].IndividualEntrepreneur;
        
        if (individualEntrepreneur != null)
        {
            text = $"{individualEntrepreneur.FullFio} \n {individualEntrepreneur.Inn}";
        }

        return text;
    }

    private async Task<string> GetTextResponse(string messageText)
    {
        var request =
            (HttpWebRequest)WebRequest.Create("https://api-fns.ru/api/egr?req=" + messageText + "&key=" + ApiKey);
        request.Method = "GET";
        var response = await request.GetResponseAsync();
        
        using var streamReader = new StreamReader(response.GetResponseStream());
            
        var result = await streamReader.ReadToEndAsync();
        return result;
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception);
        return Task.CompletedTask;
    }
}