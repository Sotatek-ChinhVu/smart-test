using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetLastKarute;

namespace EmrCloudApi.Presenters.Reception;

public class GetLastKarutePresenter
{
    public Response<GetLastKaruteResponse> Result { get; private set; } = new();

    public void Complete(GetLastKaruteOutputData output)
    {
        Result.Data = new GetLastKaruteResponse(output.Reception);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetLastKaruteStatus status) => status switch
    {
        GetLastKaruteStatus.Successed => ResponseMessage.Success,
        GetLastKaruteStatus.InvalidPtNum => ResponseMessage.InvalidPtNum,
        GetLastKaruteStatus.InvalidNoData => ResponseMessage.InvalidNoData,
        _ => string.Empty
    };
}
