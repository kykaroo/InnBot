using Newtonsoft.Json;

namespace InnBot.Abstractions;

public class AddressDetails
{
    [JsonProperty("Регион")] public Region Region { get; set; } = new();
    [JsonProperty("Город")] public City City { get; set; } = new();
    [JsonProperty("Улица")] public Street Street { get; set; } = new();
    [JsonProperty("Дом")] public string House { get; set; } = string.Empty;
    [JsonProperty("Помещ")] public string Room { get; set; } = string.Empty;
}