using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListSyoukiInf;

public class SaveSyoukiInfListOutputData : IOutputData
{
    public SaveSyoukiInfListOutputData(SaveSyoukiInfListStatus status)
    {
        Status = status;
    }

    public SaveSyoukiInfListStatus Status { get; private set; }
}
