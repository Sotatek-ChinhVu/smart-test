using UseCase.Core.Sync.Core;

namespace UseCase.AccountDue.IsNyukinExisted;

public class IsNyukinExistedInputData : IInputData<IsNyukinExistedOutputData>
{
    public IsNyukinExistedInputData(int hpId, long ptId, long raiinNo, int sinDate)
    {
        HpId = hpId;
        PtId = ptId;
        RaiinNo = raiinNo;
        SinDate = sinDate;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public int SinDate { get; private set; }
}
