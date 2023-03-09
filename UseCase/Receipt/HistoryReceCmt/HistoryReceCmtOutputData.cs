using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.HistoryReceCmt;

public class HistoryReceCmtOutputData : IOutputData
{
    public HistoryReceCmtOutputData(List<HistoryReceCmtOutputItem> receCmtList, HistoryReceCmtStatus status)
    {
        ReceCmtList = receCmtList;
        Status = status;
    }

    public List<HistoryReceCmtOutputItem> ReceCmtList { get; private set; }

    public HistoryReceCmtStatus Status { get; private set; }
}
