using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.CopyPasteSetMst;

public class CopyPasteSetMstInputData : IInputData<CopyPasteSetMstOutputData>
{
    public CopyPasteSetMstInputData(int hpId, int userId, long ptId, long raiinNo, int sinDate, int generationId, int copySetCd, int pasteSetCd, bool pasteToOtherGroup, int copySetKbnEdaNo, int copySetKbn, int pasteSetKbnEdaNo, int pasteSetKbn)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        RaiinNo = raiinNo;
        SinDate = sinDate;
        GenerationId = generationId;
        CopySetCd = copySetCd;
        PasteSetCd = pasteSetCd;
        PasteToOtherGroup = pasteToOtherGroup;
        CopySetKbnEdaNo = copySetKbnEdaNo;
        CopySetKbn = copySetKbn;
        PasteSetKbnEdaNo = pasteSetKbnEdaNo;
        PasteSetKbn = pasteSetKbn;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; set; }

    public long RaiinNo { get; set; }

    public int SinDate { get; set; }

    public int GenerationId { get; private set; }

    public int CopySetCd { get; private set; }

    public int PasteSetCd { get; private set; }

    public bool PasteToOtherGroup { get; private set; }

    public int CopySetKbnEdaNo { get; private set; }

    public int CopySetKbn { get; private set; }

    public int PasteSetKbnEdaNo { get; private set; }

    public int PasteSetKbn { get; private set; }
}
