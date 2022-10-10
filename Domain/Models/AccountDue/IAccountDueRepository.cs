namespace Domain.Models.AccountDue;

public interface IAccountDueRepository
{
    List<AccountDueListModel> GetAccountDueList(long ptId, int sinDate);
}
