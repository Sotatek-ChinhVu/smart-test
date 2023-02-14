using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.SaveListReceCmt;

namespace EmrCloudApi.Presenters.Receipt;

public class SaveListReceCmtPresenter : ISaveListReceCmtOutputPort
{
    public Response<SaveListReceCmtResponse> Result { get; private set; } = new();

    public void Complete(SaveListReceCmtOutputData outputData)
    {
        Result.Data = new SaveListReceCmtResponse(outputData.Status == SaveListReceCmtStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveListReceCmtStatus status) => status switch
    {
        SaveListReceCmtStatus.Successed => ResponseMessage.Success,
        SaveListReceCmtStatus.Failed => ResponseMessage.Failed,
        SaveListReceCmtStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SaveListReceCmtStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        SaveListReceCmtStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveListReceCmtStatus.InvalidSinYm => ResponseMessage.InvalidSinYm,
        SaveListReceCmtStatus.InvalidItemCd => ResponseMessage.InvalidItemCd,
        SaveListReceCmtStatus.InvalidReceCmtId => ResponseMessage.InvalidReceCmtId,
        SaveListReceCmtStatus.InvalidCmtKbn => ResponseMessage.InvalidCmtKbn,
        SaveListReceCmtStatus.InvalidCmtSbt => ResponseMessage.InvalidCmtSbt,
        SaveListReceCmtStatus.InvalidCmt => ResponseMessage.InvalidCmt,
        SaveListReceCmtStatus.InvalidCmtData => ResponseMessage.InvalidCmtData,
        _ => string.Empty
    };
}