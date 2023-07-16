using LiteLoader.RemoteCall;
using MC;

namespace MessageSync.Utils;

internal static class APIHelper
{
    public static string GetUserName(Player player)
    {
        if (!RemoteCallAPI.HasFunc("UserName", "Get"))
        {
            return player.RealName;
        }
        RemoteCallAPI.CallbackFn method = RemoteCallAPI.ImportFunc("UserName", "Get");
        return method(new() { player });
    }
}
