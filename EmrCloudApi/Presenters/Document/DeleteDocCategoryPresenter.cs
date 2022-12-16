using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.DeleteDocCategory;

namespace EmrCloudApi.Presenters.Document;

public class DeleteDocCategoryPresenter : IDeleteDocCategoryOutputPort
{
    public Response<DeleteDocCategoryResponse> Result { get; private set; } = new();

    public void Complete(DeleteDocCategoryOutputData output)
    {
        Result.Data = new DeleteDocCategoryResponse(output.Status == DeleteDocCategoryStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(DeleteDocCategoryStatus status) => status switch
    {
        DeleteDocCategoryStatus.Successed => ResponseMessage.Success,
        DeleteDocCategoryStatus.Failed => ResponseMessage.Failed,
        DeleteDocCategoryStatus.DocCategoryNotFound => ResponseMessage.InvalidDocumentCategoryCd,
        DeleteDocCategoryStatus.MoveDocCategoryNotFound => ResponseMessage.MoveDocCategoryNotFound,
        DeleteDocCategoryStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        DeleteDocCategoryStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        _ => string.Empty
    };
}
