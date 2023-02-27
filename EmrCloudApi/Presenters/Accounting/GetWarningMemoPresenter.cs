using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.WarningMemo;

namespace EmrCloudApi.Presenters.Accounting
{
    public class GetWarningMemoPresenter : IGetWarningMemoOutputPort
    {
        public Response<GetWarningMemoResponse> Result { get; private set; } = new();

        public void Complete(GetWarningMemoOutputData outputData)
        {
            Result.Data = new GetWarningMemoResponse(outputData.WarningMemoModels);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(object status) => status switch
        {
            GetWarningMemoStatus.Successed => ResponseMessage.Success,
            GetWarningMemoStatus.Failed => ResponseMessage.Failed,
            GetWarningMemoStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
