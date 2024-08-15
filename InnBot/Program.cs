using InnBot;
using InnBot.CompanyInfoService;
using InnBot.MessageProcessors;
using InnBot.MessageProcessors.InfoBuffer;
using InnBot.MessageProcessors.Middleware;
using InnBot.UserDataSaving;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Telegram.Bot;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile("secrets.json", true)
    .AddJsonFile("loggersettings.json", true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.AddSerilog(x => x
    .ReadFrom.Configuration(builder.Configuration));

builder.Services.AddSingleton<IRepository, DictionaryRepository>();

builder.Services.AddSingleton<ICompanyInfoService, CompanyInfoService>(x =>
    ActivatorUtilities.CreateInstance<CompanyInfoService>(x, builder.Configuration["INN_API_KEY"],
        builder.Configuration["INN_API_URL"]));

builder.Services.AddSingleton<TelegramBotClient>(x =>
    ActivatorUtilities.CreateInstance<TelegramBotClient>(x, builder.Configuration["TELEGRAM_API"]));

builder.Services.AddSingleton<MessageHandler>();

builder.Services.AddSingleton<HelloCommandInfoBuffer>(x =>
    ActivatorUtilities.CreateInstance<HelloCommandInfoBuffer>(x, builder.Configuration["HOST_INFO"]));

builder.Services.AddSingleton(typeof(ICommand), typeof(HelloCommand));
builder.Services.AddSingleton(typeof(ICommand), typeof(InnCommand));
builder.Services.AddSingleton(typeof(ICommand), typeof(LastCommand));
builder.Services.AddSingleton(typeof(ICommand), typeof(OkvedCommand));
builder.Services.AddSingleton(typeof(ICommand), typeof(StartCommand));
builder.Services.AddSingleton(typeof(ICommand), typeof(HelpCommand));

builder.Services.AddSingleton(typeof(CommandMiddleware), typeof(LastCommandMiddleware));
builder.Services.AddSingleton(typeof(CommandMiddleware), typeof(LoggerMiddleware));

builder.Services.AddHostedService<TelegramBot>();

var app = builder.Build();

app.Run();