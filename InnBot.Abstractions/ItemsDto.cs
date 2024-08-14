using Newtonsoft.Json;

namespace InnBot.Abstractions;

public class ItemsDto
{
    [JsonProperty("ИП")] public IndividualEntrepreneur? IndividualEntrepreneur { get; set; }
    [JsonProperty("ЮЛ")] public LegalEntity? LegalEntity { get; set; }
}