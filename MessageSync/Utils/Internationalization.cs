namespace MessageSync.Utils;

internal class Internationalization
{
    internal readonly Dictionary<string, string> _languageData;
    private readonly string _name;

    internal Internationalization(Dictionary<string, string> langData, string name = "")
    {
        _languageData = langData;
        _name = name;
    }

    internal string this[string languageCode] => Translate(languageCode);

    /// <summary>
    ///     获取翻译
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="values">参数</param>
    /// <returns>翻译完成的信息</returns>
    internal string Translate(string key, params object[] values)
    {
        return !_languageData.TryGetValue(key, out string? value)
            ? key
            : string.Format(value, values);
    }
}