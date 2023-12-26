using System.Text.Json.Serialization;

namespace Helper.Messaging.Data;

public class UploadDrugImageAndReleaseStatus
{
    public UploadDrugImageAndReleaseStatus(bool done, int length, int successCount, string folderName, string fileName, string message)
    {
        Done = done;
        Length = length;
        SuccessCount = successCount;
        FolderName = folderName;
        FileName = fileName;
        Message = message;
    }

    [JsonPropertyName("done")]
    public bool Done { get; private set; }

    [JsonPropertyName("length")]
    public int Length { get; private set; }

    [JsonPropertyName("successCount")]
    public int SuccessCount { get; private set; }

    [JsonPropertyName("folderName")]
    public string FolderName { get; private set; }

    [JsonPropertyName("fileName")]
    public string FileName { get; private set; }

    [JsonPropertyName("message")]
    public string Message { get; private set; }
}
