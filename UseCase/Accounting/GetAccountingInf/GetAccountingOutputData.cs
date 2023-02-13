using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetAccountingInf
{
    public class GetAccountingOutputData : IOutputData
    {
        public GetAccountingOutputData(List<AccountingModel> accountingModel, GetAccountingStatus getAccountingStatus)
        {
            AccountingModel = accountingModel;
            GetAccountingStatus = getAccountingStatus;
            TotalPoint = GetToTalPoint();
            KanFutan = GetKanFutan();
            TotalSelfExpense = GetTotalSelfExpense();
            Tax = GetTax();
            AdjustFutan = GetAdjustFutan();
            DebitBalance = GetDebitBalance();
            SumAdjust = GetSumAdjust();
            SumAdjustView = GetSumAdjustView();
            ThisCredit = GetThisCredit();
            ThisWari = GetThisWari();
        }

        public List<AccountingModel> AccountingModel { get; private set; }
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
        #region Caculate
        public int GetToTalPoint() { return AccountingModel.Sum(item => item.SyunoSeikyu.SeikyuTensu); }
        public int GetKanFutan() { return AccountingModel.Sum(item => item.PtFutan + item.AdjustRound); }
        public int GetTotalSelfExpense() { return AccountingModel.Sum(item => item.JihiFutan + item.JihiOuttax); }
        public int GetTax() { return AccountingModel.Sum(item => item.JihiTax + item.JihiOuttax); }
        public int GetAdjustFutan() { return AccountingModel.Sum(item => item.AdjFutan); }
        public int GetDebitBalance()
        {
            return AccountingModel.Sum(item => item.SyunoSeikyu.SeikyuGaku -
                                                      item.SyunoNyukinModels.Sum(itemNyukin =>
                                                          itemNyukin.NyukinGaku + itemNyukin.AdjustFutan));
        }
        public int GetSumAdjust() { return Adjust(); }
        public int GetSumAdjustView() { return AdjustView(); }
        public int GetThisCredit() { return SumCredit(); }
        public int GetThisWari() { return SumWari(); }


        private bool isSettled
        {
            get => AccountingModel.Select(item => item.SyunoSeikyu.NyukinKbn != 0).FirstOrDefault();
        }


        private int Adjust()
        {
            if (isSettled)
            {
                return AccountingModel.Sum(item => item.SyunoSeikyu.SeikyuGaku);
            }
            return AccountingModel.Sum(item => item.SyunoSeikyu.SeikyuGaku);

        }
        private int AdjustView()
        {
            var SumAdjustView = 0;
            if (isSettled)
            {
                return SumAdjust;
            }
            return SumAdjustView += DebitBalance;
        }

        private int SumCredit()
        {
            var ThisCredit = 0;
            if (isSettled)
            {
                return AccountingModel.Sum(item => item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.NyukinGaku));
            }
            return SumAdjust;
        }

        private int SumWari()
        {
            if (isSettled)
            {
                return AccountingModel.Sum(item => item.SyunoNyukinModels.Sum(itemNyukin => itemNyukin.AdjustFutan));
            }
            return 0;
        }
        #endregion

    }
}
