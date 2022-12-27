using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.MoveTemplateToOtherCategory;

namespace EmrCloudApi.Presenters.Document;

public class MoveTemplateToOtherCategoryPresenter : IMoveTemplateToOtherCategoryOutputPort
{
    public Response<MoveTemplateToOtherCategoryResponse> Result { get; private set; } = new();

    public void Complete(MoveTemplateToOtherCategoryOutputData output)
    {
        Result.Data = new MoveTemplateToOtherCategoryResponse(output.Status == MoveTemplateToOtherCategoryStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(MoveTemplateToOtherCategoryStatus status) => status switch
    {
        MoveTemplateToOtherCategoryStatus.Successed => ResponseMessage.Success,
        MoveTemplateToOtherCategoryStatus.Failed => ResponseMessage.Failed,
        MoveTemplateToOtherCategoryStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        MoveTemplateToOtherCategoryStatus.InvalidNewCategoryCd => ResponseMessage.InvalidNewCategoryCd,
        MoveTemplateToOtherCategoryStatus.InvalidOldCategoryCd => ResponseMessage.InvalidOldCategoryCd,
        MoveTemplateToOtherCategoryStatus.FileTemplateNotFould => ResponseMessage.FileTemplateNotFould,
        MoveTemplateToOtherCategoryStatus.FileTemplateIsExistInNewFolder => ResponseMessage.FileTemplateIsExistInNewFolder,
        _ => string.Empty
    };
}
