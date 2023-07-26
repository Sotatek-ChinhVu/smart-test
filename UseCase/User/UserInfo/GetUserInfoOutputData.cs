using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.UserInfo
{
    public class GetUserInfoOutputData : IOutputData
    {
        public GetUserInfoOutputData(GetUserInfoStatus status, UserMstDto userInfo)
        {
            Status = status;
            UserInfo = userInfo;
        }

        public GetUserInfoStatus Status { get; private set; }
        public UserMstDto UserInfo { get; private set; }
    }
}
