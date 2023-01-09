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

        public int TotalPoint { get; set; }

        public int KanFutan { get; set; }

        public int TotalSelfExpense { get; set; }

        public int Tax { get; set; }

        public int DebitBalance { get; set; }

        public int SumAdjust { get; set; }

        public int SumAdjustView { get; set; }

        public int ThisCredit { get; set; }

        public int ThisWari { get; set; }

        public int PayType { get; set; }

        public int AdjustFutan { get; set; }
    }
}
