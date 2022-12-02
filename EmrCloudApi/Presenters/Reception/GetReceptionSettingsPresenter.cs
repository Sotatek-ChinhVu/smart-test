using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetSettings;

namespace EmrCloudApi.Presenters.Reception;

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
