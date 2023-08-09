using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MessageSync.Utils;

internal static class BotHelper
{
    internal static async Task<Message> SendMessageAsync(this ITelegramBotClient botClient, long chatId,
        int messageThreadId, string message, int? reply = default)
    {
        while (true)
        {
            try
            {
                return await botClient.SendTextMessageAsync(chatId, message,
                    messageThreadId: (await botClient.GetChatAsync(chatId)).IsForum ?? false
                        ? messageThreadId
                        : default, parseMode: ParseMode.MarkdownV2, replyToMessageId: reply);
            }
            catch (ApiRequestException ex) when (ex.Message.Contains("Too Many Requests"))
            {
            }
            catch (ApiRequestException ex)
            {
                Main.Logger.Warn.WriteLine(Main.I18nHelper[CultureInfo.CurrentCulture.Name]
                    .Translate("bot.failed.messagesend", ex.Message, message));
                break;
            }
            catch (RequestException)
            {
            }
            catch (AggregateException ex)
            {
                if (ex.WriteAllException("bot.failed.messagesend", message))
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                Main.Logger.Warn.WriteLine(Main.I18nHelper[CultureInfo.CurrentCulture.Name]
                    .Translate("bot.failed.messagesend", ex.Message, message));
                Main.Logger.Debug.WriteLine(ex);
            }
        }

        return default;
    }

    internal static bool WriteAllException(this AggregateException ex, string message, params string[] args)
    {
        bool rt = false;
        foreach (Exception innerEx in ex.InnerExceptions)
        {
            switch (innerEx)
            {
                case ApiRequestException:
                    rt = true;
                    break;
                case RequestException:
                    continue;
                case AggregateException exception:
                    rt = exception.WriteAllException(message) || rt;
                    continue;
            }

            Main.Logger.Warn.WriteLine(Main.I18nHelper[CultureInfo.CurrentCulture.Name]
                .Translate(message, innerEx.Message, args));
        }

        return rt;
    }
}