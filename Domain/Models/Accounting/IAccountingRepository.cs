using Domain.Models.AccountDue;

namespace Domain.Models.Accounting
{
    public interface IAccountingRepository
    {
        SyunoSeikyuModel GetListSyunoSeikyu(int hpId, long ptId, int sinDate, List<long> listRaiinNo, bool getAll = false);
        AccountingModel GetAccountingInfo(int hpId, long ptId, long oyaRaiinNo);
    }
}
