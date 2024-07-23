using System.Text.Json.Serialization;

namespace  Trip.Advisor.Be.Domain.Arguments.Trip;

public class TripResponse
{
    [JsonPropertyName("destiny")]
    public string Destiny { get; set; }

    [JsonPropertyName("period")]
    public string Period { get; set; }

    [JsonPropertyName("amount")]
    public Amount Amount { get; set; }

    [JsonPropertyName("days")]
    public List<DayPosition> Days { get; set; }
}
public class Activity
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("detail")]
    public string Detail { get; set; }
}

public class Amount
{
    [JsonPropertyName("accommodation")]
    public int Accommodation { get; set; }

    [JsonPropertyName("restaurants")]
    public int Restaurants { get; set; }
}

public class DayPosition
{
    [JsonPropertyName("day")]
    public int Day{ get; set; }

    [JsonPropertyName("activities")]
    public List<Activity> Activities { get; set; }
}
