using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Tenant;
using SuperAdminAPI.Reponse.Tenant.Dto;
using UseCase.SuperAdmin.GetTenant;

namespace SuperAdminAPI.Presenters.Tenant;

public class GetTenantPresenter : IGetTenantOutputPort
{
    public Response<GetTenantResponse> Result { get; private set; } = new();

    public void Complete(GetTenantOutputData outputData)
    {
        Result.Data = new GetTenantResponse(outputData.TenantList.Select(item => new TenantDto(item)).ToList(), outputData.TotalTenant);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetTenantStatus status) => status switch
    {
        GetTenantStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

