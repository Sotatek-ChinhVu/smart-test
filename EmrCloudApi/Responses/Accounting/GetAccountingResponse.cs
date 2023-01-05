using Domain.Models.Accounting;
using UseCase.Accounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingResponse
    {
        public GetAccountingResponse(AccountingModel accountingModel, GetAccountingStatus getAccountingStatus)
        {
            AccountingModel = accountingModel;
            GetAccountingStatus = getAccountingStatus;
        }

        public AccountingModel AccountingModel { get; private set; }
        public GetAccountingStatus GetAccountingStatus { get; private set; }
    }
}
