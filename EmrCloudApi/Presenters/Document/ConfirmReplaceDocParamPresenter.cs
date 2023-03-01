using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.ConfirmReplaceDocParam;

namespace EmrCloudApi.Presenters.Document;

public class ConfirmReplaceDocParamPresenter : IConfirmReplaceDocParamOutputPort
{
    public Response<ConfirmReplaceDocParamResponse> Result { get; private set; } = new();

    public void Complete(ConfirmReplaceDocParamOutputData output)
    {
        Result.Data = new ConfirmReplaceDocParamResponse(output.DocComments);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(ConfirmReplaceDocParamStatus status) => status switch
    {
        ConfirmReplaceDocParamStatus.Successed => ResponseMessage.Success,
        ConfirmReplaceDocParamStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
