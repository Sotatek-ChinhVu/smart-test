using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.CheckAccountingStatus;

namespace EmrCloudApi.Presenters.Accounting
{
    public class CheckAccountingStatusPresenter : ICheckAccountingStatusOutputPort
    {
        public Response<CheckAccountingStatusResponse> Result { get; private set; } = new();
        public void Complete(CheckAccountingStatusOutputData outputData)
        {
            Result.Data = new CheckAccountingStatusResponse(outputData.ErrorType, outputData.Message);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
        private string GetMessage(object status) => status switch
        {
            CheckAccountingStatus.Successed => ResponseMessage.Success,
            CheckAccountingStatus.Failed => ResponseMessage.Failed,
            CheckAccountingStatus.NoData => ResponseMessage.NoData,
            CheckAccountingStatus.StateChanged => ResponseMessage.StateChanged,
            CheckAccountingStatus.VisitRemoved => ResponseMessage.VisitRemoved,
            CheckAccountingStatus.BillUpdated => ResponseMessage.BillUpdated,
            CheckAccountingStatus.ValidPaymentAmount => ResponseMessage.ValidPaymentAmount,
            CheckAccountingStatus.ValidThisCredit => ResponseMessage.InvalidThisCredit,
            CheckAccountingStatus.DateNotVerify => ResponseMessage.DateNotVerify,
            _ => string.Empty
        };
    }
}