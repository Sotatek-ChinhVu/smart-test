using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.ReceCmtHistory;

public class ReceCmtHistoryOutputData : IOutputData
{
    public ReceCmtHistoryOutputData(List<ReceCmtHistoryOutputItem> receCmtList, ReceCmtHistoryStatus status)
    {
        ReceCmtList = receCmtList;
        Status = status;
    }

    public List<ReceCmtHistoryOutputItem> ReceCmtList { get; private set; }

    public ReceCmtHistoryStatus Status { get; private set; }
}
