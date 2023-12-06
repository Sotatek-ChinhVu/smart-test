
namespace SuperAdmin.Responses.Admin
{
    public class LoginResponse
    {
        public LoginResponse ( string token, int userId, string name, string fullName, int role, string refreshToken, DateTime refreshTokenExpiryTime)
        {
            Token = token;
            UserId = userId;
            Name = name;
            FullName = fullName;
            Role = role;
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }

        public LoginResponse()
        {
            Token = string.Empty;
            Name = string.Empty;
            FullName = string.Empty;
            RefreshToken = string.Empty;
        }

        public string  Token { get; private set; }

        public int UserId { get; private set; }

        public string Name { get; private set; }

        public string FullName { get; private set; }

        public int Role { get; private set; }

        public string RefreshToken { get; private set; }

        public DateTime RefreshTokenExpiryTime { get; private set; }
    }
}
