using System.Text.Json.Serialization;

namespace TamagotchiApi.Model;

internal class GptMessage
{
    [JsonPropertyName("role")]
    public string Role { get; init; } = "user";

    [JsonPropertyName("content")]
    public string? Content { get; init; }
}
