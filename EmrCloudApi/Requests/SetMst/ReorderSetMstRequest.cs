namespace EmrCloudApi.Requests.SetMst;

public class ReorderSetMstRequest
{
    public long PtId { get; set; }

    public long RaiinNo { get; set; }

    public int SinDate { get; set; }

    public int DragSetCd { get; set; }

    public int DropSetCd { get; set; }
}