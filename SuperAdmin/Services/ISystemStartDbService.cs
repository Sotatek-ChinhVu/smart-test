using UseCase.SuperAdmin.RevokeInsertPermission;

namespace SuperAdminAPI.Services
{
    public interface ISystemStartDbService
    {
        void RevokeInsertPermission();
    }
}
