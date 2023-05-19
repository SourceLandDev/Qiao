using System.Text.Json;

namespace MessageSync.Utils;

internal class I18nHelper
{
    internal I18nHelper(string path)
    {
        DirectoryInfo langFileDir = FileHelper.CheckDir(path);
        string defaultValue = JsonSerializer.Serialize(new Dictionary<string, string>());
        foreach (FileInfo file in langFileDir.GetFiles("*.json"))
        {
            this[Path.GetFileNameWithoutExtension(file.Name)] = new(JsonSerializer.Deserialize<Dictionary<string, string>>(FileHelper.CheckFile(file.FullName, defaultValue)));
        }
    }
    private readonly Dictionary<string, Internationalization> _language = new();
    internal bool TryGetLanguageData(string languageCode, out Internationalization languageData) => _language.TryGetValue(languageCode, out languageData);
    internal void AddLanguage(string languageCode, Internationalization languageData) => _language[languageCode] = languageData;
    internal Internationalization this[string languageCode]
    {
        get => TryGetLanguageData(languageCode, out Internationalization languageData)
                ? languageData
                : new Internationalization(new(), languageCode);
        set => AddLanguage(languageCode, value);
    }
}
