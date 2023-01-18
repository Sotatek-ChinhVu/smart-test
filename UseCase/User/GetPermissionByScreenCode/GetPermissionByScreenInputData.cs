using UseCase.Core.Sync.Core;

namespace UseCase.User.GetPermissionByScreenCode;

public class GetPermissionByScreenInputData : IInputData<GetPermissionByScreenOutputData>
{
    public GetPermissionByScreenInputData(int hpId, int userId, string permissionCode)
    {
        HpId = hpId;
        UserId = userId;
        PermissionCode = permissionCode;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
    public string PermissionCode { get; private set; }
}
