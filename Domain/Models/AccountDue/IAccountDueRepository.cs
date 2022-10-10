namespace Domain.Models.AccountDue;

public interface IAccountDueRepository
{
    List<AccountDueListModel> GetAccountDueList(int hpId, long ptId, int sinDate, bool isUnpaidChecked, int pageIndex, int pageSize);
}
