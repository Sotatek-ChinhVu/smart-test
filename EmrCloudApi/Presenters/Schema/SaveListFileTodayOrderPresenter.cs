using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Schema;
using UseCase.Schema.SaveListImageTodayOrder;

namespace EmrCloudApi.Presenters.Schema;

public class SaveListFileTodayOrderPresenter : ISaveListFileTodayOrderOutputPort
{
    public Response<SaveListFileTodayOrderResponse> Result { get; private set; } = new();

    public void Complete(SaveListFileTodayOrderOutputData outputData)
    {
        Result.Data = new SaveListFileTodayOrderResponse(outputData.SeqNo);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveListFileTodayOrderStatus status) => status switch
    {
        SaveListFileTodayOrderStatus.Successed => ResponseMessage.Success,
        SaveListFileTodayOrderStatus.Failed => ResponseMessage.Failed,
        SaveListFileTodayOrderStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SaveListFileTodayOrderStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveListFileTodayOrderStatus.InvalidRaiinNo => ResponseMessage.InvalidRaiinNo,
        SaveListFileTodayOrderStatus.InvalidFileImage => ResponseMessage.InvalidFileImage,
        SaveListFileTodayOrderStatus.InvalidListFileIdDeletes => ResponseMessage.InvalidListFileDeletes,
        _ => string.Empty
    };
}
