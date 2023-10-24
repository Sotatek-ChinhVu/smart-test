using EmrCloudApi.Constants;
using EmrCloudApi.Responses.ColumnSetting;
using EmrCloudApi.Responses;
using UseCase.ColumnSetting.GetColumnSettingByTableNameList;

namespace EmrCloudApi.Presenters.ColumnSetting;

public class GetColumnSettingByTableNameListPresenter : IGetColumnSettingByTableNameListOutputPort
{
    public Response<GetColumnSettingByTableNameListResponse> Result { get; set; } = new();

    public void Complete(GetColumnSettingByTableNameListOutputData output)
    {
        Result.Data = new GetColumnSettingByTableNameListResponse(output.SettingList);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetColumnSettingByTableNameListStatus status) => status switch
    {
        GetColumnSettingByTableNameListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}