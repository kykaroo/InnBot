using Newtonsoft.Json;

namespace InnBot.Abstractions;

public class IndividualEntrepreneurAddress
{
    [JsonProperty("КодРегион")] public string RegionCode { get; set; } = string.Empty;
    [JsonProperty("АдресПолн")] public string FullAddress { get; set; } = string.Empty;
    [JsonProperty("Дата")] public string Date { get; set; } = string.Empty;
}