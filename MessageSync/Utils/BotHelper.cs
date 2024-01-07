using Qiao;
using System.Collections.Concurrent;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace MessageSync.Utils;

internal record Message(
    long ChatId,
    int? MessageThreadId,
    string Text,
    Action<Telegram.Bot.Types.Message?>? Callback = default,
    int? Reply = default);

public static class BotHelper
{
    private static readonly ConcurrentQueue<Message> ts_messages;

    private static readonly AutoResetEvent s_resetEvent;

    static BotHelper()
    {
        ts_messages = new();
        s_resetEvent = new(false);
        Thread thread = new(() =>
        {
            while (true)
            {
                if (Bot.Client is null)
                {
                    continue;
                }

                while (ts_messages.TryDequeue(out Message? message))
                {
                    Telegram.Bot.Types.Message? msg = Bot.Client.SendMessageAsync(message.ChatId,
                        message.MessageThreadId, message.Text, message.Reply).Result;
                    message.Callback?.Invoke(msg);
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

    public static void Enqueue(this ITelegramBotClient _, long chatId, int? messageThreadId, string message,
        Action<Telegram.Bot.Types.Message?>? callback = default, int? reply = default)
    {
        ts_messages.Enqueue(new(chatId, messageThreadId, message, callback, reply));
        s_resetEvent.Set();
    }

    public static async Task<Telegram.Bot.Types.Message?> SendMessageAsync(this ITelegramBotClient botClient,
        long chatId,
        int? messageThreadId, string message, int? reply = default)
    {
        while (true)
        {
            try
            {
                return await botClient.SendTextMessageAsync(chatId, message,
                    (await botClient.GetChatAsync(chatId)).IsForum ?? false
                        ? messageThreadId
                        : default, ParseMode.MarkdownV2, replyToMessageId: reply);
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