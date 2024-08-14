using Newtonsoft.Json;

namespace InnBot.Abstractions;

public class CompanyActivity
{
    [JsonProperty("Код")] public string Code { get; set; } = string.Empty;
    [JsonProperty("Текст")] public string Text { get; set; } = string.Empty;
    [JsonProperty("Дата")] public string Date { get; set; } = string.Empty;
}