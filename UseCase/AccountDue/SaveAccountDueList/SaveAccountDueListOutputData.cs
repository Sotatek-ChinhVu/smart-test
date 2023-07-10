using Domain.Models.AccountDue;
using UseCase.Core.Sync.Core;

namespace UseCase.AccountDue.SaveAccountDueList;

public class SaveAccountDueListOutputData : IOutputData
{
    public SaveAccountDueListOutputData(SaveAccountDueListStatus status)
    {
        Status = status;
        AccountDueModel = new AccountDueListModel(new(), new(), new());
    }

    public SaveAccountDueListOutputData(SaveAccountDueListStatus status, List<AccountDueModel> accountDueList)
    {
        Status = status;
        AccountDueModel = new AccountDueListModel(accountDueList, new(), new());
    }

    public SaveAccountDueListStatus Status { get; private set; }

    public AccountDueListModel AccountDueModel { get; private set; }
}
