using InnBot.CompanyInfoService;

namespace InnBot.MessageProcessors;

public class InnCommand(ICompanyInfoService companyInfoService) : ICommand
{
    public string CommandName => "/inn";

    public async Task<string[]> ProcessMessage(long fromId, string[] messageText)
    {
        if (messageText.Length < 2)
        {
            return ["Введите ИИН после команды /inn"];
        }

        var results = messageText.Where((t, i) => i != 0).Select(companyInfoService.GetCompanyInfoByInn)
            .ToArray();

        return await Task.WhenAll(results);
    }
}