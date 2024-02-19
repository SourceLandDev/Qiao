using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SourceLand.Qiao.Utils;

public record Config(string Token, string ProxyUrl)
{
    internal static readonly Lazy<Config?> Shared = new(() =>
    {
        string path = Path.Combine(Main.ContainerPath.Value!, "config.json");
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddJsonFile(path, true, true);
        return builder.Configuration.Get<Config>();
    });
}