using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.CheckAccountingStatus;
using UseCase.Accounting.UpdateAccountingFormMst;

namespace EmrCloudApi.Presenters.Accounting
{
    public class UpdateAccountingFormMstPresenter : IUpdateAccountingFormMstOutputPort
    {
        public Response<UpdateAccountingFormMstResponse> Result { get; private set; } = new();
        public void Complete(UpdateAccountingFormMstOutputData outputData)
        {
            Result.Data = new UpdateAccountingFormMstResponse(outputData.Status == UpdateAccountingFormMstStatus.Successed);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
        private string GetMessage(object status) => status switch
        {
            UpdateAccountingFormMstStatus.Successed => ResponseMessage.Success,
            UpdateAccountingFormMstStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            UpdateAccountingFormMstStatus.InputNoData => ResponseMessage.InputNoData,
            _ => string.Empty
        };
    }
}