using System.Text.Json.Serialization;

namespace Helper.Messaging.Data;

public class CreateYuIchiFileStatus
{
    public CreateYuIchiFileStatus(bool doneProgress, string messageProgress)
    {
        DoneProgress = doneProgress;
        MessageProgress = messageProgress;
    }

    [JsonPropertyName("doneProgress")]
    public bool DoneProgress { get; private set; }

    [JsonPropertyName("messageProgress")]
    public string MessageProgress { get; private set; }
}
