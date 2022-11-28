using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.JsonSetting;
using UseCase.JsonSetting.Get;

namespace EmrCloudApi.Presenters.ColumnSetting;

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
