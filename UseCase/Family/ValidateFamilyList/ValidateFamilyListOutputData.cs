using UseCase.Core.Sync.Core;

namespace UseCase.Family.ValidateFamilyList;

public class ValidateFamilyListOutputData : IOutputData
{
    public ValidateFamilyListOutputData(ValidateFamilyListStatus status)
    {
        Status = status;
    }

    public ValidateFamilyListStatus Status { get; private set; }
}
