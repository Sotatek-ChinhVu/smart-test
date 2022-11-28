using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ColumnSetting;
using UseCase.ColumnSetting.SaveList;

namespace EmrCloudApi.Presenters.ColumnSetting;

public class SaveColumnSettingListPresenter : ISaveColumnSettingListOutputPort
{
    public Response<SaveColumnSettingListResponse> Result { get; set; } = new();

    public void Complete(SaveColumnSettingListOutputData output)
    {
        Result.Data = new SaveColumnSettingListResponse(output.Status == SaveColumnSettingListStatus.Success);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveColumnSettingListStatus status) => status switch
    {
        SaveColumnSettingListStatus.Failed => ResponseMessage.Failed,
        SaveColumnSettingListStatus.Success => ResponseMessage.Success,
        _ => string.Empty
    };
}
