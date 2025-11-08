namespace TelegramHelper.Abstractions.Exceptions;

public class TelegramClientException : Exception
{
	public TelegramClientException()
	{
	}

	public TelegramClientException(string? message) : base(message)
	{
	}

	public TelegramClientException(string? message, Exception? innerException) : base(message, innerException)
	{
	}

	public static void ThrowIfStatusOnWorked(TelegramClientStatus? status)
	{
		if (status != TelegramClientStatus.Ready)
		{
			throw new TelegramClientException("client not working");
		}
	}
}
