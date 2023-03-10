using UseCase.Accounting.CheckOpenAccounting;

namespace EmrCloudApi.Responses.Accounting
{
    public class CheckOpenAccountingResponse
    {
        public CheckOpenAccountingResponse(CheckOpenAccountingStatus status)
        {
            Status = status;
        }

        public CheckOpenAccountingStatus Status { get; private set; }
    }
}
