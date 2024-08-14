using Newtonsoft.Json;

namespace InnBot.Abstractions;

public class LegalAddress
{
    [JsonProperty("КодРегион")] public string RegionCode { get; set; } = string.Empty;
    [JsonProperty("Индекс")] public string Index { get; set; } = string.Empty;
    [JsonProperty("АдресПолн")] public string FullAddress { get; set; } = string.Empty;
    [JsonProperty("ИдНомФИАС")] public string IdNomFias { get; set; } = string.Empty;
    [JsonProperty("АдресДетали")] public AddressDetails AddressDetails { get; set; } = new();
    [JsonProperty("Дата")] public string Data { get; set; } = string.Empty;
}