using TdLib;
using TelegramHelper.Abstractions.Models;

namespace TelegramHelper.Core.ObjectStorage.Mappers;

public static class OwnerMapper
{
	public static OwnerModel TelegramUserToOwnerModel(this TdApi.User src)
	{
		return new OwnerModel
		{
			Id = src.Id,
			PhoneNumber = src.PhoneNumber,
			FirstName = src.FirstName,
			LastName = src.LastName
		};
	}
}
