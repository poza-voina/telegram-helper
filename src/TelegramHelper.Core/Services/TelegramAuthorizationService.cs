using TdLib;
using TelegramHelper.Core.Services.Interfaces;
using static TdLib.TdApi;

namespace TelegramHelper.Core.Services;

public class TelegramAuthorizationService(TdClient client) : ITelegramAuthorizationService
{
    public async Task SendCodeAsync(string code)
    {
        await client.ExecuteAsync(new CheckAuthenticationCode
        {
            Code = code
        });
    }

    public async Task SendPasswordAsync(string password)
    {
        await client.ExecuteAsync(new CheckAuthenticationPassword
        { Password = password }
        );
    }
}
