using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListSyoukiInf;

public class SaveSyoukiInfListOutputData : IOutputData
{
    public SaveSyoukiInfListOutputData(SaveSyoukiInfListStatus status, List<SyoukiInfItem> syoukiInfInvalidList)
    {
        Status = status;
        SyoukiInfInvalidList = syoukiInfInvalidList;
    }

    public SaveSyoukiInfListStatus Status { get; private set; }

    public List<SyoukiInfItem> SyoukiInfInvalidList { get; private set; }
}
