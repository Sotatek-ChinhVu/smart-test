using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SyobyoKeikaHistory;

public class SyobyoKeikaHistoryOutputData : IOutputData
{
    public SyobyoKeikaHistoryOutputData(List<SyobyoKeikaHistoryOutputItem> syoukiKeikaList, SyobyoKeikaHistoryStatus status)
    {
        SyobyoKeikaList = syoukiKeikaList;
        Status = status;
    }

    public List<SyobyoKeikaHistoryOutputItem> SyobyoKeikaList { get; private set; }

    public SyobyoKeikaHistoryStatus Status { get; private set; }
}

