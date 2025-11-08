namespace TelegramHelper.Abstractions.Models;

public interface IDatabaseModel<T>
{
	T Id { get; set; }
}