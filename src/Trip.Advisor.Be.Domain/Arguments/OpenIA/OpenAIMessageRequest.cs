using System.Text.Json.Serialization;

namespace  Trip.Advisor.Be.Domain.Arguments.OpenIA;

public class OpenAIMessageRequest
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}
