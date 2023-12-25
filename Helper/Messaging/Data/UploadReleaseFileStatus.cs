using System.Text.Json.Serialization;

namespace Helper.Messaging.Data;

public class UploadReleaseFileStatus
{
    public UploadReleaseFileStatus(bool done, int length, int successCount, string fileName, string message)
    {
        Done = done;
        Length = length;
        SuccessCount = successCount;
        FileName = fileName;
        Message = message;
    }

    [JsonPropertyName("done")]
    public bool Done { get; private set; }

    [JsonPropertyName("length")]
    public int Length { get; private set; }

    [JsonPropertyName("successCount")]
    public int SuccessCount { get; private set; }

    [JsonPropertyName("fileName")]
    public string FileName { get; private set; }

    [JsonPropertyName("message")]
    public string Message { get; private set; }
}
