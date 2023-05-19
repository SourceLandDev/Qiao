using System.Text.Json;

namespace Qiao.Utils;
internal record ConfigHelper(string Token, string ProxyUrl)
{
    internal ConfigHelper(string path) : this(default, default)
    {
        string configStr = FileHelper.CheckFile(path, JsonSerializer.Serialize(this));
        ConfigHelper config = JsonSerializer.Deserialize<ConfigHelper>(configStr);
        Token = config.Token;
        ProxyUrl = config.ProxyUrl;
    }
}
