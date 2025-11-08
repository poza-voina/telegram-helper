using TelegramHelper.Abstractions;

namespace TelegramHelper.Core.ObjectStorage;

public class TelegramClientContext
{
	public TelegramClientStatus? Status { get; set; }
	public long? OwnerId { get; set; }
	public InitializeClientOptions? InitializeClientOptions { get; set; }
}
