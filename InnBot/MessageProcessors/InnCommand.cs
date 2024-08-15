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

        var result = new List<string>();
        
        for (var i = 0; i < messageText.Length; i++)
        {
            if (i == 0) continue;
            
            var text = messageText[i];

            if (text.Length is 10 or 12)
            {
                result.Add(await companyInfoService.GetCompanyInfoByInn(text));
                continue;
            }
            
            result.Add($"Некорректный ИНН: {text}");
        }

        return result.ToArray();
    }
}