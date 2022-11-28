using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.VisitingList;
using UseCase.VisitingList.SaveSettings;

namespace EmrCloudApi.Presenters.VisitingList;

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
