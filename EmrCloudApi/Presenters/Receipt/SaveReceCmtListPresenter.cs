using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.SaveListReceCmt;

namespace EmrCloudApi.Presenters.Receipt;

public class SaveReceCmtListPresenter : ISaveReceCmtListOutputPort
{
    public Response<SaveReceCmtListResponse> Result { get; private set; } = new();

    public void Complete(SaveReceCmtListOutputData outputData)
    {
        Result.Data = new SaveReceCmtListResponse(outputData.Status == SaveReceCmtListStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveReceCmtListStatus status) => status switch
    {
        SaveReceCmtListStatus.Successed => ResponseMessage.Success,
        SaveReceCmtListStatus.Failed => ResponseMessage.Failed,
        SaveReceCmtListStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveReceCmtListStatus.InvalidSinYm => ResponseMessage.InvalidSinYm,
        SaveReceCmtListStatus.InvalidItemCd => ResponseMessage.InvalidItemCd,
        SaveReceCmtListStatus.InvalidReceCmtId => ResponseMessage.InvalidReceCmtId,
        SaveReceCmtListStatus.InvalidCmtKbn => ResponseMessage.InvalidCmtKbn,
        SaveReceCmtListStatus.InvalidCmtSbt => ResponseMessage.InvalidCmtSbt,
        SaveReceCmtListStatus.InvalidCmt => ResponseMessage.InvalidCmt,
        SaveReceCmtListStatus.InvalidCmtData => ResponseMessage.InvalidCmtData,
        SaveReceCmtListStatus.InvalidHokenId => ResponseMessage.InvalidHokenId,
        _ => string.Empty
    };
}