using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.User;
using UseCase.User.UserInfo;

namespace EmrCloudApi.Presenters.User
{
    public class GetUserInfoPresenter : IGetUserInfoOutputPort
    {
        public Response<GetUserInfoResponse> Result { get; private set; } = new Response<GetUserInfoResponse>();

        public void Complete(GetUserInfoOutputData outputData)
        {
            Result.Data = new GetUserInfoResponse(outputData.UserInfo);
            Result.Message = GetMessage(outputData.Status);
            Result.Status = (int)outputData.Status;
        }

        private string GetMessage(GetUserInfoStatus status) => status switch
        {
            GetUserInfoStatus.Success => ResponseMessage.Success,
            _ => string.Empty
        };
    }
}
