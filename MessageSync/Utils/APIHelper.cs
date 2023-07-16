using LiteLoader.RemoteCall;
using MC;

namespace MessageSync.Utils;

internal static class APIHelper
{
    public static string GetUserName(Player player)
    {
        if (!RemoteCallAPI.HasFunc("UserName", "GetFromXuid"))
        {
            return player.RealName;
        }
        RemoteCallAPI.CallbackFn method = RemoteCallAPI.ImportFunc("UserName", "GetFromXuid");
        return method(new() { player.Xuid });
    }
}
