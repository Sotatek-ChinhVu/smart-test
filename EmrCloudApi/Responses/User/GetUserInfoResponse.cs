using UseCase.User.UserInfo;

namespace EmrCloudApi.Responses.User
{
    public class GetUserInfoResponse
    {
        public GetUserInfoResponse(UserMstDto userInfo)
        {
            UserInfo = userInfo;
        }

        public UserMstDto UserInfo { get; private set; }
    }
}
