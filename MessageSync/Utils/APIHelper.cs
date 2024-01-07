using LiteLoader.RemoteCall;
using MC;
using System.Globalization;

namespace MessageSync.Utils;

internal static class APIHelper
{
    public static string GetUserName(Player player)
    {
        string name = player.RealName;
        if (!RemoteCallAPI.HasFunc("UserName", "Get"))
        {
            return Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.name.player", name.Escape());
        }

        RemoteCallAPI.CallbackFn method = RemoteCallAPI.ImportFunc("UserName", "Get");
        name = method([player]);

        return Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("message.name.player", name.Escape());
    }
}