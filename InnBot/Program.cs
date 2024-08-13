using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("secrets.json", true)
    .AddEnvironmentVariables()
    .Build();

var builder = Host.CreateApplicationBuilder(args);

builder.Build();