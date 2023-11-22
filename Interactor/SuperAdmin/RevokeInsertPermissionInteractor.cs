using Domain.SuperAdminModels.Tenant;
using UseCase.SuperAdmin.RevokeInsertPermission;

namespace Interactor.SuperAdmin
{
    public class RevokeInsertPermissionInteractor : IRevokeInsertPermissionInputPort
    {
        private readonly ITenantRepository _tenantRepository;

        public RevokeInsertPermissionInteractor(ITenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public RevokeInsertPermissionOutputData Handle(RevokeInsertPermissionInputData inputData)
        {
            try
            {
                _tenantRepository.RevokeInsertPermission();

                return new RevokeInsertPermissionOutputData(true);
            }
            catch (Exception)
            {
                return new RevokeInsertPermissionOutputData(false);
            }
            finally
            {
                _tenantRepository.ReleaseResource();
            }
        }
    }
}
