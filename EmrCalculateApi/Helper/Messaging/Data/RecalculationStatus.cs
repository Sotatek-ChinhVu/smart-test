using System.Text.Json.Serialization;

namespace EmrCalculateApi.Helper.Messaging.Data;

public class RecalculationStatus
{
    public RecalculationStatus(bool done, int type, int length, int successCount, string message)
    {
        Done = done;
        Type = type;
        Length = length;
        SuccessCount = successCount;
        Message = message;
    }

    [JsonPropertyName("done")]
    public bool Done { get; private set; }

    [JsonPropertyName("type")]
    public int Type { get; private set; }

    [JsonPropertyName("length")]
    public int Length { get; private set; }

    [JsonPropertyName("successCount")]
    public int SuccessCount { get; private set; }

    [JsonPropertyName("message")]
    public string Message { get; private set; }
}
