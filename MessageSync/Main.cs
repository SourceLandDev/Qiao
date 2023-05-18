using LiteLoader.Logger;
using LiteLoader.NET;
using MessageSync.Functions;
using MessageSync.Utils;
using System.Reflection;

namespace MessageSync;
[PluginMain(pluginName)]
public class Main : IPluginInitializer
{
    internal const string pluginName = "MessageSync";
    public string Introduction => "消息同步";
    public Dictionary<string, string> MetaData => new();
    public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    internal static Logger Logger = new(pluginName);
    internal static I18nHelper I18nHelper;
    internal static I18nHelper EmojiHelper;
    internal static ConfigHelper Config;
    public void OnInitialize()
    {
        string path = Path.Combine("plugins", pluginName);
        FileHelper.CheckDir(path);

        Config = new ConfigHelper(Path.Combine(path, "config.json"));

        I18nHelper = new I18nHelper(Path.Combine(path, "languagePack"));

        EmojiHelper = new I18nHelper(Path.Combine(path, "emojiDescription"));

        EventSystem.SetupPlayer();

        EventSystem.SetupServer();

        ExportFunc.Setup();
    }
}
