using Newtonsoft.Json;

namespace InnBot.Abstractions;

public class City
{
    [JsonProperty("Тип")] public string Type { get; set; } = string.Empty;
    [JsonProperty("Наим")] public string Naim { get; set; } = string.Empty;
}