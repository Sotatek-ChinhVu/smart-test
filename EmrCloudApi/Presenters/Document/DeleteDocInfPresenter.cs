using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.DeleteDocInf;

namespace EmrCloudApi.Presenters.Document;

public class DeleteDocInfPresenter : IDeleteDocInfOutputPort
{
    public Response<DeleteDocInfResponse> Result { get; private set; } = new();

    public void Complete(DeleteDocInfOutputData output)
    {
        Result.Data = new DeleteDocInfResponse(output.Status == DeleteDocInfStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(DeleteDocInfStatus status) => status switch
    {
        DeleteDocInfStatus.Successed => ResponseMessage.Success,
        DeleteDocInfStatus.Failed => ResponseMessage.Failed,
        DeleteDocInfStatus.DocInfNotFount => ResponseMessage.DocInfNotFount,
        _ => string.Empty
    };
}
