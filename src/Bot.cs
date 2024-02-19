using Microsoft.Extensions.Logging;
using SourceLand.Qiao.Utils;
using System.Globalization;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;

namespace SourceLand.Qiao;

public static class Bot
{
    public static Lazy<TelegramBotClient> Client { get; } = new(() =>
    {
        Config? config = Config.Shared.Value;
        return new(config!.Token, string.IsNullOrWhiteSpace(config.ProxyUrl)
            ? default
            : new(new HttpClientHandler
            {
                Proxy = new WebProxy(config.ProxyUrl, true)
            }));
    });

    public static event EventHandler<Update>? Received;

    internal static void Initialize()
    {
        Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    Client.Value.StartReceiving((botClient, update, _) => Received!(botClient, update),
                        (_, exception, _) =>
                        {
                            switch (exception)
                            {
                                case ApiRequestException ex:
                                    Main.Logger.Value.LogError(
                                        L10nProvider.Shared.Value[CultureInfo.CurrentCulture.Name][
                                            "bot.failed.listenstart"], ex.Message);
                                    break;
                                case RequestException:
                                    break;
                                case AggregateException ex:
                                    ex.WriteAllException("bot.failed.listenstart");
                                    break;
                                default:
                                    Main.Logger.Value.LogError(
                                        L10nProvider.Shared.Value[CultureInfo.CurrentCulture.Name][
                                            "bot.failed.listenstart"], exception.Message);
                                    break;
                            }
                        });
                    break;
                }
                catch (ApiRequestException ex)
                {
                    Main.Logger.Value.LogError(
                        L10nProvider.Shared.Value[CultureInfo.CurrentCulture.Name]["bot.failed.listenstart"],
                        ex.Message);
                }
                catch (RequestException)
                {
                }
                catch (AggregateException ex)
                {
                    ex.WriteAllException("bot.failed.listenstart");
                }
                catch (Exception ex)
                {
                    Main.Logger.Value.LogError(
                        L10nProvider.Shared.Value[CultureInfo.CurrentCulture.Name]["bot.failed.listenstart"],
                        ex.Message);
                }
            }
        });
    }
}