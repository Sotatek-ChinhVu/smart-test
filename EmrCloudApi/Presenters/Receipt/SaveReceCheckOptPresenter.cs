using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.SaveReceCheckOpt;

namespace EmrCloudApi.Presenters.Receipt;

public class SaveReceCheckOptPresenter : ISaveReceCheckOptOutputPort
{
    public Response<SaveReceCheckOptResponse> Result { get; private set; } = new();

    public void Complete(SaveReceCheckOptOutputData outputData)
    {
        Result.Data = new SaveReceCheckOptResponse(outputData.Status == SaveReceCheckOptStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveReceCheckOptStatus status) => status switch
    {
        SaveReceCheckOptStatus.Successed => ResponseMessage.Success,
        SaveReceCheckOptStatus.Failed => ResponseMessage.Failed,
        SaveReceCheckOptStatus.InvalidErrCd => ResponseMessage.InvalidErrCd,
        SaveReceCheckOptStatus.InvalidCheckOpt => ResponseMessage.InvalidCheckOpt,
        _ => string.Empty
    };
}
