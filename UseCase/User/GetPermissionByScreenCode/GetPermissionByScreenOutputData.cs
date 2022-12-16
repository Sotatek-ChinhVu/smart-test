using UseCase.Core.Sync.Core;
using static Helper.Constants.UserConst;

namespace UseCase.User.GetPermissionByScreenCode;

public class GetPermissionByScreenOutputData : IOutputData
{
    public GetPermissionByScreenOutputData(GetPermissionByScreenStatus status)
    {
        Status = status;
    }

    public GetPermissionByScreenOutputData(GetPermissionByScreenStatus status, PermissionType permissionType)
    {
        Status = status;
        PermissionType = permissionType;
    }

    public GetPermissionByScreenStatus Status { get; private set; }
    public PermissionType PermissionType { get; private set; }
}
