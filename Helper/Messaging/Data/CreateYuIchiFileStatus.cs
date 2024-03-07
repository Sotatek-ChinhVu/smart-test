using System.Text.Json.Serialization;

namespace Helper.Messaging.Data;

public class CreateYuIchiFileStatus
{
    public CreateYuIchiFileStatus(bool doneProgress, string messageType, string message, string dataContent, string fileName)
    {
        DoneProgress = doneProgress;
        MessageType = messageType;
        Message = message;
        DataContent = dataContent;
        FileName = fileName;
    }

    [JsonPropertyName("doneProgress")]
    public bool DoneProgress { get; private set; }

    [JsonPropertyName("messageType")]
    public string MessageType { get; private set; }

    [JsonPropertyName("message")]
    public string Message { get; private set; }

    [JsonPropertyName("dataContent")]
    public string DataContent { get; private set; }

    [JsonPropertyName("fileName")]
    public string FileName { get; private set; }
}
