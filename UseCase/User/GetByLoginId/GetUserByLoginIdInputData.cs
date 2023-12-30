using UseCase.Core.Sync.Core;

namespace UseCase.User.GetByLoginId;

public class GetUserByLoginIdInputData : IInputData<GetUserByLoginIdOutputData>
{
    public GetUserByLoginIdInputData(string loginId, string password)
    {
        LoginId = loginId;
        Password = password;
    }

    public string LoginId { get; private set; }

    public string Password { get; private set; }
}
