using LiteLoader.Event;
using LiteLoader.Logger;
using LiteLoader.NET;
using Qiao.Utils;
using System.Reflection;

namespace Qiao;

[PluginMain(pluginName)]
public class Plugin : IPluginInitializer
{
    internal const string pluginName = "Qiao";
    public string Introduction => "IM机器人API";
    public Dictionary<string, string> MetaData => new();
    public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    internal static Logger Logger = new(pluginName);
    internal static I18nHelper I18nHelper;
    internal static ConfigHelper Config;

    public void OnInitialize()
    {
        string path = Path.Combine("plugins", pluginName);
        FileHelper.CheckDir(path);

        Config = new ConfigHelper(Path.Combine(path, "config.json"));

        I18nHelper = new I18nHelper(Path.Combine(path, "languagePack"));

        ServerStartedEvent.Event += (_) =>
        {
            Bot.Initialize();
            return default;
        };
    }
}