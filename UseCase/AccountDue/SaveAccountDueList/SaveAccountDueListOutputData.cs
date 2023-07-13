using Domain.Models.AccountDue;
using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.AccountDue.SaveAccountDueList;

public class SaveAccountDueListOutputData : IOutputData
{
    public SaveAccountDueListOutputData(SaveAccountDueListStatus status)
    {
        Status = status;
        AccountDueModel = new AccountDueListModel(new(), new(), new());
        ReceptionInfos = new();
        SameVisitList = new();
    }

    public SaveAccountDueListOutputData(SaveAccountDueListStatus status, List<AccountDueModel> accountDueList, List<ReceptionRowModel> receptionInfos, List<SameVisitModel> sameVisitList)
    {
        Status = status;
        AccountDueModel = new AccountDueListModel(accountDueList, new(), new());
        ReceptionInfos = receptionInfos;
        SameVisitList = sameVisitList;
    }

    public SaveAccountDueListStatus Status { get; private set; }

    public AccountDueListModel AccountDueModel { get; private set; }

    public List<ReceptionRowModel> ReceptionInfos { get; private set; }

    public List<SameVisitModel> SameVisitList { get; private set; }
}
