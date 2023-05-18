using System.Text.Json;
using System.Text.Json.Serialization;

namespace MessageSync.Utils;
internal class ConfigHelper
{
    [JsonPropertyName("chat_id")]
    internal long ChatId { get; private set; }
    [JsonPropertyName("message_thread_id")]
    internal int MessageThreadId { get; private set; }
    [JsonPropertyName("info_thread_id")]
    internal int InfoThreadId { get; private set; }
    [JsonPropertyName("sync_mode")]
    internal bool SyncMode { get; private set; }
    internal ConfigHelper(string path)
    {
        string configStr = FileHelper.CheckFile(path, JsonSerializer.Serialize(this));
        ConfigHelper config = JsonSerializer.Deserialize<ConfigHelper>(configStr);
        ChatId = config.ChatId;
        MessageThreadId = config.MessageThreadId;
        InfoThreadId = config.InfoThreadId;
        SyncMode = config.SyncMode;
    }
}
