using Domain.Models.User;

namespace EmrCloudApi.Responses.User
{
    public class GetUserInfoResponse
    {
        public GetUserInfoResponse(UserMstModel userInfo)
        {
            UserInfo = userInfo;
        }

        public UserMstModel UserInfo { get; private set; }
    }
}
