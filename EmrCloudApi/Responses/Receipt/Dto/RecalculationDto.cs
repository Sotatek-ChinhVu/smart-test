using Helper.Messaging.Data;
using System.Text.Json.Serialization;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class RecalculationDto
{
    public RecalculationDto(RecalculationStatus status)
    {
        Done = status.Done;
        Type = status.Type;
        Length = status.Length;
        SuccessCount = status.SuccessCount;
        Message = status.Message;
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