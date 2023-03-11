using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace Qiao.Utils;
internal static class BotHelper
{
    internal static async Task SendMessageAsync(this ITelegramBotClient botClient, long chatId, int messageThreadId, string message, int? reply = default)
    {
        while (true)
        {
            try
            {
                await botClient.SendTextMessageAsync(chatId, message, messageThreadId: (await botClient.GetChatAsync(chatId)).IsForum ?? false ? messageThreadId : default, parseMode: ParseMode.MarkdownV2, replyToMessageId: reply);
                break;
            }
            catch (ApiRequestException ex)
            {
                Main.Logger.Warn.WriteLine(Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("bot.failed.messagesend", ex.Message));
                break;
            }
            catch (RequestException) { }
            catch (AggregateException ex)
            {
                if (ex.WriteAllException())
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                Main.Logger.Warn.WriteLine(Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("bot.failed.messagesend", ex.Message));
                Main.Logger.Debug.WriteLine(ex);
            }
        }
    }
    internal static bool WriteAllException(this AggregateException ex)
    {
        bool rt = false;
        foreach (Exception innerEx in ex.InnerExceptions)
        {
            try
            {
                throw innerEx;
            }
            catch (ApiRequestException)
            {
                rt = true;
            }
            catch (RequestException) { continue; }
            catch (AggregateException exception)
            {
                rt = exception.WriteAllException() || rt;
                continue;
            }
            Main.Logger.Warn.WriteLine(Main.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("bot.failed.messagesend", innerEx.Message));
        }
        return rt;
    }
}
