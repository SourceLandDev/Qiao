using System.Text.Json;
using System.Text.Json.Serialization;

namespace MessageSync.Utils;

public record ConfigHelper(
    [property: JsonPropertyName("chat_id")]
    long ChatId,
    [property: JsonPropertyName("message_thread_id")]
    int MessageThreadId,
    [property: JsonPropertyName("info_thread_id")]
    int InfoThreadId,
    [property: JsonPropertyName("sync_mode")]
    bool SyncMode)
{
    internal ConfigHelper(string path) : this(default, default, default, default)
    {
        string configStr = FileHelper.CheckFile(path, JsonSerializer.Serialize(this));
        ConfigHelper config = JsonSerializer.Deserialize<ConfigHelper>(configStr) ?? throw new NullReferenceException();
        ChatId = config.ChatId;
        MessageThreadId = config.MessageThreadId;
        InfoThreadId = config.InfoThreadId;
        SyncMode = config.SyncMode;
    }
}