using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MainMenu;
using UseCase.MainMenu.GetDailyStatisticMenu;

namespace EmrCloudApi.Presenters.MainMenu;

public class GetDailyStatisticMenuPresenter : IGetDailyStatisticMenuOutputPort
{
    public Response<GetDailyStatisticMenuResponse> Result { get; private set; } = new();

    public void Complete(GetDailyStatisticMenuOutputData output)
    {
        Result.Data = new GetDailyStatisticMenuResponse(output.StatisticMenuList, output.StaGrpItemList);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetDailyStatisticMenuStatus status) => status switch
    {
        GetDailyStatisticMenuStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
