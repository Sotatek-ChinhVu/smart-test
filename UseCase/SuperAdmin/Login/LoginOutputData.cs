using Domain.Models.SuperSetDetail;
using Domain.SuperAdminModels.Admin;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.Login;

public class LoginOutputData : IOutputData
{
    public LoginOutputData(AdminModel user, LoginStatus status)
    {
        User = user;
        Status = status;
    }

    public AdminModel User { get; private set; }

    public LoginStatus Status { get; private set; }
}
