using Domain.SuperAdminModels.Tenant;
using UseCase.SuperAdmin.GetTenantDetail;

namespace Interactor.SuperAdmin;

public class GetTenantDetailInteractor : IGetTenantDetailInputPort
{
    private readonly ITenantRepository _tenantRepository;

    public GetTenantDetailInteractor(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public GetTenantDetailOutputData Handle(GetTenantDetailInputData inputData)
    {
        try
        {
            var result = _tenantRepository.GetTenant(inputData.TenantId);
            return new GetTenantDetailOutputData(result, GetTenantDetailStatus.Successed);
        }
        finally
        {
            _tenantRepository.ReleaseResource();
        }
    }
}
