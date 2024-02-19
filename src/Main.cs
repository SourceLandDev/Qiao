using Hosihikari.PluginManagement;
using Microsoft.Extensions.Logging;
using SourceLand.Qiao;
using System.Reflection;

[assembly: EntryPoint<Main>]

namespace SourceLand.Qiao;

public sealed class Main : IEntryPoint
{
    internal static readonly Lazy<string?> ContainerPath;
    internal static readonly Lazy<ILogger> Logger;

    static Main()
    {
        ContainerPath = new(() =>
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            string location = currentAssembly.Location;
            return Path.GetDirectoryName(location);
        });
        Logger = new(() =>
        {
            using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
            return factory.CreateLogger(nameof(Qiao));
        });
    }

    public void Initialize(AssemblyPlugin _)
    {
        Bot.Initialize();
    }
}