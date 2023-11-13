
namespace SuperAdmin.Responses.Admin
{
    public class LoginResponse
    {
        public LoginResponse(bool result)
        {
            Result = result;
        }

        public bool Result { get; private set; }
    }
}
