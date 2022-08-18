using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.VisitingList;
using UseCase.VisitingList.SaveSettings;

namespace EmrCloudApi.Tenant.Presenters.VisitingList;

public class SaveVisitingListSettingsPresenter : ISaveVisitingListSettingsOutputPort
{
    public Response<SaveVisitingListSettingsResponse> Result { get; set; } = new();

    public void Complete(SaveVisitingListSettingsOutputData output)
    {
        Result.Data = new SaveVisitingListSettingsResponse(output.Status == SaveVisitingListSettingsStatus.Success);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveVisitingListSettingsStatus status) => status switch
    {
        SaveVisitingListSettingsStatus.Success => ResponseMessage.Success,
        _ => string.Empty
    };
}
