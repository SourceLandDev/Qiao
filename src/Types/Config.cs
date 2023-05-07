using System.Text.Json.Serialization;

namespace Qiao.Types;
internal record Config
{
    [JsonPropertyName("token")]
    public required string Token { get; set; }
    [JsonPropertyName("chatId")]
    public required long ChatId { get; set; }
    [JsonPropertyName("messageThreadId")]
    public required int MessageThreadId { get; set; }
    [JsonPropertyName("infoThreadId")]
    public required int InfoThreadId { get; set; }
    [JsonPropertyName("proxy")]
    public required string ProxyUrl { get; set; }
    [JsonPropertyName("asyncmode")]
    public required bool AsyncMode { get; set; }
}
