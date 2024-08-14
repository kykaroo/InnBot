using Newtonsoft.Json;

namespace InnBot.Abstractions;
    
public class QueryResult
{
    [JsonProperty("items")] public ItemsDto[] ItemsDto { get; set; } = [];
}