using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Responses;
using UseCase.MainMenu.SaveDailyStatisticMenu;

namespace EmrCloudApi.Presenters.MainMenu;

public class SaveDailyStatisticMenuPresenter : ISaveDailyStatisticMenuOutputPort
{
    public Response<SaveDailyStatisticMenuResponse> Result { get; private set; } = new();

    public void Complete(SaveDailyStatisticMenuOutputData output)
    {
        Result.Data = new SaveDailyStatisticMenuResponse(output.Status == SaveDailyStatisticMenuStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveDailyStatisticMenuStatus status) => status switch
    {
        SaveDailyStatisticMenuStatus.Successed => ResponseMessage.Success,
        SaveDailyStatisticMenuStatus.Failed => ResponseMessage.Failed,
        SaveDailyStatisticMenuStatus.InvalidMenuId => ResponseMessage.InvalidMenuId,
        SaveDailyStatisticMenuStatus.InvalidGrpId => ResponseMessage.InvalidGrpId,
        SaveDailyStatisticMenuStatus.InvalidReportId => ResponseMessage.InvalidReportId,
        SaveDailyStatisticMenuStatus.InvalidMenuName => ResponseMessage.InvalidMenuName,
        _ => string.Empty
    };
}
