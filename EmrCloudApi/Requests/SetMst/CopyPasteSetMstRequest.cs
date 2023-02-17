namespace EmrCloudApi.Requests.SetMst;

public class CopyPasteSetMstRequest
{
    public int CopySetCd { get; set; }

    public int PasteSetCd { get; set; }

    public bool PasteToOtherGroup { get; set; }

    public int CopyGenerationId { get; set; }

    public int CopySetKbn { get; set; }

    public int CopySetKbnEdaNo { get; set; }

    public int PasteGenerationId { get; set; }

    public int PasteSetKbn { get; set; }

    public int PasteSetKbnEdaNo { get; set; }
}
