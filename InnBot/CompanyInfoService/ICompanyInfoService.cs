namespace InnBot.CompanyInfoService;

public interface ICompanyInfoService
{
    Task<string> GetCompanyInfoByInn(string messageText);
    Task<string> GetCompanyCodesByInn(string messageText);
}