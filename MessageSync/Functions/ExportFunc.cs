using LiteLoader.RemoteCall;
using MessageSync.Utils;
using Qiao;
using Telegram.Bot.Types;

namespace MessageSync.Functions;
internal static class ExportFunc
{
    internal static void Setup()
    {
        RemoteCallAPI.ExportFunc(Main.pluginName, "SendMessage", (List<Valuetype> args) =>
        {
            Message message = Bot.Client.SendMessageAsync(Main.Config.ChatId, args.Count > 1 ? (int)args[1] switch
            {
                -2 => Main.Config.InfoThreadId,
                -1 => Main.Config.MessageThreadId,
                >= 0 => args[1],
                _ => throw new ArgumentOutOfRangeException("不存在的话题ID")
            } : default, args[0], args.Count > 2 ? args[2] : default(int)).Result;
            if (message is null)
            {
                return new();
            }
            return new(message.MessageId);
        });
    }
}
