using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.SaveReceCheckCmtList;

namespace EmrCloudApi.Presenters.Receipt;

public class SaveReceCheckCmtListPresenter : ISaveReceCheckCmtListOutputPort
{
    public Response<SaveReceCheckCmtListResponse> Result { get; private set; } = new();

    public void Complete(SaveReceCheckCmtListOutputData outputData)
    {
        Result.Data = new SaveReceCheckCmtListResponse(outputData.Status == SaveReceCheckCmtListStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveReceCheckCmtListStatus status) => status switch
    {
        SaveReceCheckCmtListStatus.Successed => ResponseMessage.Success,
        SaveReceCheckCmtListStatus.Failed => ResponseMessage.Failed,
        SaveReceCheckCmtListStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveReceCheckCmtListStatus.InvalidSinYm => ResponseMessage.InvalidSinYm,
        SaveReceCheckCmtListStatus.InvalidCmt => ResponseMessage.InvalidCmt,
        SaveReceCheckCmtListStatus.InvalidHokenId => ResponseMessage.InvalidHokenId,
        SaveReceCheckCmtListStatus.InvalidSeqNo => ResponseMessage.InvalidSeqNo,
        SaveReceCheckCmtListStatus.InvalidStatusColor => ResponseMessage.InvalidStatusColor,
        _ => string.Empty
    };
}
