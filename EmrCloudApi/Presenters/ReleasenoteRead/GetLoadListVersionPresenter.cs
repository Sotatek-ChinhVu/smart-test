using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ReleasenoteRead;
using UseCase.Releasenote.LoadListVersion;

namespace EmrCloudApi.Presenters.ReleasenoteRead
{
    public class GetLoadListVersionPresenter : IGetLoadListVersionOutputPort
    {
        public Response<GetLoadListVersionResponse> Result { get; private set; } = new();
        public void Complete(GetLoadListVersionOutputData outputData)
        {
            Result.Data = new GetLoadListVersionResponse(outputData.Data, outputData.ShowReleaseNote, outputData.Status);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetLoadListVersionStatus status) => status switch
        {
            GetLoadListVersionStatus.Successed => ResponseMessage.Success,
            GetLoadListVersionStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
