using InnBot;
using InnBot.UserDataSaving;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile("secrets.json", true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddSingleton<IRepository, DictionaryRepository>();

builder.Services.AddSingleton<ICompanyInfoService, CompanyInfoService>(x =>
    ActivatorUtilities.CreateInstance<CompanyInfoService>(x, builder.Configuration["INN_API_KEY"],
        builder.Configuration["INN_API_URL"]));

builder.Services.AddSingleton<MessageHandler>(x =>
    ActivatorUtilities.CreateInstance<MessageHandler>(x, builder.Configuration["HOST_INFO"]));

builder.Services.AddSingleton<TelegramBotClient>(x =>
    ActivatorUtilities.CreateInstance<TelegramBotClient>(x, builder.Configuration["TELEGRAM_API"]));

builder.Services.AddHostedService<TelegramBot>();

var app = builder.Build();

app.Run();