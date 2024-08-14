using Newtonsoft.Json;

namespace InnBot.InnApiClasses;

public class ItemsDto
{
    [JsonProperty("ИП")] public IndividualEntrepreneur? IndividualEntrepreneur { get; set; }
    [JsonProperty("ЮЛ")] public LegalEntity? LegalEntity { get; set; }
}