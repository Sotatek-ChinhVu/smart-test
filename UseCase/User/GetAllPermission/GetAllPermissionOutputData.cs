using Domain.Models.User;
using UseCase.Core.Sync.Core;

namespace UseCase.User.GetAllPermission;

public class GetAllPermissionOutputData : IOutputData
{
    public GetAllPermissionOutputData(GetAllPermissionStatus status)
    {
        Status = status;
    }

    public GetAllPermissionOutputData(GetAllPermissionStatus status, List<UserPermissionModel> userPermissions)
    {
        Status = status;
        UserPermissions = userPermissions;
    }

    public GetAllPermissionStatus Status { get; private set; }
    public List<UserPermissionModel> UserPermissions { get; private set; } = new();
}
