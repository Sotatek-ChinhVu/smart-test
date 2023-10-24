using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Online.Dto;
using EmrCloudApi.Responses.Online;
using EmrCloudApi.Responses;
using UseCase.Online.GetListOnlineConfirmationHistoryModel;

namespace EmrCloudApi.Presenters.Online;

public class GetListOnlineConfirmationHistoryModelPresenter : IGetListOnlineConfirmationHistoryModelOutputPort
{
    public Response<GetListOnlineConfirmationHistoryModelResponse> Result { get; private set; } = new();

    public void Complete(GetListOnlineConfirmationHistoryModelOutputData output)
    {
        Result.Data = new GetListOnlineConfirmationHistoryModelResponse(output.OnlineConfirmationHistoryList.Select(item => new OnlineConfirmationHistoryDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListOnlineConfirmationHistoryModelStatus status) => status switch
    {
        GetListOnlineConfirmationHistoryModelStatus.Successed => ResponseMessage.Success,
        GetListOnlineConfirmationHistoryModelStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        _ => string.Empty
    };
}

