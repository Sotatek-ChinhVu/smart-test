using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using UseCase.Yousiki.UpdateYosiki;

namespace EmrCloudApi.Presenters.Yousiki
{
    public class UpdateYosikiPresenter : IUpdateYosikiOutputPort
    {
        public Response<UpdateYosikiResponse> Result { get; private set; } = new();
        public void Complete(UpdateYosikiOutputData outputData)
        {
            Result.Data = new UpdateYosikiResponse(outputData.Status, outputData.Message);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(UpdateYosikiStatus status) => status switch
        {
            UpdateYosikiStatus.Successed => ResponseMessage.Success,
            UpdateYosikiStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
