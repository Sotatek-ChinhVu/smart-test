using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.CopyPasteSetMst;

public class CopyPasteSetMstInputData : IInputData<CopyPasteSetMstOutputData>
{
    public CopyPasteSetMstInputData(int hpId, int userId, int copySetCd, int pasteSetCd, bool pasteToOtherSetKbn, int pasteSetKbnEdaNo, int pasteSetKbn)
    {
        HpId = hpId;
        UserId = userId;
        CopySetCd = copySetCd;
        PasteSetCd = pasteSetCd;
        PasteToOtherSetKbn = pasteToOtherSetKbn;
        PasteSetKbnEdaNo = pasteSetKbnEdaNo;
        PasteSetKbn = pasteSetKbn;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int CopySetCd { get; private set; }

    public int PasteSetCd { get; private set; }

    public bool PasteToOtherSetKbn { get; private set; }

    public int PasteSetKbnEdaNo { get; private set; }

    public int PasteSetKbn { get; private set; }
}
