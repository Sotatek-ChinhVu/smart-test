using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.ReceiptCheck.ReceiptInfEdit;

namespace EmrCloudApi.Presenters.Receipt
{
    public class DeleteReceiptInfPresenter : IDeleteReceiptInfEditOutputPort
    {
        public Response<DeleteReceiptInfResponse> Result { get; private set; } = new();
        public void Complete(DeleteReceiptInfEditOutputData outputData)
        {
            Result.Data = new DeleteReceiptInfResponse(outputData.Status);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(DeleteReceiptInfStatus status) => status switch
        {
            DeleteReceiptInfStatus.Successed => ResponseMessage.Success,
            DeleteReceiptInfStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
