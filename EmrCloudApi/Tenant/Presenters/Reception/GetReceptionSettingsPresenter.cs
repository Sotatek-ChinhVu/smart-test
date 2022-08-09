using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Reception;
using UseCase.Reception.GetSettings;

namespace EmrCloudApi.Tenant.Presenters.Reception;

public class GetReceptionSettingsPresenter : IGetReceptionSettingsOutputPort
{
    public Response<GetReceptionSettingsResponse> Result { get; private set; } = new Response<GetReceptionSettingsResponse>();

    public void Complete(GetReceptionSettingsOutputData output)
    {
        Result.Data = new GetReceptionSettingsResponse(output.Settings);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetReceptionSettingsStatus status) => status switch
    {
        GetReceptionSettingsStatus.Success => ResponseMessage.Success,
        _ => string.Empty
    };
}
