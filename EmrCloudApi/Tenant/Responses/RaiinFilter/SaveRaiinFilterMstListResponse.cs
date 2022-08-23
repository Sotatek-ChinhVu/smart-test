namespace EmrCloudApi.Tenant.Responses.RaiinFilter;

public class SaveRaiinFilterMstListResponse
{
    public SaveRaiinFilterMstListResponse(bool success)
    {
        Success = success;
    }

    public bool Success { get; private set; }
}
