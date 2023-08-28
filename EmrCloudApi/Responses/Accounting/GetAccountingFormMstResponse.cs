using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Domain.Models.Insurance;

namespace EmrCloudApi.Responses.Accounting
{
    public class GetAccountingFormMstResponse
    {
        public GetAccountingFormMstResponse(List<AccountingFormMstModel> accountingFormMstModels)
        {
            AccountingFormMstModels = accountingFormMstModels;
        }

        public List<AccountingFormMstModel> AccountingFormMstModels { get; private set; }
    }
}
