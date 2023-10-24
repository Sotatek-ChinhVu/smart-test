using EmrCloudApi.Constants;
using EmrCloudApi.Responses.MainMenu;
using EmrCloudApi.Responses;
using UseCase.MainMenu.SaveStatisticMenu;

namespace EmrCloudApi.Presenters.MainMenu;

public class SaveStatisticMenuPresenter : ISaveStatisticMenuOutputPort
{
    public Response<SaveStatisticMenuResponse> Result { get; private set; } = new();

    public void Complete(SaveStatisticMenuOutputData output)
    {
        Result.Data = new SaveStatisticMenuResponse(output.MenuIdTemp, output.Status == SaveStatisticMenuStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveStatisticMenuStatus status) => status switch
    {
        SaveStatisticMenuStatus.Successed => ResponseMessage.Success,
        SaveStatisticMenuStatus.Failed => ResponseMessage.Failed,
        SaveStatisticMenuStatus.InvalidMenuId => ResponseMessage.InvalidMenuId,
        SaveStatisticMenuStatus.InvalidGrpId => ResponseMessage.InvalidGrpId,
        SaveStatisticMenuStatus.InvalidReportId => ResponseMessage.InvalidReportId,
        SaveStatisticMenuStatus.InvalidMenuName => ResponseMessage.InvalidMenuName,
        SaveStatisticMenuStatus.NoPermission => ResponseMessage.NoPermission,
        _ => string.Empty
    };
}
