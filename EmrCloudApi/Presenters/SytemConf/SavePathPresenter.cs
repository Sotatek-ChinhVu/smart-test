using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SystemConf;
using UseCase.SystemConf.SavePath;

namespace EmrCloudApi.Presenters.SytemConf;

public class SavePathPresenter : ISavePathOutputPort
{
    public Response<SavePathResponse> Result { get; private set; } = new();

    public void Complete(SavePathOutputData outputData)
    {
        Result.Data = new SavePathResponse(outputData.Status == SavePathStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(SavePathStatus status) => status switch
    {
        SavePathStatus.Successed => ResponseMessage.Success,
        SavePathStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}

