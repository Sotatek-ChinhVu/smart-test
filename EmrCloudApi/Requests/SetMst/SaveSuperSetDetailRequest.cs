namespace EmrCloudApi.Requests.SetMst;

public class SaveSuperSetDetailRequest
{
    public int SetCd { get; set; } = 0;

    public int UserId { get; set; } = 0;

    public int HpId { get; set; } = 1;

    public List<SaveSetByomeiRequestItem> SaveSetByomeiRequestItems { get; set; } = new();

    public SaveSetKarteRequestItem SaveSetKarteRequestItem { get; set; } = new();

    public List<SaveSetOrderMstRequestItem> SaveSetOrderMstRequestItems { get; set; } = new();

    public FileItemRequestItem FileItem { get; set; } = new();
}
