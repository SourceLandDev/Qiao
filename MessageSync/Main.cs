using LiteLoader.Logger;
using LiteLoader.NET;
using MessageSync.Functions;
using MessageSync.Utils;

namespace MessageSync;

[PluginMain(PluginName)]
public sealed class Main : IPluginInitializer
{
    internal const string PluginName = "MessageSync";
    internal static Logger Logger;
    internal static I18nHelper I18nHelper;
    internal static I18nHelper EmojiHelper;
    public static ConfigHelper Config;
    public string Introduction => "消息同步";
    public Dictionary<string, string> MetaData => new();

    public void OnInitialize()
    {
        Logger = new(PluginName);

        string path = Path.Combine("plugins", PluginName);
        FileHelper.CheckDir(path);

        Config = new(Path.Combine(path, "config.json"));

        I18nHelper = new(Path.Combine(path, "languagePack"));

        EmojiHelper = new(Path.Combine(path, "emojiDescription"));

        EventSystem.SetupPlayer();

        EventSystem.SetupServer();

        ExportFunc.Setup();
    }
}