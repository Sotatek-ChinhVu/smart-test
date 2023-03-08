using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.CheckOpenAccounting;

namespace EmrCloudApi.Presenters.Accounting
{
    public class CheckOpenAccountingPresenter : ICheckOpenAccountingOutputPort
    {
        public Response<CheckOpenAccountingResponse> Result { get; private set; } = new();
        public void Complete(CheckOpenAccountingOutputData outputData)
        {
            Result.Data = new CheckOpenAccountingResponse(outputData.Status);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
        private string GetMessage(object status) => status switch
        {
            CheckOpenAccountingStatus.Successed => ResponseMessage.Success,
            CheckOpenAccountingStatus.Failed => ResponseMessage.Failed,
            CheckOpenAccountingStatus.NoPaymentInfo => ResponseMessage.NoData,
            CheckOpenAccountingStatus.TryAgainLater => ResponseMessage.StateChanged,
            _ => string.Empty
        };
    }
}
