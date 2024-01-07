using System.Text.Json;

namespace Qiao.Utils;

internal class I18nHelper
{
    private readonly Dictionary<string, Internationalization> _language;

    internal I18nHelper(string path)
    {
        _language = new();
        DirectoryInfo langFileDir = FileHelper.CheckDir(path);
        string defaultValue = JsonSerializer.Serialize(new Dictionary<string, string>());
        foreach (FileInfo file in langFileDir.GetFiles("*.json"))
        {
            this[Path.GetFileNameWithoutExtension(file.Name)] = new(
                JsonSerializer.Deserialize<Dictionary<string, string>>(
                    FileHelper.CheckFile(file.FullName, defaultValue)) ?? throw new NullReferenceException());
        }
    }

    internal Internationalization this[string languageCode]
    {
        get
        {
            if (TryGetLanguageData(languageCode, out Internationalization? languageData))
            {
                return languageData ?? throw new NullReferenceException();
            }

            Internationalization data = new(new(), languageCode);
            _language[languageCode] = data;
            return data;
        }
        set => AddLanguage(languageCode, value);
    }

    internal bool TryGetLanguageData(string languageCode, out Internationalization? languageData)
    {
        return _language.TryGetValue(languageCode, out languageData);
    }

    internal void AddLanguage(string languageCode, Internationalization languageData)
    {
        _language[languageCode] = languageData;
    }
}