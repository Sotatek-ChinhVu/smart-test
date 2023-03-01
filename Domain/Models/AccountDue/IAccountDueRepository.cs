using Domain.Common;

namespace Domain.Models.AccountDue;

public interface IAccountDueRepository : IRepositoryBase
{
    List<AccountDueModel> GetAccountDueList(int hpId, long ptId, int sinDate, bool isUnpaidChecked);

    bool SaveAccountDueList(int hpId, long ptId, int userId, int sinDate, List<AccountDueModel> listAccountDues);

    List<SyunoSeikyuModel> GetListSyunoSeikyuModel(List<long> listRaiinNo);

    List<SyunoNyukinModel> GetListSyunoNyukinModel(List<long> listRaiinNo);

    Dictionary<int, string> GetPaymentMethod(int hpId);

    Dictionary<int, string> GetUketsukeSbt(int hpId);
}
