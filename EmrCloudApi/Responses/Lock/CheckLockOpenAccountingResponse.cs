namespace EmrCloudApi.Responses.Lock;

public class CheckLockOpenAccountingResponse
{
    public CheckLockOpenAccountingResponse(bool status)
    {
        Status = status;
    }

    public bool Status { get; private set; }
}
