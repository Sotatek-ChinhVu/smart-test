using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListSyobyoKeika;

public class SaveListSyobyoKeikaOutputData : IOutputData
{
    public SaveListSyobyoKeikaOutputData(SaveListSyobyoKeikaStatus status)
    {
        Status = status;
    }

    public SaveListSyobyoKeikaStatus Status { get;private set; }
}
