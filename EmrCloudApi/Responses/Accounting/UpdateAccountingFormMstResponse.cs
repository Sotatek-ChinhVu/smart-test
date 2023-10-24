using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Domain.Models.Insurance;

namespace EmrCloudApi.Responses.Accounting
{
    public class UpdateAccountingFormMstResponse
    {
        public UpdateAccountingFormMstResponse(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}
