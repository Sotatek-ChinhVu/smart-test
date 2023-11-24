using UseCase.Core.Sync;
using UseCase.SuperAdmin.RevokeInsertPermission;

namespace SuperAdminAPI.Services
{
    public class SystemStartDbService : ISystemStartDbService
    {
        private readonly UseCaseBus _bus;

        public SystemStartDbService(UseCaseBus bus)
        {
            _bus = bus;
        }

        public void RevokeInsertPermission()
        {
            var input = new RevokeInsertPermissionInputData();
            var output = _bus.Handle(input);
        }
    }
}
