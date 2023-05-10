using LiteLoader.RemoteCall;
using Qiao.Utils;

namespace Qiao.Functions;
internal static class ExportFunc
{
    internal static void Setup()
    {
        _ = RemoteCallAPI.ExportAs(Main.pluginName, "SendMessage", (string message) =>
        {
            _ = Main.BotClient.SendMessageAsync(Main.Config.ChatId, Main.Config.MessageThreadId, message);
        });
    }
}
