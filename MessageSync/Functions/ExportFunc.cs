using LiteLoader.RemoteCall;
using MessageSync.Utils;
using Qiao;

namespace MessageSync.Functions;
internal static class ExportFunc
{
    internal static void Setup()
    {
        RemoteCallAPI.ExportAs(Main.pluginName, "SendMessage", (string message) =>
        {
            _ = Bot.Client.SendMessageAsync(Main.Config.ChatId, Main.Config.MessageThreadId, message);
        });
    }
}
