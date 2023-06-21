namespace Domain.Models.UserToken
{
    public class UserTokenModel
    {
        public UserTokenModel(int userId, string refreshToken, DateTime refreshTokenExpiryTime, bool refreshTokenIsUsed)
        {
            UserId = userId;
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
            RefreshTokenIsUsed = refreshTokenIsUsed;
        }

        public UserTokenModel() => RefreshToken = string.Empty;

        public int UserId { get; set; }

        public string RefreshToken { get; private set; }

        public DateTime RefreshTokenExpiryTime { get; private set; }

        public bool RefreshTokenIsUsed { get; private set; }

        public bool RefreshTokenIsValid => !string.IsNullOrEmpty(RefreshToken) && DateTime.UtcNow <= RefreshTokenExpiryTime && !RefreshTokenIsUsed;
    }
}
