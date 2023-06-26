using UseCase.Core.Sync.Core;

namespace UseCase.UserToken.GetInfoRefresh
{
    public class RefreshTokenByUserInputData : IInputData<RefreshTokenByUserOutputData>
    {
        public RefreshTokenByUserInputData(int userId, string refreshToken, string newRefreshToken, double refreshHour)
        {
            UserId = userId;
            RefreshToken = refreshToken;
            NewRefreshToken = newRefreshToken;
            RefreshHour = refreshHour;
        }

        public int UserId { get; private set; }

        public string RefreshToken { get; private set; }

        public string NewRefreshToken { get; private set; }

        public double RefreshHour { get; private set; }
    }
}
