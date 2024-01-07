using System.Text.Json;

namespace Qiao.Utils;

internal record ConfigHelper(string Token, string ProxyUrl)
{
    internal ConfigHelper(string path) : this("", "")
    {
        string configStr = FileHelper.CheckFile(path, JsonSerializer.Serialize(this));
        ConfigHelper config = JsonSerializer.Deserialize<ConfigHelper>(configStr) ?? throw new NullReferenceException();
        Token = config.Token;
        ProxyUrl = config.ProxyUrl;
    }
}