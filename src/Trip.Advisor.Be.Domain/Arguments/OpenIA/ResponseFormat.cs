using System.Text.Json.Serialization;

namespace  Trip.Advisor.Be.Domain.Arguments.OpenIA
{
    public class ResponseFormat
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}