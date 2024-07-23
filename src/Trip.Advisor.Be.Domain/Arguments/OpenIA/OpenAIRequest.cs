using System.Text.Json.Serialization;

namespace  Trip.Advisor.Be.Domain.Arguments.OpenIA;

public class OpenAIRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("response_format")]
    public ResponseFormat ResponseFormat { get; set; }

    [JsonPropertyName("messages")]
    public List<OpenAIMessageRequest> Messages { get; set; } = new();

    [JsonPropertyName("temperature")]
    public float Temperature { get; set; }

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; } = 40;
}
