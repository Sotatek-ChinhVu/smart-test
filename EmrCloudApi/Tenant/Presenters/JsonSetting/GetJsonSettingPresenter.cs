using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.JsonSetting;
using UseCase.JsonSetting.Get;

namespace EmrCloudApi.Tenant.Presenters.ColumnSetting;

public class GetJsonSettingPresenter : IGetJsonSettingOutputPort
{
    public Response<GetJsonSettingResponse> Result { get; private set; } = new();

    public void Complete(GetJsonSettingOutputData output)
    {
        Result.Data = new GetJsonSettingResponse(output.Setting);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetJsonSettingStatus status) => status switch
    {
        GetJsonSettingStatus.Success => ResponseMessage.Success,
        GetJsonSettingStatus.NotFound => ResponseMessage.NotFound,
        _ => string.Empty
    };
}
