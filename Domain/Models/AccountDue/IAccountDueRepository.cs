namespace Domain.Models.AccountDue;

public interface IAccountDueRepository
{
    List<AccountDueItemModel> GetAccountDueList(int hpId, long ptId, int sinDate, bool isUnpaidChecked, int pageIndex, int pageSize);

    Dictionary<int, string> GetPaymentMethod(int hpId);

    Dictionary<int, string> GetUketsukeSbt(int hpId);
}
