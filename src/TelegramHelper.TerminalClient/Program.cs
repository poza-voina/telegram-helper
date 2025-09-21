// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TdLib;
using TelegramHelper.Core.Executors;
using TelegramHelper.Core.Executors.Interfaces;
using TelegramHelper.Core.ObjectStorage;
using TelegramHelper.Core.ObjectStorage.Interfaces;
using TelegramHelper.Core.Services;
using TelegramHelper.Core.Services.Interfaces;
using TelegramHelper.Infrastructure;
using TelegramHelper.Infrastructure.Repositories;
using TelegramHelper.Infrastructure.Repositories.Interfaces;
using static TdLib.TdApi;
using static TdLib.TdApi.AuthorizationState;


var initOptions =  new InitializeClientOptions
{
    InitializeOptions = new InitializeTelegramClientOptions
    {
        DatabaseDirectory = "tg_db",
        FilesDirectory = "files",
        ApiId = 0,
        ApiHash = ""
    },
    AuthorizeOptions = new AuthorizeOptions { PhoneNumber = "" }
};

var connectionString = "Server=localhost;Port=5432;Database=telegram;User Id=postgres;Password=postgres;";

IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<ITelegramClient, TelegramClient>();
                services.AddSingleton<ITelegramUpdateExecutor, TelegramUpdateExecutor>();
                services.AddSingleton<IUpdateChatFoldersExecutor, UpdateChatFoldersExecutor>();
                services.AddSingleton<IUpdateAuhorizationStateExecutor, UpdateAuthorizationStateExecutor>();
                services.AddSingleton<IChatFolderRepository, ChatFolderRepository>();
                services.AddSingleton<IFolderRepository, FolderRepository>();
                services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
                services.AddSingleton<ClientServiceFactory>();
                services.AddSingleton<PostgresContext>(sp =>
                {
                    var options = new DbContextOptionsBuilder<PostgresContext>()
                        .UseNpgsql(connectionString)
                        .Options;
                    return new PostgresContext(options);
                });

                services.AddSingleton<TdClient>(sp =>
                {
                    var client = new TdClient();

                    client.SetLogStreamAsync(
                        new TdApi.LogStream.LogStreamFile
                    {
                        Path = "tdlib_log.txt",
                        MaxFileSize = 10_000_000
                    });

                    client.ExecuteAsync(new TdApi.SetLogVerbosityLevel { NewVerbosityLevel = 0 }).Wait();

                    return client;
                });
                services.AddSingleton<ITelegramAuthorizationService, TelegramAuthorizationService>();
                services.AddSingleton<InitializeClientOptions>(initOptions);
            })
            .Build();
host.Start();
var services = host.Services;

services.GetRequiredService<IUpdateAuhorizationStateExecutor>().WaitEvent += async (sender, state) =>
{
    var telegramAuthorizationService = services.GetRequiredService<ITelegramAuthorizationService>();
    switch (state)
    {
        case AuthorizationStateWaitPassword:
            Console.Write("Введите пароль: ");
            var password = Console.ReadLine();
            await telegramAuthorizationService.SendPasswordAsync(password!);
            break;

        case AuthorizationStateWaitCode:
            Console.Write("Введите код из SMS/Telegram: ");
            var code = Console.ReadLine();
            await telegramAuthorizationService.SendCodeAsync(code!);
            break;

        case AuthorizationStateReady:
            break;

        default:
            throw new Exception("Нет обработчика тут");
    }
};

ITelegramClient client = await services.GetRequiredService<ClientServiceFactory>().CreateAsync(initOptions);
await client.Test();
await Task.Delay(Timeout.Infinite);