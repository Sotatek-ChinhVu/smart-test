using UseCase.User.UpdateHashPassword;
using UseCase.User.UserInfo;

namespace EmrCloudApi.Responses.User
{
    public class UpdateHashPasswordResponse
    {
        public UpdateHashPasswordResponse(UpdateHashPasswordStatus status)
        {
            Status = status;
        }

        public UpdateHashPasswordStatus Status { get; private set; }
    }
}
