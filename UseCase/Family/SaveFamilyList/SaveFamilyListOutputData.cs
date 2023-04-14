using UseCase.Core.Sync.Core;

namespace UseCase.Family.SaveFamilyList;

public class SaveFamilyListOutputData : IOutputData
{
    public SaveFamilyListOutputData(ValidateFamilyListStatus status)
    {
        Status = status;
    }

    public ValidateFamilyListStatus Status { get; private set; }
}
