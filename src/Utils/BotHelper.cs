using Microsoft.Extensions.Logging;
using System.Globalization;
using Telegram.Bot.Exceptions;

namespace SourceLand.Qiao.Utils;

internal static class BotHelper
{
    public static bool WriteAllException(this AggregateException ex, string message)
    {
        bool rt = false;
        string text = L10nProvider.Shared.Value[CultureInfo.CurrentCulture.Name][message];
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

            Main.Logger.Value.LogError(text, innerEx.Message);
        }

        return rt;
    }
}