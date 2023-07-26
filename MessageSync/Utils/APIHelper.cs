using System.Globalization;
using LiteLoader.RemoteCall;
using MC;

namespace MessageSync.Utils;

internal static class APIHelper
{
    public static string GetUserName(Player player)
    {
        string name = player.RealName;
        if (RemoteCallAPI.HasFunc("UserName", "Get"))
        {
            RemoteCallAPI.CallbackFn method = RemoteCallAPI.ImportFunc("UserName", "Get");
            name = method(new() { player });
        }
        return Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.name.player", name);
    }
}
