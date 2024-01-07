using LiteLoader.Logger;
using LiteLoader.NET;
using Qiao.Utils;

namespace Qiao;

[PluginMain(PluginName)]
public sealed class Plugin : IPluginInitializer
{
    private const string PluginName = "Qiao";
    internal static Logger Logger;
    internal static I18nHelper I18nHelper;
    internal static ConfigHelper Config;
    public string Introduction => "IM机器人API";
    public Dictionary<string, string> MetaData => new();

    public void OnInitialize()
    {
        Logger = new(PluginName);

        string path = Path.Combine("plugins", PluginName);
        FileHelper.CheckDir(path);

        Config = new(Path.Combine(path, "config.json"));

        I18nHelper = new(Path.Combine(path, "languagePack"));

        Bot.Initialize();
    }
}