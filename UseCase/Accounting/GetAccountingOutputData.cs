﻿using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting
{
    public class GetAccountingOutputData : IOutputData
    {
        public GetAccountingOutputData(List<AccountingModel> accountingModel, AccountingInfModel accountingInfModel, GetAccountingStatus getAccountingStatus)
        {
            AccountingModel = accountingModel;
            AccountingInfModel = accountingInfModel;
            GetAccountingStatus = getAccountingStatus;
        }

        public List<AccountingModel> AccountingModel { get; private set; }
        public AccountingInfModel AccountingInfModel { get; private set; }
        public GetAccountingStatus GetAccountingStatus { get; private set; }
    }
}
