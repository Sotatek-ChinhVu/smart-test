using UseCase.Core.Sync.Core;

namespace UseCase.UserToken.SiginRefresh
{
    public class SigninRefreshTokenInputData : IInputData<SigninRefreshTokenOutputData>
    {
        public SigninRefreshTokenInputData(int userId, string refreshToken, DateTime expToken)
        {
            UserId = userId;
            RefreshToken = refreshToken;
            ExpToken = expToken;
        }

        public int UserId { get; private set; }

        public string RefreshToken { get; private set; }

        public DateTime ExpToken { get; private set; }
    }
}
