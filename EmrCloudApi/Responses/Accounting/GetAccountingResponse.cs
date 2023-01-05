using Domain.Models.Accounting;
using UseCase.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingResponse
    {
        public GetAccountingResponse(AccountingModel accountingModel)
        {
            AccountingModel = accountingModel;
        }

        public AccountingModel AccountingModel { get; private set; }
    }
}
