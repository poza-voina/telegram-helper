using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
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

var initOptions = new InitializeClientOptions
{
	InitializeOptions = new InitializeTelegramClientOptions
	{
		DatabaseDirectory = "tg_db_1",
		FilesDirectory = "files",
		ApiId = 0,
		ApiHash = ""
	},
	AuthorizeOptions = new AuthorizeOptions { PhoneNumber = "" },
	Id = Guid.NewGuid(),
};

var connectionString = "Server=localhost;Port=5432;Database=telegram-helper;User Id=postgres;Password=postgres;";


Log.Logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.File("Test.log")
			.WriteTo.Console()
			.CreateLogger();

IHost host = Host.CreateDefaultBuilder(args)
			.ConfigureServices((context, services) =>
			{
				services.AddScoped<ITelegramUpdateExecutor, TelegramUpdateExecutor>();

				services.AddScoped(x => new TelegramClientContext());
				services.AddScoped<IUpdateAuthorizationStateExecutor, UpdateAuthorizationStateExecutor>();
				services.AddScoped<IUpdateChatFoldersExecutor, UpdateChatFoldersExecutor>();
				services.AddSingleton<ITelegramClientDispatcher, TelegramClientDispatcher>();
				services.AddScoped<ICurrentStaticFolderFilterRepository, CurrentStaticFolderFilterRepository>();
				services.AddScoped<ICurrentDynamicFolderFilterRepository, CurrentDynamicFolderFilterRepository>();
				services.AddScoped<ICurrentFolderRepository, CurrentFolderRepository>();
				services.AddScoped<IOwnerRepository, OwnerRepository>();
				services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
				services.AddScoped<IFolderService, FolderService>();
				services.AddScoped<PostgresContext>(sp =>
				{
					var options = new DbContextOptionsBuilder<PostgresContext>()
						.UseNpgsql(connectionString)
						.Options;
					return new PostgresContext(options);
				});

				services.AddLogging(x =>
				{
					x.ClearProviders();
					x.AddSerilog();
				});

				services.AddScoped<TdClient>(sp =>
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
				services.AddScoped<ITelegramAuthorizationService, TelegramAuthorizationService>();
			})
			.Build();
host.Start();
var services = host.Services;

var clientDispatcher = services.GetRequiredService<ITelegramClientDispatcher>();
var clientId = clientDispatcher.CreateClient(initOptions);

clientDispatcher.GetAuthorizationStateExecutor(clientId).WaitEvent += async (sender, state) =>
{
	var telegramAuthorizationService = clientDispatcher.GetAuthorizationService(clientId);

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
var client = await clientDispatcher.Wait(clientId);

var folderService = client.ServiceProvider.GetRequiredService<IFolderService>();

var folders = await folderService.GetCurrentFolders();

//await folderService.FolderToArchiveAsync(folders.First(x => x.FolderName == "test").Id);

await folderService.FolderFromArhive(folders.First(x => x.FolderName == "test").Id);

await Task.Delay(Timeout.Infinite);