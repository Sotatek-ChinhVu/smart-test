namespace EmrCloudApi.Requests.SetMst;

public class SaveSuperSetDetailRequest
{
    public long PtId { get; set; }

    public long RaiinNo { get; set; }

    public int SinDate { get; set; }

    public int SetCd { get; set; } = 0;

    public List<SaveSetByomeiRequestItem> SaveSetByomeiRequestItems { get; set; } = new();

    public SaveSetKarteRequestItem SaveSetKarteRequestItem { get; set; } = new();

    public List<SaveSetOrderMstRequestItem> SaveSetOrderMstRequestItems { get; set; } = new();

    public FileItemRequestItem FileItem { get; set; } = new();
}
