using System.Globalization;
using Telegram.Bot.Exceptions;

namespace Qiao.Utils;
internal static class BotHelper
{
    internal static bool WriteAllException(this AggregateException ex)
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
                    rt = exception.WriteAllException() || rt;
                    continue;
            }
            Plugin.Logger.Warn.WriteLine(Plugin.I18nHelper[CultureInfo.CurrentCulture.Name].Translate("bot.failed.messagesend", innerEx.Message));
        }
        return rt;
    }
}
