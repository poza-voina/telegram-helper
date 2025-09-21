using System.Runtime.CompilerServices;

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

    public static void ThrowIfNull(object? value)
    {
        if (value is null)
        {
            throw new NotFoundException("сущность не найдена");
        }
    }
}
