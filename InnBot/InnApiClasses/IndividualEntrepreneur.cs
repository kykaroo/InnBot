using Newtonsoft.Json;

namespace InnBot.InnApiClasses;

public class IndividualEntrepreneur
{
    [JsonProperty("ФИОПолн")] public string FullFio { get; set; } = string.Empty;
    [JsonProperty("ИНН")] public string Inn { get; set; } = string.Empty;
    [JsonProperty("ОснВидДеят")] public CompanyActivity MainActivity { get; set; } = new();
    [JsonProperty("ДопВидДеят")] public CompanyActivity[] AdditionalActivity { get; set; } = [];
}