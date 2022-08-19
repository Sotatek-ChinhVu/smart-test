namespace EmrCloudApi.Tenant.Responses.SetMst;

public class SaveSetMstResponse
{
    public SaveSetMstResponse(bool status)
    {
        Status = status;
    }
    public bool Status { get; private set; } = false;
}
