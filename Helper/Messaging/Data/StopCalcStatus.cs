namespace Helper.Messaging.Data;

public class StopCalcStatus
{
    public StopCalcStatus(bool status)
    {
        Status = status;
    }

    public bool Status { get; set; }
}
