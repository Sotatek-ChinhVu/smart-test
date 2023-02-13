using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingResponse
    {
        public GetAccountingResponse(List<AccountingModel> accountingModels, int totalPoint, int kanFutan, int totalSelfExpense, int tax, int adjustFutan, int debitBalance, int sumAdjust, int sumAdjustView, int thisCredit, int thisWari)
        {
            AccountingModels = accountingModels;
            TotalPoint = totalPoint;
            KanFutan = kanFutan;
            TotalSelfExpense = totalSelfExpense;
            Tax = tax;
            AdjustFutan = adjustFutan;
            DebitBalance = debitBalance;
            SumAdjust = sumAdjust;
            SumAdjustView = sumAdjustView;
            ThisCredit = thisCredit;
            ThisWari = thisWari;
        }

        public List<AccountingModel> AccountingModels { get; private set; }
        public int TotalPoint { get; private set; }
        public int KanFutan { get; private set; }
        public int TotalSelfExpense { get; private set; }
        public int Tax { get; private set; }
        public int AdjustFutan { get; private set; }
        public int DebitBalance { get; private set; }
        public int SumAdjust { get; private set; }
        public int SumAdjustView { get; private set; }
        public int ThisCredit { get; private set; }
        public int ThisWari { get; private set; }
    }
}
