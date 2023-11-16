using Domain.SuperAdminModels.Logger;
using UseCase.SuperAdmin.AuditLog;

namespace Interactor.SuperAdmin.AuditLog;

public class GetAuditLogListInteractor : IGetAuditLogListInputPort
{
    private readonly IAdminAuditLogRepository _adminAuditLogRepository;

    public GetAuditLogListInteractor(IAdminAuditLogRepository adminAuditLogRepository)
    {
        _adminAuditLogRepository = adminAuditLogRepository;
    }

    public GetAuditLogListOutputData Handle(GetAuditLogListInputData inputData)
    {
        try
        {
            var result = _adminAuditLogRepository.GetAuditLogList(inputData.TenantId, inputData.RequestModel, inputData.SortDictionary, inputData.Skip, inputData.Take);
            return new GetAuditLogListOutputData(result, GetAuditLogListStatus.Successed);
        }
        finally
        {
            _adminAuditLogRepository.ReleaseResource();
        }
    }
}
