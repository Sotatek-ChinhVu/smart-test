using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.RevokeInsertPermission
{
    public class RevokeInsertPermissionOutputData : IOutputData
    {
        public RevokeInsertPermissionOutputData(bool success)
        {
            Success = success;
        }

        public bool Success { get; private set; }
    }
}
