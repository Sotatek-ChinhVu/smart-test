using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.User;
using UseCase.User.UpdateHashPassword;
using UseCase.User.UserInfo;

namespace EmrCloudApi.Presenters.User
{
    public class UpdateHashPasswordPresenter : IUpdateHashPasswordOutputPort
    {
        public Response<UpdateHashPasswordResponse> Result { get; private set; } = new Response<UpdateHashPasswordResponse>();

        public void Complete(UpdateHashPasswordOutputData outputData)
        {
            Result.Data = new UpdateHashPasswordResponse(outputData.Status);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(UpdateHashPasswordStatus status) => status switch
        {
            UpdateHashPasswordStatus.Success => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
