namespace TelegramHelper.Core.ObjectStorage;

public class InitializeClientOptions
{
    public required InitializeTelegramClientOptions InitializeOptions { get; set; }
    public required AuthorizeOptions AuthorizeOptions { get; set; }
}
