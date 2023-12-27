using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.JsonSetting;
using UseCase.JsonSetting.GetAll;

namespace EmrCloudApi.Presenters.ColumnSetting;

public class GetAllJsonSettingPresenter : IGetAllJsonSettingOutputPort
{
    public Response<GetAllJsonSettingResponse> Result { get; private set; } = new();

    public void Complete(GetAllJsonSettingOutputData output)
    {
        Result.Data = new GetAllJsonSettingResponse(output.Settings);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetAllJsonSettingStatus status) => status switch
    {
        GetAllJsonSettingStatus.Success => ResponseMessage.Success,
        GetAllJsonSettingStatus.NotFound => ResponseMessage.NotFound,
        _ => string.Empty
    };
}
