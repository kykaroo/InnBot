namespace InnBot.MessageProcessors;

public class HelpCommand : ICommand
{
    public string CommandName => "/help";

    public Task<string[]> ProcessMessage(long fromId, string[] messageText)
    {
        return Task.FromResult<string[]>([
            "/start - начало общения с ботом \n" +
            "/hello - вывести имя, фамилию email и ссылку на github хоста \n" +
            "/inn - получить наименования и адреса компаний по ИНН (можно использовать несколько ИНН через пробел) \n" +
            "/last - повторить последнее действие бота \n" +
            "/help - справка о доступных командах \n"
        ]);
    }
}