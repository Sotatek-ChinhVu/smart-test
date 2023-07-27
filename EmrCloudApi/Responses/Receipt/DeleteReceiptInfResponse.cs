using UseCase.ReceiptCheck.ReceiptInfEdit;

namespace EmrCloudApi.Responses.Receipt
{
    public class DeleteReceiptInfResponse
    {
        public DeleteReceiptInfResponse(DeleteReceiptInfStatus status)
        {
            Status = status;
        }

        public DeleteReceiptInfStatus Status { get; private set; }
    }
}
