using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ColumnSetting;
using UseCase.ColumnSetting.SaveList;

namespace EmrCloudApi.Tenant.Presenters.ColumnSetting;

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
