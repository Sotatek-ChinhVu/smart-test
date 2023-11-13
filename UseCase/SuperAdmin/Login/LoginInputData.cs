using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.Login;

public class LoginInputData : IInputData<LoginOutputData>
{
    public LoginInputData(int loginId, string password)
    {
        LoginId = loginId;
        Password = password;
    }

    public int LoginId { get; private set; }

    public string Password { get; private set; }
}
