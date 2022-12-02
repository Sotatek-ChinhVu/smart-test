using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.DeleteDocTemplate;

namespace EmrCloudApi.Presenters.Document;

public class DeleteDocTemplatePresenter : IDeleteDocTemplateOutputPort
{
    public Response<DeleteDocTemplateResponse> Result { get; private set; } = new();

    public void Complete(DeleteDocTemplateOutputData output)
    {
        Result.Data = new DeleteDocTemplateResponse(output.Status == DeleteDocTemplateStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(DeleteDocTemplateStatus status) => status switch
    {
        DeleteDocTemplateStatus.Successed => ResponseMessage.Success,
        DeleteDocTemplateStatus.Failed => ResponseMessage.Failed,
        DeleteDocTemplateStatus.TemplateNotFount => ResponseMessage.TemplateNotFount,
        _ => string.Empty
    };
}
