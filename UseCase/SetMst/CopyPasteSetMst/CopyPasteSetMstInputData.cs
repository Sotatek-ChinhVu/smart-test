using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.CopyPasteSetMst;

public class CopyPasteSetMstInputData : IInputData<CopyPasteSetMstOutputData>
{
    public CopyPasteSetMstInputData(int hpId, int userId, int copySetCd, int pasteSetCd, bool pasteToOtherGroup, int copyGenerationId, int copySetKbnEdaNo, int copySetKbn, int pasteGenerationId, int pasteSetKbnEdaNo, int pasteSetKbn)
    {
        HpId = hpId;
        UserId = userId;
        CopySetCd = copySetCd;
        PasteSetCd = pasteSetCd;
        PasteToOtherGroup = pasteToOtherGroup;
        CopyGenerationId = copyGenerationId;
        CopySetKbnEdaNo = copySetKbnEdaNo;
        CopySetKbn = copySetKbn;
        PasteGenerationId = pasteGenerationId;
        PasteSetKbnEdaNo = pasteSetKbnEdaNo;
        PasteSetKbn = pasteSetKbn;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int CopySetCd { get; private set; }

    public int PasteSetCd { get; private set; }

    public bool PasteToOtherGroup { get; private set; }

    public int CopyGenerationId { get; private set; }

    public int CopySetKbnEdaNo { get; private set; }

    public int CopySetKbn { get; private set; }

    public int PasteGenerationId { get; private set; }

    public int PasteSetKbnEdaNo { get; private set; }

    public int PasteSetKbn { get; private set; }
}
