namespace Helper.Messaging.Data;

public class RecalculationStatus
{
    public RecalculationStatus(bool done, int type, int length, int successCount, string message)
    {
        this.done = done;
        this.type = type;
        this.length = length;
        this.successCount = successCount;
        this.message = message;
    }

    public bool done { get; private set; }

    public int type { get; private set; }

    public int length { get; private set; }

    public int successCount { get; private set; }

    public string message { get; private set; }
}
