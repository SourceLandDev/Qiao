using System.Text.Json;
using System.Text.Json.Serialization;

namespace MessageSync.Utils;
internal class ConfigHelper
{
    [JsonPropertyName("chat_id")]
    public long ChatId { get; set; }
    [JsonPropertyName("message_thread_id")]
    public int MessageThreadId { get; set; }
    [JsonPropertyName("info_thread_id")]
    public int InfoThreadId { get; set; }
    [JsonPropertyName("sync_mode")]
    public bool SyncMode { get; set; }
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
