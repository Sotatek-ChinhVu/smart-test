namespace EmrCloudApi.Tenant.Requests.SetMst;

public class SaveSuperSetDetailRequest
{
    public int SetCd { get; set; } = 0;

    public List<SaveSetByomeiRequestItem> SaveSetByomeiRequestItems { get; set; } = new();

    public SaveSetKarteRequestItem SaveSetKarteRequestItem { get; set; } = new();

    public List<SaveSetOrderMstRequestItem> SaveSetOrderMstRequestItems { get; set; } = new();

}
