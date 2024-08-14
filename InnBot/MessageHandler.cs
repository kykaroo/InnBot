using InnBot.UserDataSaving;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace InnBot;

public class MessageHandler(string hostInfo, IRepository repository, ICompanyInfoService companyInfoService) : IUpdateHandler
{
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
                repository.SetLastCommand(fromId, "/start");
                return ["Приветствую! Для вывода справки введите /help"];
            
            case "/help":
                repository.SetLastCommand(fromId, "/help");
                return
                [
                    "/start - начало общения с ботом \n" +
                              "/hello - вывести имя, фамилию email и ссылку на github хоста \n" +
                              "/inn - получить наименования и адреса компаний по ИНН (можно использовать несколько ИНН через пробел) \n" +
                              "/last - повторить последнее действие бота \n" +
                              "/help - справка о доступных командах \n"
                ];
            
            case "/hello":
                repository.SetLastCommand(fromId, "/hello");
                return [hostInfo];
            
            case "/inn": 
                repository.SetLastCommand(fromId, "/inn");

                if (messageText.Length < 2)
                {
                    return ["Введите ИИН после команды /inn"];
                }

                var results = messageText.Where((t, i) => i != 0).Select(companyInfoService.GetCompanyInfoByInn)
                    .ToArray();

                return await Task.WhenAll(results);
            
            case "/last":
                var lastCommand = repository.GetLastCommand(fromId);

                return string.IsNullOrEmpty(lastCommand) 
                    ? ["Вы еще не вводили никаких команд"]
                    : await GetReplyText(fromId, [lastCommand]);
            
            case "/okved":
                repository.SetLastCommand(fromId, "/okved");
                
                if (messageText.Length < 2)
                {
                    return ["Введите ИИН после команды /okved"];
                }

                var results1 = messageText
                    .Where((t, i) => i != 0)
                    .Select(companyInfoService.GetCompanyCodesByInn)
                    .ToArray();

                return await Task.WhenAll(results1);
        }

        return [];
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception);
        return Task.CompletedTask;
    }
}