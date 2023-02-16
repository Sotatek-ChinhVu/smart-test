using UseCase.Core.Sync.Core;

namespace UseCase.Family.SaveFamilyList;

public class SaveFamilyListOutputData : IOutputData
{
    public SaveFamilyListOutputData(SaveFamilyListStatus status)
    {
        Status = status;
    }

    public SaveFamilyListStatus Status { get; private set; }
}
