﻿using Newtonsoft.Json;

namespace InnBot.Abstractions;

public class LegalEntity
{
    [JsonProperty("НаимПолнЮЛ")] public string FullName { get; set; }  = string.Empty;
    [JsonProperty("ИНН")] public string Inn { get; set; } = string.Empty;
    [JsonProperty("ОснВидДеят")] public CompanyActivity MainActivity { get; set; } = new();
    [JsonProperty("ДопВидДеят")] public CompanyActivity[] AdditionalActivity { get; set; } = [];
    [JsonProperty("Адрес")] public LegalAddress Address { get; set; } = new();
}