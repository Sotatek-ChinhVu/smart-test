using UseCase.Core.Sync.Core;

namespace UseCase.UserToken.SiginRefresh
{
    public class SigninRefreshTokenOutputData : IOutputData
    {
        public SigninRefreshTokenOutputData(SigninRefreshTokenStatus status) => Status = status;

        public SigninRefreshTokenStatus Status { get; private set; }
    }
}
