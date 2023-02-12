using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingResponse
    {
        public GetAccountingResponse(List<RaiinInfModel> raiinInfModels, List<AccountingModel> accountingModels, AccountingInfModel accountingInfModel, List<WarningMemoModel> warningMemoModels)
        {
            RaiinInfModels = raiinInfModels;
            AccountingModels = accountingModels;
            AccountingInfModel = accountingInfModel;
            WarningMemoModels = warningMemoModels;
        }

        public List<RaiinInfModel> RaiinInfModels { get; private set; }
        public List<AccountingModel> AccountingModels { get; private set; }
        public AccountingInfModel AccountingInfModel { get; private set; }
        public List<WarningMemoModel> WarningMemoModels { get; private set; }

    }
}
