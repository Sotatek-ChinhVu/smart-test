using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.GetVisitingColumnSettings;

namespace EmrCloudApi.Tenant.Presenters.Reception;

public class GetVisitingColumnSettingsPresenter : IGetVisitingColumnSettingsOutputPort
{
    public Response<GetVisitingColumnSettingsResponse> Result { get; private set; } = new Response<GetVisitingColumnSettingsResponse>();

    public void Complete(GetVisitingColumnSettingsOutputData output)
    {
        Result.Data = new GetVisitingColumnSettingsResponse(output.Settings);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetVisitingColumnSettingsStatus status) => status switch
    {
        GetVisitingColumnSettingsStatus.Success => ResponseMessage.Success,
        _ => string.Empty
    };
}
