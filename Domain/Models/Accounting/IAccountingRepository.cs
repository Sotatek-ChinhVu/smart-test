namespace Domain.Models.Accounting
{
    public interface IAccountingRepository
    {
        public List<AccountingModel> GetListSyunoSeikyu(int hpId, long ptId, int sinDate, List<long> listRaiinNo, bool getAll = false);
    }
}
