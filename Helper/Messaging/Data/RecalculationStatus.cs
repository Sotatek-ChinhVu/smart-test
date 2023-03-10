namespace Helper.Messaging.Data;

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

    public bool Done { get; set; }

    public int Type { get; set; }

    public int Length { get; set; }

    public int SuccessCount { get; set; }

    public string Message { get; set; }
}
