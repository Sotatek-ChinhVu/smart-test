using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting;

namespace EmrCloudApi.Presenters.Accounting
{
    public class GetAccountingPresenter : IGetAccountingOutputPort
    {
        public Response<GetAccountingResponse> Result { get; private set; } = new();
        public void Complete(GetAccountingOutputData outputData)
        {
            Result.Data = new GetAccountingResponse(outputData.AccountingModel, outputData.AccountingInfModel, outputData.PtByomeiModels);
            Result.Message = GetMessage(outputData.GetAccountingStatus);
            Result.Status = (int)outputData.GetAccountingStatus;
        }

        private string GetMessage(object status) => status switch
        {
            GetAccountingStatus.Successed => ResponseMessage.Success,
            GetAccountingStatus.Failed => ResponseMessage.Failed,
            GetAccountingStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
            GetAccountingStatus.InvalidSindate => ResponseMessage.InvalidSinDate,
            GetAccountingStatus.InvalidPtId => ResponseMessage.InvalidPtId,
            _ => string.Empty
        };
    }
}
