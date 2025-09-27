namespace TelegramHelper.Abstractions.Models;

public class OwnerModel : IDatabaseModel<long>
{
    public long Id { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public virtual IEnumerable<CurrentFolderModel> CurrentFolders { get; set; } = [];
}
