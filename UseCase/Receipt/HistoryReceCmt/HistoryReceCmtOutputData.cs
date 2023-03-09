using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.HistoryReceCmt;

public class HistoryReceCmtOutputData : IOutputData
{
    public HistoryReceCmtOutputData(List<ReceCmtItem> receCmtList, HistoryReceCmtStatus status)
    {
        ReceCmtList = receCmtList;
        Status = status;
    }

    public List<ReceCmtItem> ReceCmtList { get; private set; }

    public HistoryReceCmtStatus Status { get; private set; }
}
