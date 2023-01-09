namespace Domain.Models.Accounting
{
    public interface IAccountingRepository
    {
        public List<AccountingModel> GetListSyunoSeikyu(int hpId, long ptId, int sinDate, long raiinNo, bool getAll = false);

        public AccountingInfModel GetAccountingInfAllRaiinNo(List<AccountingModel> accountingModels);
    }
}
