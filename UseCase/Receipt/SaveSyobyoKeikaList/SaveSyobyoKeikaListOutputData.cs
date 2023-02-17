using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListSyobyoKeika;

public class SaveSyobyoKeikaListOutputData : IOutputData
{
    public SaveSyobyoKeikaListOutputData(SaveSyobyoKeikaListStatus status)
    {
        Status = status;
    }

    public SaveSyobyoKeikaListStatus Status { get;private set; }
}
