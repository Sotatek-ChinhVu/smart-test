using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.Login;

public class LoginOutputData : IOutputData
{
    public LoginOutputData(LoginStatus status)
    {
        Status = status;
    }

    public LoginStatus Status { get; private set; }
}
