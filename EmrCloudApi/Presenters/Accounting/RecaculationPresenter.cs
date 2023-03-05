using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Accounting;
using UseCase.Accounting.Recaculate;

namespace EmrCloudApi.Presenters.Accounting
{
    public class RecaculationPresenter : IRecaculationOutputPort
    {
        public Response<RecaculationResponse> Result { get; private set; } = new();
        public void Complete(RecaculationOutputData outputData)
        {
            Result.Data = new RecaculationResponse(outputData.Status);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(object status) => status switch
        {
            RecaculationStatus.Successed => ResponseMessage.Success,
            RecaculationStatus.Failed => ResponseMessage.Failed,
            RecaculationStatus.NoData => ResponseMessage.NoData,
            _ => string.Empty
        };
    }
}
