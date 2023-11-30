
namespace SuperAdmin.Responses.Admin
{
    public class LoginResponse
    {
        public LoginResponse ( string token, int userId)
        {
            Token = token;
            UserId = userId;
        }

        public string  Token { get; private set; }

        public int UserId { get; private set; }
    }
}
