using Microsoft.EntityFrameworkCore;
using TelegramHelper.Abstractions.Exceptions;
using TelegramHelper.Contracts.Folders;
using TelegramHelper.Core.ObjectStorage;
using TelegramHelper.Core.ObjectStorage.Interfaces;
using TelegramHelper.Core.ObjectStorage.Mappers;
using TelegramHelper.Core.Services.Interfaces;
using TelegramHelper.Infrastructure.Repositories.Interfaces;
using static TdLib.TdApi;

namespace TelegramHelper.Core.Services;

public class FolderService : IFolderService
{
	private readonly ITelegramClient _telegramClient;
	private readonly ICurrentFolderRepository _currentFolderRepository;

	public FolderService(
		ITelegramClientDispatcher dispatcher,
		TelegramClientContext context,
		ICurrentFolderRepository currentFolderRepository)
	{
		var id = context.InitializeClientOptions?.Id;
		NotFoundException.ThrowIfNull(id);

		_telegramClient = dispatcher.GetReadyTelegramClient(id.Value);
		_currentFolderRepository = currentFolderRepository;
	}

	public async Task FolderToArchiveAsync(long id)
	{
		var model = await _currentFolderRepository
			.GetAll()
			.Include(x => x.DynamicFilters)
			.Include(x => x.StaticFilters)
			.FirstOrDefaultAsync(x => x.Id == id);

		NotFoundException.ThrowIfNull(model);
		NotFoundException.ThrowIfNull(model.TelegramFolderId);

		model.IsArchive = true;
		var folderId = model.TelegramFolderId;
		model.TelegramFolderId = null;
		
		await _currentFolderRepository.UpdateAsync(model);

		await _telegramClient.RemoveFolderAsync(folderId.Value);
	}

	public async Task FolderFromArhive(long id)
	{
		var model = await _currentFolderRepository
			.GetAll()
			.Include(x => x.DynamicFilters)
			.Include(x => x.StaticFilters)
			.FirstOrDefaultAsync(x => x.Id == id);

		NotFoundException.ThrowIfNull(model);

		var telegramFolderInfo = await _telegramClient.CreateFolderAsync(model.CurrentFolderModel_To_TelegramFolder());

		model.IsArchive = false;
		model.TelegramFolderId = telegramFolderInfo.Id;
		await _currentFolderRepository.UpdateAsync(model);
	}

	public async Task<IEnumerable<FolderView>> GetCurrentFolders()
	{
		var query = _currentFolderRepository
			.GetAll()
			.Where(x => x.OwnerId == _telegramClient.Context.OwnerId)
			.Select(x => x.CurrentFolderModel_To_FolderView());

		return await query.ToListAsync();
	}
}
