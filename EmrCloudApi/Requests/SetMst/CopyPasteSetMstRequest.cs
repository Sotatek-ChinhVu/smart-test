namespace EmrCloudApi.Requests.SetMst;

public class CopyPasteSetMstRequest
{
    public long PtId { get; set; }

    public long RaiinNo { get; set; }

    public int SinDate { get; set; }

    public int GenerationId { get; set; }

    public int CopySetCd { get; set; }

    public int PasteSetCd { get; set; }

    public bool PasteToOtherGroup { get; set; }

    public int CopySetKbn { get; set; }

    public int CopySetKbnEdaNo { get; set; }

    public int PasteSetKbn { get; set; }

    public int PasteSetKbnEdaNo { get; set; }
}
