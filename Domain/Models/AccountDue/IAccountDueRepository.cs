using Domain.Common;

namespace Domain.Models.AccountDue;

public interface IAccountDueRepository : IRepositoryBase
{
    List<AccountDueModel> GetAccountDueList(int hpId, long ptId, int sinDate, bool isUnpaidChecked);

    List<AccountDueModel> SaveAccountDueList(int hpId, long ptId, int userId, int sinDate, List<AccountDueModel> listAccountDues, string kaikeiTime);

    List<SyunoSeikyuModel> GetListSyunoSeikyuModel(int hpId, List<long> listRaiinNo);

    List<SyunoNyukinModel> GetListSyunoNyukinModel(int hpId,List<long> listRaiinNo);

    Dictionary<int, string> GetPaymentMethod(int hpId);

    Dictionary<int, string> GetUketsukeSbt(int hpId);

    bool IsNyukinExisted(int hpId, long ptId, long raiinNo, int sinDate);
}
