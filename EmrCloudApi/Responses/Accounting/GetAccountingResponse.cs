using Domain.Models.AccountDue;
using Domain.Models.Insurance;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingResponse
    {
        public GetAccountingResponse(List<SyunoSeikyuModel> syunoSeikyuModels, int totalPoint, int kanFutan, int totalSelfExpense, int tax, int adjustFutan, int debitBalance, int sumAdjust, int sumAdjustView, int thisCredit, int thisWari, List<KohiInfModel> kohiInfModels)
        {
            SyunoSeikyuModels = syunoSeikyuModels;
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
            KohiInfModels = kohiInfModels;
        }

        public List<SyunoSeikyuModel> SyunoSeikyuModels { get; private set; }
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
        public List<KohiInfModel> KohiInfModels { get; private set; }
    }
}
