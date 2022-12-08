using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.SortDocCategory;

namespace EmrCloudApi.Presenters.Document;

public class SortDocCategoryPresenter : ISortDocCategoryOutputPort
{
    public Response<SortDocCategoryResponse> Result { get; private set; } = new();

    public void Complete(SortDocCategoryOutputData output)
    {
        Result.Data = new SortDocCategoryResponse(output.Status == SortDocCategoryStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SortDocCategoryStatus status) => status switch
    {
        SortDocCategoryStatus.Successed => ResponseMessage.Success,
        SortDocCategoryStatus.Failed => ResponseMessage.Failed,
        SortDocCategoryStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        SortDocCategoryStatus.InvalidUserId => ResponseMessage.InvalidUserId,
        SortDocCategoryStatus.InvalidMoveInCd => ResponseMessage.InvalidMoveInDocCategoryCd,
        SortDocCategoryStatus.InvalidMoveOutCd => ResponseMessage.InvalidMoveOutDocCategoryCd,
        _ => string.Empty
    };
}
