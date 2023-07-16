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
        Func<string, string> method = RemoteCallAPI.ImportAs<Func<string, string>>("UserName", "GetFromXuid");
        return method(player.Xuid);
    }
}
