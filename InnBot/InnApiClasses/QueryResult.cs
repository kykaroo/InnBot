using Newtonsoft.Json;

namespace InnBot.InnApiClasses;
    
public class QueryResult
{
    [JsonProperty("items")] public ItemsDto[] ItemsDto { get; set; } = [];
}