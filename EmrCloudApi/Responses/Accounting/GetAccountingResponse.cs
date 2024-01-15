﻿using Domain.Models.AccountDue;
using Domain.Models.Insurance;
using UseCase.Accounting.GetAccountingInf;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingResponse
    {
        public GetAccountingResponse(List<RaiinInfItem> listRaiinInf, List<SyunoSeikyuModel> syunoSeikyuModels, int totalPoint, int kanFutan, int totalSelfExpense, int tax, int adjustFutan, int debitBalance, int sumAdjust, int sumAdjustView, int thisCredit, int thisWari, int payType, string comment, List<KohiInfModel> kohiInfModels, List<SyunoSeikyuModel> allSyunoSeikyuModel, bool isSettled)
        {
            ListRaiinInf = listRaiinInf;
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
            PayType = payType;
            Comment = comment;
            KohiInfModels = kohiInfModels;
            AllSyunoSeikyuModel = allSyunoSeikyuModel;
            IsSettled = isSettled;
        }

        public List<RaiinInfItem> ListRaiinInf { get; private set; }
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
        public int PayType { get; private set; }
        public string Comment { get; private set; }
        public List<KohiInfModel> KohiInfModels { get; private set; }
        public List<SyunoSeikyuModel> AllSyunoSeikyuModel { get; private set; }
        public bool IsSettled { get; private set; }
    }
}
