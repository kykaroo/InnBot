using System.Net;
using System.Text;
using InnBot.Abstractions;
using Newtonsoft.Json;

namespace InnBot.CompanyInfoService;

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
            return $"Ошибка при запросе сторонему сервису (ИНН: {messageText})";
        }

        var text = string.Empty;

        var legalEntity = queryResult.ItemsDto[0].LegalEntity;
        
        if (legalEntity != null)
        {
            text = $"{legalEntity.FullName}\nИНН: {legalEntity.Inn}\nАдрес: {legalEntity.Address.FullAddress}";
        }
        
        var individualEntrepreneur = queryResult.ItemsDto[0].IndividualEntrepreneur;
        
        if (individualEntrepreneur != null)
        {
            text = $"{individualEntrepreneur.FullFio}ИНН: \n{individualEntrepreneur.Inn}\nАдрес: {individualEntrepreneur.Address.FullAddress}";
        }

        return text;
    }

    public async Task<string> GetCompanyCodesByInn(string messageText)
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
            var builder = new StringBuilder();
            
            builder.AppendLine($"{legalEntity.FullName}\nИНН: {legalEntity.Inn}\nОКВЭД:");

            var list = legalEntity.AdditionalActivity
                .Append(legalEntity.MainActivity)
                .OrderByDescending(x => x.Name)
                .Select(companyActivity => $"{companyActivity.Name} : {companyActivity.Code}");
            
            builder.AppendJoin("\n", list);

            text = builder.ToString();
        }
        
        var individualEntrepreneur = queryResult.ItemsDto[0].IndividualEntrepreneur;
        
        if (individualEntrepreneur != null)
        {
            var builder = new StringBuilder();
            
            builder.AppendLine($"{individualEntrepreneur}\nИНН: {individualEntrepreneur.Inn}\nОКВЭД:");

            var list = individualEntrepreneur.AdditionalActivity
                .Append(individualEntrepreneur.MainActivity)
                .OrderByDescending(x => x.Name)
                .Select(individualEntrepreneurActivity =>
                    $"{individualEntrepreneurActivity.Name} : {individualEntrepreneurActivity.Code}");

            builder.AppendJoin("\n", list);

            text = builder.ToString();
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