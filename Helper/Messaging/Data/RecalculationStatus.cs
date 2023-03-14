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

    public bool Done { get; private set; }

    public int Type { get; private set; }

    public int Length { get; private set; }

    public int SuccessCount { get; private set; }

    public string Message { get; private set; }
}
