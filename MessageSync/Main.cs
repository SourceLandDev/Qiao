using LiteLoader.Logger;
using LiteLoader.NET;
using MessageSync.Functions;
using MessageSync.Utils;

namespace MessageSync;

[PluginMain(PluginName)]
public sealed class Main : IPluginInitializer
{
    internal const string PluginName = "MessageSync";
    public string Introduction => "消息同步";
    public Dictionary<string, string> MetaData => new();
    internal static Logger Logger = new(PluginName);
    internal static I18nHelper I18nHelper;
    internal static I18nHelper EmojiHelper;
    public static ConfigHelper Config;

    public void OnInitialize()
    {
        string path = Path.Combine("plugins", PluginName);
        FileHelper.CheckDir(path);

        Config = new ConfigHelper(Path.Combine(path, "config.json"));

        I18nHelper = new I18nHelper(Path.Combine(path, "languagePack"));

        EmojiHelper = new I18nHelper(Path.Combine(path, "emojiDescription"));

        EventSystem.SetupPlayer();

        EventSystem.SetupServer();

        ExportFunc.Setup();
    }
}