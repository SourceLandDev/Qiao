using LiteLoader.RemoteCall;
using Qiao.Utils;

namespace Qiao.Functions;
internal static class ExportFunc
{
    internal static void Setup()
    {
        _ = RemoteCallAPI.ExportAs(Main.pluginName, "SendMessageAsync",
            (long chatId, int messageThreadId, string message, int reply) =>
            {
                _ = Main.BotClient.SendMessageAsync(chatId, messageThreadId, message, reply);
            });
        _ = RemoteCallAPI.ExportAs(Main.pluginName, "SendMessage",
            (long chatId, int messageThreadId, string message, int reply) => Main.BotClient.SendMessageAsync(chatId, messageThreadId, message, reply).Wait());
    }
}
