using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.ReceCmtHistory;

public class ReceCmtHistoryInputData : IInputData<ReceCmtHistoryOutputData>
{
    public ReceCmtHistoryInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }
}
