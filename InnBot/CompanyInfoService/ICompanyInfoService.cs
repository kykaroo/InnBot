namespace InnBot;

public interface ICompanyInfoService
{
    Task<string> GetCompanyInfoByInn(string messageText);
    Task<string> GetCompanyCodesByInn(string messageText);
}