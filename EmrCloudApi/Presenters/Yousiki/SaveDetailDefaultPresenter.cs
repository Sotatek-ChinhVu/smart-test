using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Yousiki;
using UseCase.Yousiki.SaveDetailDefault;

namespace EmrCloudApi.Presenters.Yousiki;

public class SaveDetailDefaultPresenter : ISaveDetailDefaultOutputPort
{
    public Response<SaveDetailDefaultResponse> Result { get; private set; } = new();
    public void Complete(SaveDetailDefaultOutputData outputData)
    {
        Result.Data = new SaveDetailDefaultResponse(outputData.Status == SaveDetailDefaultStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SaveDetailDefaultStatus status) => status switch
    {
        SaveDetailDefaultStatus.Successed => ResponseMessage.Success,
        SaveDetailDefaultStatus.Failed => ResponseMessage.Failed,
        SaveDetailDefaultStatus.InvalidMode => ResponseMessage.InvalidMode,
        _ => string.Empty
    };
}
