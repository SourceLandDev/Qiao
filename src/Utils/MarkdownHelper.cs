namespace Qiao.Utils;
internal static class MarkdownHelper
{
    internal static string Escape(this string input) => input.ToString().Replace("_", "\\_").Replace("*", "\\*").Replace("[", "\\[").Replace("]", "\\]")
            .Replace("(", "\\(").Replace(")", "\\)").Replace("~", "\\~").Replace("`", "\\`")
            .Replace(">", "\\>").Replace("#", "\\#").Replace("+", "\\+").Replace("-", "\\-")
            .Replace("=", "\\=").Replace("|", "\\|").Replace("{", "\\{").Replace("}", "\\}")
            .Replace(".", "\\.").Replace("!", "\\!");
    internal static string Format(this string input)
    {
        string cacheend = string.Empty;
        for (int index = input.IndexOf("§"); index > -1 && index + 1 != input.Length; index = input.IndexOf("§", index + 1))
        {
            switch (input.ToUpperInvariant()[index + 1])
            {
                case 'L':
                    input = input.Remove(index, 2);
                    input = input.Insert(index, "*");
                    cacheend = $"*{cacheend}";
                    break;
                case 'O':
                    input = input.Remove(index, 2);
                    input = input.Insert(index, "_");
                    cacheend = $"_{(cacheend.StartsWith("__") ? "\0" : string.Empty)}{cacheend}";
                    break;
                case 'N':
                    input = input.Remove(index, 2);
                    input = input.Insert(index, "__");
                    cacheend = $"__{cacheend}";
                    break;
                case 'M':
                    input = input.Remove(index, 2);
                    input = input.Insert(index, "~");
                    cacheend = $"~{cacheend}";
                    break;
                case 'K':
                    input = input.Remove(index, 2);
                    input = input.Insert(index, "||");
                    cacheend = $"||{cacheend}";
                    break;
                case 'R':
                    input = input.Remove(index, 2);
                    input = input.Insert(index, cacheend);
                    cacheend = string.Empty;
                    break;
            }
        }
        if (!string.IsNullOrEmpty(cacheend))
        {
            input += cacheend;
        }
        return input;
    }
}
