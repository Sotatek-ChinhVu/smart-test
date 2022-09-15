namespace EmrCloudApi.Tenant.Requests.SetMst;

public class SaveSuperSetDetailRequest
{
    public int SetCd { get; set; } = 0;

    public int UserId { get; set; } = 0;

    public List<SaveSetByomeiRequestItem> SaveSetByomeiRequestItems { get; set; } = new();

}
