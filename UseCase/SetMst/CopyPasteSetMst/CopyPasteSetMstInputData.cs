using UseCase.Core.Sync.Core;

namespace UseCase.SetMst.CopyPasteSetMst;

public class CopyPasteSetMstInputData : IInputData<CopyPasteSetMstOutputData>
{
    public CopyPasteSetMstInputData(int hpId, int userId, int copySetCd, int pasteSetCd)
    {
        HpId = hpId;
        UserId = userId;
        CopySetCd = copySetCd;
        PasteSetCd = pasteSetCd;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
    public int CopySetCd { get; private set; }
    public int PasteSetCd { get; private set; }
}
