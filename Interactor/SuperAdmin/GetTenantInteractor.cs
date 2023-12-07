using Domain.SuperAdminModels.Tenant;
using UseCase.SuperAdmin.GetTenant;

namespace Interactor.SuperAdmin;

public class GetTenantInteractor : IGetTenantInputPort
{
    private readonly ITenantRepository _tenantRepository;

    public GetTenantInteractor(ITenantRepository tenantRepository)
    {
        _tenantRepository = tenantRepository;
    }

    public GetTenantOutputData Handle(GetTenantInputData inputData)
    {
        try
        {
            var result = _tenantRepository.GetTenantList(inputData.SearchModel, inputData.SortDictionary, inputData.Skip, inputData.Take);
            return new GetTenantOutputData(result.TenantList, result.TotalTenant, GetTenantStatus.Successed);
        }
        finally
        {
            _tenantRepository.ReleaseResource();
        }
    }
}
