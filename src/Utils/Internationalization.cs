namespace Qiao.Utils;

internal class Internationalization
{
    internal readonly Dictionary<string, string> _languageData;
    private readonly string _name;
    internal Internationalization(Dictionary<string, string> langData, string name = "")
    {
        _languageData = langData;
        _name = name;
    }
    /// <summary>
    /// 获取翻译
    /// </summary>
    /// <param name="key">键</param>
    /// <param name="values">参数</param>
    /// <returns>翻译完成的信息</returns>
    internal string Translate(string key, params object[] values)
    {
        if (!_languageData.TryGetValue(key, out string value))
        {
            throw new KeyNotFoundException($"{key} not find{(string.IsNullOrWhiteSpace(_name) ? string.Empty : $" in ${_name}")}, please check your language file");
        }
        if (values is not null)
        {
            try
            {
                return string.Format(value, values);
            }
            catch (FormatException ex)
            {
                Main.Logger.Error.WriteLine($"Failed to translate '{key}': {ex.Message}");
            }
        }
        return value;
    }
    internal string this[string languageCode] => Translate(languageCode);
}
