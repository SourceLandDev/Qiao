using System.Text.Json;

namespace Qiao.Utils;
internal class ConfigHelper
{
    public string Token { get; set; }
    public string ProxyUrl { get; set; }
    internal ConfigHelper(string path)
    {
        string configStr = FileHelper.CheckFile(path, JsonSerializer.Serialize(this));
        ConfigHelper config = JsonSerializer.Deserialize<ConfigHelper>(configStr);
        Token = config.Token;
        ProxyUrl = config.ProxyUrl;
    }
}
