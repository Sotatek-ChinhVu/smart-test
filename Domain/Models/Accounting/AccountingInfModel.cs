namespace Domain.Models.Accounting
{
    public class AccountingInfModel
    {
        public AccountingInfModel(int totalPoint, int kanFutan, int totalSelfExpense, int tax, int debitBalance, int sumAdjust, int sumAdjustView, int thisCredit, int thisWari, int payType, int adjustFutan)
        {
            TotalPoint = totalPoint;
            KanFutan = kanFutan;
            TotalSelfExpense = totalSelfExpense;
            Tax = tax;
            DebitBalance = debitBalance;
            SumAdjust = sumAdjust;
            SumAdjustView = sumAdjustView;
            ThisCredit = thisCredit;
            ThisWari = thisWari;
            PayType = payType;
            AdjustFutan = adjustFutan;
        }

        public AccountingInfModel() { }

        public int TotalPoint { get; private set; }

        public int KanFutan { get; private set; }

        public int TotalSelfExpense { get; private set; }

        public int Tax { get; private set; }

        public int DebitBalance { get; private set; }

        public int SumAdjust { get; private set; }

        public int SumAdjustView { get; private set; }

        public int ThisCredit { get; private set; }

        public int ThisWari { get; private set; }

        public int PayType { get; private set; }

        public int AdjustFutan { get; private set; }
    }
}
