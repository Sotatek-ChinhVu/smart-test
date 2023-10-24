using EmrCloudApi.Constants;
using EmrCloudApi.Responses.Logger;
using EmrCloudApi.Responses;
using UseCase.Logger.WriteListLog;

namespace EmrCloudApi.Presenters.Logger;

public class WriteListLogPresenter : IWriteListLogOutputPort
{
    public Response<WriteListLogResponse> Result { get; private set; } = new();

    public void Complete(WriteListLogOutputData output)
    {
        Result.Data = new WriteListLogResponse(output.Status == WriteListLogStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(WriteListLogStatus status) => status switch
    {
        WriteListLogStatus.Successed => ResponseMessage.Success,
        WriteListLogStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
