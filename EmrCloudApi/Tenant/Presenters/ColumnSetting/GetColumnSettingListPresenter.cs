using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.ColumnSetting;
using UseCase.ColumnSetting.GetList;

namespace EmrCloudApi.Tenant.Presenters.ColumnSetting;

public class GetColumnSettingListPresenter : IGetColumnSettingListOutputPort
{
    public Response<GetColumnSettingListResponse> Result { get; set; } = new();

    public void Complete(GetColumnSettingListOutputData output)
    {
        Result.Data = new GetColumnSettingListResponse(output.Settings);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetColumnSettingListStatus status) => status switch
    {
        GetColumnSettingListStatus.Success => ResponseMessage.Success,
        GetColumnSettingListStatus.NoData => ResponseMessage.NoData,
        _ => string.Empty
    };
}
