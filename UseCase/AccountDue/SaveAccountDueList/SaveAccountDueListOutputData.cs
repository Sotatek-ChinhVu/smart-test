using UseCase.Core.Sync.Core;

namespace UseCase.AccountDue.SaveAccountDueList;

public class SaveAccountDueListOutputData : IOutputData
{
    public SaveAccountDueListOutputData(SaveAccountDueListStatus status)
    {
        Status = status;
    }

    public SaveAccountDueListStatus Status { get; private set; }

}
