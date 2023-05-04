using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MainMenu;
using UseCase.MainMenu.GetStatisticMenu;

namespace EmrCloudApi.Presenters.MainMenu;

public class GetStatisticMenuPresenter : IGetStatisticMenuOutputPort
{
    public Response<GetStatisticMenuResponse> Result { get; private set; } = new();

    public void Complete(GetStatisticMenuOutputData output)
    {
        Result.Data = new GetStatisticMenuResponse(output.StatisticMenuList, output.StaGrpItemList);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetStatisticMenuStatus status) => status switch
    {
        GetStatisticMenuStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
