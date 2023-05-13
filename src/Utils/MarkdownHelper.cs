using System.Text;

namespace Qiao.Utils;
internal static class MarkdownHelper
{
    internal static string Escape(this string input) => new StringBuilder(input).Replace("_", "\\_")
        .Replace("*", "\\*").Replace("[", "\\[").Replace("]", "\\]").Replace("(", "\\(").Replace(")", "\\)")
        .Replace("~", "\\~").Replace("`", "\\`").Replace(">", "\\>").Replace("#", "\\#").Replace("+", "\\+")
        .Replace("-", "\\-").Replace("=", "\\=").Replace("|", "\\|").Replace("{", "\\{").Replace("}", "\\}")
        .Replace(".", "\\.").Replace("!", "\\!").ToString();
    internal static string Format(this string input)
    {
        StringBuilder stringBuilder = new(input);
        StringBuilder cacheEnding = new();
        for (int index = stringBuilder.ToString().IndexOf("§"); index > -1 && index + 1 != stringBuilder.Length; index = stringBuilder.ToString().IndexOf("§", index + 1))
        {
            switch (char.ToUpperInvariant(stringBuilder[index + 1]))
            {
                case 'L':
                    stringBuilder.Remove(index, 2);
                    stringBuilder.Insert(index, "*");
                    cacheEnding.Insert(0, "*");
                    break;
                case 'O':
                    stringBuilder.Remove(index, 2);
                    stringBuilder.Insert(index, "_");
                    cacheEnding.Insert(0, $"_{((cacheEnding.Length > 1 && cacheEnding[0] is '_' && cacheEnding[1] is '_') ? ' ' : string.Empty)}");
                    break;
                case 'N':
                    stringBuilder.Remove(index, 2);
                    stringBuilder.Insert(index, "__");
                    cacheEnding.Insert(0, "__");
                    break;
                case 'M':
                    stringBuilder.Remove(index, 2);
                    stringBuilder.Insert(index, "~");
                    cacheEnding.Insert(0, "~");
                    break;
                case 'K':
                    stringBuilder.Remove(index, 2);
                    stringBuilder.Insert(index, "||");
                    cacheEnding.Insert(0, "||");
                    break;
                case 'R':
                    stringBuilder.Remove(index, 2);
                    stringBuilder.Insert(index, cacheEnding);
                    cacheEnding.Clear();
                    break;
            }
        }
        if (cacheEnding.Length > 0)
        {
            stringBuilder.Append(cacheEnding);
        }
        return stringBuilder.ToString();
    }
}
