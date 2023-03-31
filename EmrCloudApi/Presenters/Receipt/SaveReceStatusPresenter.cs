using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.SaveReceStatus;

namespace EmrCloudApi.Presenters.Receipt;

public class SaveReceStatusPresenter : ISaveReceStatusOutputPort
{
    public Response<SaveReceStatusResponse> Result { get; private set; } = new();

    public void Complete(SaveReceStatusOutputData outputData)
    {
        Result.Data = new SaveReceStatusResponse(outputData.Status == SaveReceStatusStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveReceStatusStatus status) => status switch
    {
        SaveReceStatusStatus.Successed => ResponseMessage.Success,
        SaveReceStatusStatus.Failed => ResponseMessage.Failed,
        SaveReceStatusStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveReceStatusStatus.InvalidSinYm => ResponseMessage.InvalidSinYm,
        SaveReceStatusStatus.InvalidSeikyuYm => ResponseMessage.InvalidSeikyuYm,
        SaveReceStatusStatus.InvalidHokenId => ResponseMessage.InvalidHokenId,
        SaveReceStatusStatus.InvalidFusenKbn => ResponseMessage.InvalidFusenKbn,
        SaveReceStatusStatus.InvalidStatusKbn => ResponseMessage.InvalidStatusKbn,
        _ => string.Empty
    };
}