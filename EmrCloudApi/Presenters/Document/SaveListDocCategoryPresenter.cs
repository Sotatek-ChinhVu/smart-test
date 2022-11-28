using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Tenant.Responses.Document;
using UseCase.Document.SaveListDocCategory;

namespace EmrCloudApi.Tenant.Presenters.Document;

public class SaveListDocCategoryPresenter : ISaveListDocCategoryOutputPort
{
    public Response<SaveListDocCategoryResponse> Result { get; private set; } = new();

    public void Complete(SaveListDocCategoryOutputData output)
    {
        Result.Data = new SaveListDocCategoryResponse(output.Status == SaveListDocCategoryStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveListDocCategoryStatus status) => status switch
    {
        SaveListDocCategoryStatus.Successed => ResponseMessage.Success,
        SaveListDocCategoryStatus.Failed => ResponseMessage.Failed,
        SaveListDocCategoryStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SaveListDocCategoryStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        SaveListDocCategoryStatus.InvalidCategoryCd => ResponseMessage.InvalidDocumentCategoryCd,
        SaveListDocCategoryStatus.InvalidCategoryName => ResponseMessage.InvalidDocumentCategoryName,
        SaveListDocCategoryStatus.InvalidSortNo => ResponseMessage.InvalidSortNo,
        _ => string.Empty
    };
}
