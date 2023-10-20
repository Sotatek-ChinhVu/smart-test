using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetNextUketukeNoBySetting;

namespace EmrCloudApi.Presenters.Reception;

public class GetNextUketukeNoBySettingPresenter : IGetNextUketukeNoBySettingOutputPort
{
    public Response<GetNextUketukeNoBySettingResponse> Result { get; private set; } = new();

    public void Complete(GetNextUketukeNoBySettingOutputData output)
    {
        Result.Data = new GetNextUketukeNoBySettingResponse(output.NextUketukeNo);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetNextUketukeNoBySettingStatus status) => status switch
    {
        GetNextUketukeNoBySettingStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
