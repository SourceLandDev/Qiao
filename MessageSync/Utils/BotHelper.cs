using System.Collections.Concurrent;
using System.Globalization;
using Qiao;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace MessageSync.Utils;

internal record class Message(long ChatId, int MessageThreadId, string Text, int? Reply = default);

internal static class BotHelper
{
    private static readonly ConcurrentQueue<Message> ts_messages;

    private static readonly AutoResetEvent s_resetEvent;

    static BotHelper()
    {
        ts_messages = new();
        s_resetEvent = new(false);
        Thread thread = new(async () =>
        {
            while (true)
            {
                if (Bot.Client is null)
                {
                    continue;
                }
                while (ts_messages.TryDequeue(out Message message))
                {
                    await Bot.Client.SendMessageAsync(message.ChatId, message.MessageThreadId, message.Text, message.Reply);
                }
                s_resetEvent.WaitOne();
            }
        })
        {
            IsBackground = true,
            Priority = ThreadPriority.BelowNormal
        };
        thread.Start();
    }

    public static void Enqueue(this ITelegramBotClient _, long chatId,
        int messageThreadId, string message, int? reply = default)
    {
        ts_messages.Enqueue(new(chatId, messageThreadId, message, reply));
        s_resetEvent.Set();
    }

    public static async Task<Telegram.Bot.Types.Message> SendMessageAsync(this ITelegramBotClient botClient, long chatId,
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

    public static bool WriteAllException(this AggregateException ex, string message, params string[] args)
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