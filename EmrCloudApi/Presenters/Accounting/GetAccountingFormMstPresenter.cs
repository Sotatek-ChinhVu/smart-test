using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.CheckAccountingStatus;
using UseCase.Accounting.GetAccountingFormMst;
using UseCase.Accounting.UpdateAccountingFormMst;

namespace EmrCloudApi.Presenters.Accounting
{
    public class GetAccountingFormMstPresenter : IGetAccountingFormMstOutputPort
    {
        public Response<GetAccountingFormMstResponse> Result { get; private set; } = new();
        public void Complete(GetAccountingFormMstOutputData outputData)
        {
            Result.Data = new GetAccountingFormMstResponse(outputData.AccountingFormMstModels);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
        private string GetMessage(object status) => status switch
        {
            GetAccountingFormMstStatus.Successed => ResponseMessage.Success,
            GetAccountingFormMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            _ => string.Empty
        };
    }
}