using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using EmrCloudApi.Responses.MstItem.Dto;
using UseCase.MstItem.GetRenkeiTiming;

namespace EmrCloudApi.Presenters.MstItem;

public class GetRenkeiTimingPresenter : IGetRenkeiTimingOutputPort
{
    public Response<GetRenkeiTimingResponse> Result { get; private set; } = new();

    public void Complete(GetRenkeiTimingOutputData output)
    {
        Result.Data = new GetRenkeiTimingResponse(output.RenkeiTimingList.Select(item => new RenkeiTimingDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetRenkeiTimingStatus status) => status switch
    {
        GetRenkeiTimingStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}