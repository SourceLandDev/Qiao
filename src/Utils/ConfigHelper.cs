using System.Text.Json;

namespace Qiao.Utils;
internal class ConfigHelper
{
    internal string Token { get; private set; }
    internal string ProxyUrl { get; private set; }
    internal ConfigHelper(string path)
    {
        string configStr = FileHelper.CheckFile(path, JsonSerializer.Serialize(this));
        ConfigHelper config = JsonSerializer.Deserialize<ConfigHelper>(configStr);
        Token = config.Token;
        ProxyUrl = config.ProxyUrl;
    }
}
