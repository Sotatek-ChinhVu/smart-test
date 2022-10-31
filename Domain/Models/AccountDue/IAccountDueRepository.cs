namespace Domain.Models.AccountDue;

public interface IAccountDueRepository
{
    List<AccountDueModel> GetAccountDueList(int hpId, long ptId, int sinDate, bool isUnpaidChecked, int pageIndex, int pageSize);

    bool SaveAccountDueList(int hpId, long ptId, int userId, int sinDate, List<AccountDueModel> listAccountDues);

    List<SyunoNyukinViewModel> GetListSyunoNyukinViewModel(List<long> listRaiinNo);

    Dictionary<int, string> GetPaymentMethod(int hpId);

    Dictionary<int, string> GetUketsukeSbt(int hpId);
}
