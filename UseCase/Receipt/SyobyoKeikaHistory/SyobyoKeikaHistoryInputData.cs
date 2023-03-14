using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SyobyoKeikaHistory;

public class SyobyoKeikaHistoryInputData : IInputData<SyobyoKeikaHistoryOutputData>
{
    public SyobyoKeikaHistoryInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }
}
