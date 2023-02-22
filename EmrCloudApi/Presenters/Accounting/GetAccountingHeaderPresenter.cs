using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.GetAccountingHeader;

namespace EmrCloudApi.Presenters.Accounting
{
    public class GetAccountingHeaderPresenter : IGetAccountingHeaderOutputPort
    {
        public Response<GetAccountingHeaderResponse> Result { get; private set; } = new();
        public void Complete(GetAccountingHeaderOutputData outputData)
        {
            Result.Data = new GetAccountingHeaderResponse(outputData.PersonNumber, outputData.HeaderDtos);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }
        private string GetMessage(object status) => status switch
        {
            GetAccountingHeaderStatus.Successed => ResponseMessage.Success,
            GetAccountingHeaderStatus.Failed => ResponseMessage.Failed,
            GetAccountingHeaderStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
