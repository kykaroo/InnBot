using InnBot.CompanyInfoService;

namespace InnBot.MessageProcessors;

public class OkvedCommand(ICompanyInfoService companyInfoService) : ICommand
{
    public string CommandName => "/okved";

    public async Task<string[]> ProcessMessage(long fromId, string[] messageText)
    {
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
}