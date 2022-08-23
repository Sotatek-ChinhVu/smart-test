namespace EmrCloudApi.Tenant.Responses.SetMst;

public class ReorderSetMstResponse
{
    public ReorderSetMstResponse(bool status)
    {
        Status = status;
    }
    public bool Status { get; private set; } = false;
}
