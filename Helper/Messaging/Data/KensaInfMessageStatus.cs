using System.Text.Json.Serialization;

namespace Helper.Messaging.Data;

public class KensaInfMessageStatus
{
    public KensaInfMessageStatus(bool doneProgress, int successCount, bool successed, string kensaItem, string errorMessage)
    {
        DoneProgress = doneProgress;
        Successed = successed;
        KensaItem = kensaItem;
        ErrorMessage = errorMessage;
        SuccessCount = successCount;
    }

    public KensaInfMessageStatus(bool doneProgress, bool successed, string kensaItem, string errorMessage)
    {
        DoneProgress = doneProgress;
        Successed = successed;
        KensaItem = kensaItem;
        ErrorMessage = errorMessage;
    }

    [JsonPropertyName("doneProgress")]
    public bool DoneProgress { get; private set; }

    [JsonPropertyName("successed")]
    public bool Successed { get; private set; }

    [JsonPropertyName("successCount")]
    public int SuccessCount { get; private set; }

    [JsonPropertyName("kensaItem")]
    public string KensaItem { get; private set; }

    [JsonPropertyName("errorMessage")]
    public string ErrorMessage { get; private set; }
}