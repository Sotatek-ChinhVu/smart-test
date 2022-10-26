using Domain.Models.AccountDue;
using UseCase.Core.Sync.Core;

namespace UseCase.AccountDue.GetAccountDueList;

public class GetAccountDueListOutputData : IOutputData
{
    public GetAccountDueListOutputData(AccountDueModel accountDueModel, GetAccountDueListStatus status)
    {
        AccountDueModel = accountDueModel;
        Status = status;
    }

    public GetAccountDueListOutputData(GetAccountDueListStatus status)
    {
        AccountDueModel = new AccountDueModel(
                    new List<AccountDueItemModel>(),
                    new Dictionary<int, string>(),
                    new Dictionary<int, string>()
                );
        Status = status;
    }

    public AccountDueModel AccountDueModel { get; private set; }
    public GetAccountDueListStatus Status { get; private set; }
}
