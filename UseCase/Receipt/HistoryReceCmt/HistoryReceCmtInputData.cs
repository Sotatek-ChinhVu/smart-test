using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.HistoryReceCmt;

public class HistoryReceCmtInputData : IInputData<HistoryReceCmtOutputData>
{
    public HistoryReceCmtInputData(int hpId, long ptId)
    {
        HpId = hpId;
        PtId = ptId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }
}
