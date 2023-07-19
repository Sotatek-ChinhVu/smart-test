using Domain.Models.UserToken;
using UseCase.Core.Sync.Core;

namespace UseCase.UserToken.GetInfoRefresh
{
    public class RefreshTokenByUserOutputData : IOutputData
    {
        public RefreshTokenByUserOutputData(RefreshTokenByUserStatus status, UserTokenModel userToken)
        {
            Status = status;
            UserToken = userToken;
        }

        public RefreshTokenByUserStatus Status { get; private set; }

        public UserTokenModel UserToken { get; private set; }
    }
}
