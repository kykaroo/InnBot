using Newtonsoft.Json;

namespace InnBot.Abstractions;

public class Region
{
    [JsonProperty("Наим")] public string Naim { get; set; } = string.Empty;
}