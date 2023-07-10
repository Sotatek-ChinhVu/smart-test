using UseCase.Core.Sync.Core;

namespace UseCase.Lock.CheckLockOpenAccounting;

public class CheckLockOpenAccountingInputData : IInputData<CheckLockOpenAccountingOutputData>
{
    public CheckLockOpenAccountingInputData(int hpId, long ptId, long raiinNo)
    {
        HpId = hpId;
        PtId = ptId;
        RaiinNo = raiinNo;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }
}
