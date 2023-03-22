using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.SaveReceiptEdit;

namespace EmrCloudApi.Presenters.Receipt;

public class SaveReceiptEditPresenter : ISaveReceiptEditOutputPort
{
    public Response<SaveReceiptEditResponse> Result { get; private set; } = new();

    public void Complete(SaveReceiptEditOutputData outputData)
    {
        Result.Data = new SaveReceiptEditResponse(outputData.Status == SaveReceiptEditStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveReceiptEditStatus status) => status switch
    {
        SaveReceiptEditStatus.Successed => ResponseMessage.Success,
        SaveReceiptEditStatus.Failed => ResponseMessage.Failed,
        SaveReceiptEditStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveReceiptEditStatus.InvalidSinYm => ResponseMessage.InvalidSinYm,
        SaveReceiptEditStatus.InvalidSeikyuYm => ResponseMessage.InvalidSeikyuYm,
        SaveReceiptEditStatus.InvalidHokenId => ResponseMessage.InvalidHokenId,
        SaveReceiptEditStatus.InvalidSeqNo => ResponseMessage.InvalidSeqNo,
        SaveReceiptEditStatus.InvalidNissuItem => ResponseMessage.InvalidNissuItem,
        SaveReceiptEditStatus.InvalidTokkiItem => ResponseMessage.InvalidTokkiItem,
        _ => string.Empty
    };
}