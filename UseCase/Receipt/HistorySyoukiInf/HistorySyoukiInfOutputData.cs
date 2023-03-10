using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.HistorySyoukiInf;

public class HistorySyoukiInfOutputData : IOutputData
{
    public HistorySyoukiInfOutputData(List<HistorySyoukiInfOutputItem> syoukiInfList, HistorySyoukiInfStatus status)
    {
        SyoukiInfList = syoukiInfList;
        Status = status;
    }

    public List<HistorySyoukiInfOutputItem> SyoukiInfList { get; private set; }

    public HistorySyoukiInfStatus Status { get; private set; }
}
