using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MainMenu;
using UseCase.Releasenote.UpdateListReleasenote;

namespace EmrCloudApi.Presenters.ReleasenoteRead
{
    public class UpdateListReleasenotePresenter : IUpdateListReleasenoteOutputPort
    {
        public Response<UpdateListReleasenoteResponse> Result { get; private set; } = new();
        public void Complete(UpdateListReleasenoteOutputData outputData)
        {
            Result.Data = new UpdateListReleasenoteResponse(outputData.Status == UpdateListReleasenoteStatus.Successed);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(UpdateListReleasenoteStatus status) => status switch
        {
            UpdateListReleasenoteStatus.Successed => ResponseMessage.Success,
            UpdateListReleasenoteStatus.Failed => ResponseMessage.Failed,
            _ => string.Empty
        };
    }
}
