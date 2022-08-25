using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.JsonSetting;
using UseCase.JsonSetting.Upsert;

namespace EmrCloudApi.Tenant.Presenters.ColumnSetting;

public class UpsertJsonSettingPresenter : IUpsertJsonSettingOutputPort
{
    public Response<UpsertJsonSettingResponse> Result { get; private set; } = new();

    public void Complete(UpsertJsonSettingOutputData output)
    {
        Result.Data = new UpsertJsonSettingResponse(output.Status == UpsertJsonSettingStatus.Success);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UpsertJsonSettingStatus status) => status switch
    {
        UpsertJsonSettingStatus.Success => ResponseMessage.Success,
        _ => string.Empty
    };
}
