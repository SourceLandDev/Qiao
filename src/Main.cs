using LiteLoader.Logger;
using LiteLoader.NET;
using Qiao.Functions;
using Qiao.Types;
using Qiao.Utils;
using System.Net;
using System.Reflection;
using System.Text.Json;
using Telegram.Bot;

namespace Qiao;
[PluginMain(pluginName)]
public class Main : IPluginInitializer
{
    internal const string pluginName = "Qiao";
    public string Introduction => "消息互联";
    public Dictionary<string, string> MetaData => new();
    public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
    internal static Logger Logger = new(pluginName);
    internal static I18nHelper I18nHelper;
    internal static I18nHelper EmojiHelper;
    internal static TelegramBotClient BotClient;
    internal static Config Config = new()
    {
        ChatId = default,
        MessageThreadId = default,
        InfoThreadId = default,
        ProxyUrl = string.Empty,
        Token = string.Empty,
        SyncMode = true
    };
    public void OnInitialize()
    {
        string path = Path.Combine("plugins", pluginName);
        FileHelper.CheckDir(path);

        string configStr = FileHelper.CheckFile(Path.Combine(path, "config.json"), JsonSerializer.Serialize(Config));
        Config = JsonSerializer.Deserialize<Config>(configStr);

        DirectoryInfo langFileDir = FileHelper.CheckDir(Path.Combine(path, "languagePack"));
        I18nHelper = new();
        foreach (FileInfo file in langFileDir.GetFiles("*.json"))
        {
            I18nHelper[Path.GetFileNameWithoutExtension(file.Name)] = new(JsonSerializer.Deserialize<Dictionary<string, string>>(FileHelper.CheckFile(file.FullName, JsonSerializer.Serialize(new Dictionary<string, string>()))));
        }

        DirectoryInfo emojiFileDir = FileHelper.CheckDir(Path.Combine(path, "emojiDescription"));
        EmojiHelper = new();
        foreach (FileInfo file in emojiFileDir.GetFiles("*.json"))
        {
            EmojiHelper[Path.GetFileNameWithoutExtension(file.Name)] = new(JsonSerializer.Deserialize<Dictionary<string, string>>(FileHelper.CheckFile(file.FullName, JsonSerializer.Serialize(new Dictionary<string, string>()))));
        }

        BotClient = new(Config.Token, string.IsNullOrWhiteSpace(Config.ProxyUrl) ? default : new(new HttpClientHandler
        {
            Proxy = new WebProxy(Config.ProxyUrl, true)
        }));

        EventSystem.SetupPlayer();

        EventSystem.SetupServer();

        ExportFunc.Setup();
    }
}
