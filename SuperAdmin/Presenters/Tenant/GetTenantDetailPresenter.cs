using SuperAdminAPI.Reponse.Tenant.Dto;
using SuperAdminAPI.Reponse.Tenant;
using UseCase.SuperAdmin.GetTenantDetail;
using SuperAdmin.Responses;
using SuperAdmin.Constants;

namespace SuperAdminAPI.Presenters.Tenant;

public class GetTenantDetailPresenter : IGetTenantDetailOutputPort
{
    public Response<GetTenantDetailResponse> Result { get; private set; } = new();

    public void Complete(GetTenantDetailOutputData outputData)
    {
        Result.Data = new GetTenantDetailResponse(new TenantDto(outputData.Tenant));
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetTenantDetailStatus status) => status switch
    {
        GetTenantDetailStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
