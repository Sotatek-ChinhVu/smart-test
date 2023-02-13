using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Accounting.GetAccountingInf
{
    public class GetAccountingOutputData : IOutputData
    {
        public GetAccountingOutputData(List<AccountingModel> accountingModel, AccountingInfModel accountingInfModel, List<PtByomeiModel> ptByomeiModels, GetAccountingStatus getAccountingStatus)
        {
            AccountingModel = accountingModel;
            AccountingInfModel = accountingInfModel;
            PtByomeiModels = ptByomeiModels;
            GetAccountingStatus = getAccountingStatus;
        }

        public List<AccountingModel> AccountingModel { get; private set; }
        public AccountingInfModel AccountingInfModel { get; private set; }
        public List<PtByomeiModel> PtByomeiModels { get; private set; }
        public GetAccountingStatus GetAccountingStatus { get; private set; }
    }
}
