using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetSendaiGeneration;
using UseCase.SetSendaiGeneration.Restore;

namespace EmrCloudApi.Presenters.SetSendaiGeneration
{
    public class RestoreSetSendaiGenerationPresenter : IRestoreSetSendaiGenerationOutputPort
    {
        public Response<RestoreSetSendaiGenerationResponse> Result { get; private set; } = new();

        public void Complete(RestoreSetSendaiGenerationOutputData output)
        {
            Result.Data = new RestoreSetSendaiGenerationResponse(output.Result);
            Result.Message = GetMessage(output.Status);
            Result.Status = (int)output.Status;
        }

        private string GetMessage(RestoreSetSendaiGenerationStatus status) => status switch
        {
            RestoreSetSendaiGenerationStatus.InvalidHpId => ResponseMessage.InvalidHpId,
            RestoreSetSendaiGenerationStatus.InvalidUserId => ResponseMessage.InvalidUserId,
            RestoreSetSendaiGenerationStatus.InvalidRestoreGenerationId => ResponseMessage.InvalidGenarationId,
            RestoreSetSendaiGenerationStatus.Faild => ResponseMessage.Failed,
            RestoreSetSendaiGenerationStatus.Success => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
