using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingResponse
    {
        public GetAccountingResponse(List<RaiinInfModel> raiinInfModels, List<AccountingModel> accountingModels, AccountingInfModel accountingInfModel)
        {
            RaiinInfModels = raiinInfModels;
            AccountingModels = accountingModels;
            AccountingInfModel = accountingInfModel;
        }

        public List<RaiinInfModel> RaiinInfModels { get;private set; }
        public List<AccountingModel> AccountingModels { get; private set; }
        public AccountingInfModel AccountingInfModel { get; private set; }
    }
}
