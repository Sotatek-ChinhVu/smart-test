using UseCase.Core.Sync.Core;

namespace UseCase.Lock.CheckLockOpenAccounting;

public class CheckLockOpenAccountingInputData : IInputData<CheckLockOpenAccountingOutputData>
{
    public CheckLockOpenAccountingInputData(int hpId, long ptId, long raiinNo, int userId)
    {
        HpId = hpId;
        PtId = ptId;
        RaiinNo = raiinNo;
        UserId = userId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public int UserId { get; private set; }
}
