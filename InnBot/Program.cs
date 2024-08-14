using InnBot;
using InnBot.UserDataSaving;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("secrets.json", true)
    .AddEnvironmentVariables()
    .Build();

var bot = new TelegramBotClient(config["TELEGRAM_API"]);
bot.StartReceiving(new MessageHandler(config["HOST_INFO"], new DictionaryRepository(),
    new CompanyInfoService(config["INN_API_KEY"])));

var builder = Host.CreateApplicationBuilder(args);

builder.Build();

Console.ReadKey();

await bot.CloseAsync();