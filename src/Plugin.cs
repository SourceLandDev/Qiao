using LiteLoader.Event;
using LiteLoader.Logger;
using LiteLoader.NET;
using Qiao.Utils;

namespace Qiao;

[PluginMain(PluginName)]
public sealed class Plugin : IPluginInitializer
{
    private const string PluginName = "Qiao";
    public string Introduction => "IM机器人API";
    public Dictionary<string, string> MetaData => new();
    internal static Logger Logger = new(PluginName);
    internal static I18nHelper I18nHelper;
    internal static ConfigHelper Config;

    public void OnInitialize()
    {
        string path = Path.Combine("plugins", PluginName);
        FileHelper.CheckDir(path);

        Config = new ConfigHelper(Path.Combine(path, "config.json"));

        I18nHelper = new I18nHelper(Path.Combine(path, "languagePack"));

        Bot.Initialize();
    }
}