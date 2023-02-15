using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListSyoukiInf;

public class SaveListSyoukiInfOutputData : IOutputData
{
    public SaveListSyoukiInfOutputData(SaveListSyoukiInfStatus status)
    {
        Status = status;
    }

    public SaveListSyoukiInfStatus Status { get; private set; }
}
