using System.Diagnostics.CodeAnalysis;

namespace TelegramHelper.Abstractions.Exceptions;

public class NotFoundException : Exception
{
	public NotFoundException()
	{
	}

	public NotFoundException(string? message) : base(message)
	{
	}

	public NotFoundException(string? message, Exception? innerException) : base(message, innerException)
	{
	}

	public static void ThrowIfNull([NotNull] object? value)
	{
		if (value is null)
		{
			throw new NotFoundException($"Object {value?.GetType()} не найден");
		}
	}
	public static void ThrowIfNull<T>([NotNull] T? value)
	{
		if (value is null)
		{
			throw new NotFoundException($"Object {value?.GetType()} не найден");
		}
	}
}
