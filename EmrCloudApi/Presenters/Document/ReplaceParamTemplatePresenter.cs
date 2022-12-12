using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.ReplaceParamTemplate;

namespace EmrCloudApi.Presenters.Document;

public class ReplaceParamTemplatePresenter : IReplaceParamTemplateOutputPort
{
    public Response<ReplaceParamTemplateResponse> Result { get; private set; } = new();

    public void Complete(ReplaceParamTemplateOutputData output)
    {
        Result.Data = new ReplaceParamTemplateResponse(output.Status == ReplaceParamTemplateStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(ReplaceParamTemplateStatus status) => status switch
    {
        ReplaceParamTemplateStatus.Successed => ResponseMessage.Success,
        ReplaceParamTemplateStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}

