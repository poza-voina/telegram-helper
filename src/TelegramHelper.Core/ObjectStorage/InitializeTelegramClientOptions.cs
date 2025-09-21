namespace TelegramHelper.Core.ObjectStorage;

public class InitializeTelegramClientOptions
{
    public required string DatabaseDirectory { get; set; }
    public required string FilesDirectory { get; set; }
    public required int ApiId { get; set; }
    public required string ApiHash { get; set; }
}
