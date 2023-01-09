using Domain.Models.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingResponse
    {
        public GetAccountingResponse(AccountingInfModel accountingInfModel)
        {
            AccountingInfModel = accountingInfModel;
        }

        public AccountingInfModel AccountingInfModel { get; private set; }
    }
}
