using LiteLoader.RemoteCall;
using MessageSync.Utils;
using Qiao;

namespace MessageSync.Functions;
internal static class ExportFunc
{
    internal static void Setup()
    {
        RemoteCallAPI.ExportFunc(Main.pluginName, "SendMessage", (List<Valuetype> message) =>
        {
            _ = Bot.Client.SendMessageAsync(Main.Config.ChatId, Main.Config.MessageThreadId, message[0]);
            return new Valuetype();
        });
    }
}
