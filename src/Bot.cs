using Qiao.Utils;
using System.Globalization;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace Qiao;
public class Bot
{
    public static TelegramBotClient Client { get; private set; }
    public static event EventHandler<Update> Received;
    internal static void Initialize()
    {
        Client = new(Plugin.Config.Token, string.IsNullOrWhiteSpace(Plugin.Config.ProxyUrl) ? default : new(new HttpClientHandler
        {
            Proxy = new WebProxy(Plugin.Config.ProxyUrl, true)
        }));
        Client.StartReceiving((botClient, update, _) => Received(botClient, update),
            (_, exception, _) =>
        {
            switch (exception)
            {
                case ApiRequestException ex:
                    Plugin.Logger.Warn.WriteLine(Plugin.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("bot.failed.listenstart", ex.Message));
                    break;
                case RequestException:
                    break;
                case AggregateException ex:
                    ex.WriteAllException();
                    break;
                default:
                    Plugin.Logger.Warn.WriteLine(Plugin.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("bot.failed.listenstart", exception.Message));
                    Plugin.Logger.Debug.WriteLine(exception);
                    break;
            }
        });
    }
}
