namespace EmrCloudApi.Tenant.Responses.KarteFilter;

public class SaveKarteFilterMstResponse
{
    public SaveKarteFilterMstResponse(bool success)
    {
        Success = success;
    }
    public bool Success { get; private set; }
}
