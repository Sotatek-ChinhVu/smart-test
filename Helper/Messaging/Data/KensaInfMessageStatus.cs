using System.Text.Json.Serialization;

namespace Helper.Messaging.Data;

public class KensaInfMessageStatus
{
    public KensaInfMessageStatus(bool done, int length, int successCount, string message)
    {
        Done = done;
        Length = length;
        SuccessCount = successCount;
        Message = message;
    }

    [JsonPropertyName("done")]
    public bool Done { get; private set; }

    [JsonPropertyName("length")]
    public int Length { get; private set; }

    [JsonPropertyName("successCount")]
    public int SuccessCount { get; private set; }

    [JsonPropertyName("message")]
    public string Message { get; private set; }
}