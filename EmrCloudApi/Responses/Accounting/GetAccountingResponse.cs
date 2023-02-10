using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingResponse
    {
        public GetAccountingResponse(List<AccountingModel> accountingModels, AccountingInfModel accountingInfModel)
        {
            AccountingModels = accountingModels;
            AccountingInfModel = accountingInfModel;
        }

        public List<AccountingModel> AccountingModels { get; private set; }
        public AccountingInfModel AccountingInfModel { get; private set; }
    }
}
