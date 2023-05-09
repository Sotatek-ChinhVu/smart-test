using UseCase.Core.Sync.Core;

namespace UseCase.User.GetAllPermission;

public class GetAllPermissionInputData : IInputData<GetAllPermissionOutputData>
{
    public GetAllPermissionInputData(int hpId, int userId)
    {
        HpId = hpId;
        UserId = userId;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
}
