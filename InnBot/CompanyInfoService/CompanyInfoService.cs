using System.Net;
using InnBot.Abstractions;
using Newtonsoft.Json;

namespace InnBot;

public class CompanyInfoService(string apiKey, string apiUrl) : ICompanyInfoService
{
    public async Task<string> GetCompanyInfoByInn(string messageText)
    {
        QueryResult? queryResult;
        
        try
        {
            var result = await GetWebTextResponse(messageText);

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

    private async Task<string> GetWebTextResponse(string messageText)
    {
        var request =
            (HttpWebRequest)WebRequest.Create($"{apiUrl}/egr?req={messageText}&key={apiKey}");
        request.Method = "GET";
        var response = await request.GetResponseAsync();
        
        using var streamReader = new StreamReader(response.GetResponseStream());
            
        var result = await streamReader.ReadToEndAsync();
        return result;
    }
}