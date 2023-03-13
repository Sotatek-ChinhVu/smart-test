using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SyoukiInfHistory;

public class SyoukiInfHistoryOutputData : IOutputData
{
    public SyoukiInfHistoryOutputData(List<SyoukiInfHistoryOutputItem> syoukiInfList, SyoukiInfHistoryStatus status)
    {
        SyoukiInfList = syoukiInfList;
        Status = status;
    }

    public List<SyoukiInfHistoryOutputItem> SyoukiInfList { get; private set; }

    public SyoukiInfHistoryStatus Status { get; private set; }
}
