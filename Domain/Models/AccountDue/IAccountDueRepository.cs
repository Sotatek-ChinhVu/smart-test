namespace Domain.Models.AccountDue;

public interface IAccountDueRepository
{
    List<AccountDueModel> GetAccountDueList(int hpId, long ptId, int sinDate, bool isUnpaidChecked, int pageIndex, int pageSize);
   
    bool SaveAccountDueList(int hpId, int ptId, int userId, int sinDate, List<AccountDueModel> listAccountDues);

    Dictionary<int, string> GetPaymentMethod(int hpId);

    Dictionary<int, string> GetUketsukeSbt(int hpId);
}
