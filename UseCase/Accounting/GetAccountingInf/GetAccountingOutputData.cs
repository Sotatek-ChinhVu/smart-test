using Domain.Models.AccountDue;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetAccountingInf
{
    public class GetAccountingOutputData : IOutputData
    {
        public GetAccountingOutputData(List<SyunoSeikyuModel> syunoSeikyuModels, GetAccountingStatus getAccountingStatus, int totalPoint, int kanFutan, int totalSelfExpense, int tax, int adjustFutan, int debitBalance, int sumAdjust, int sumAdjustView, int thisCredit, int thisWari)
        {
            SyunoSeikyuModels = syunoSeikyuModels;
            GetAccountingStatus = getAccountingStatus;
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

        public List<SyunoSeikyuModel> SyunoSeikyuModels { get; private set; }
        public GetAccountingStatus GetAccountingStatus { get; private set; }
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