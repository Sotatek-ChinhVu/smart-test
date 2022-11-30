using EmrCloudApi.Constants;
using UseCase.Document.GetDocCategoryDetail;

namespace EmrCloudApi.Responses.Document;

public class GetDocCategoryDetailPresenter : IGetDocCategoryDetailOutputPort
{
    public Response<GetDocCategoryDetailResponse> Result { get; private set; } = new();

    public void Complete(GetDocCategoryDetailOutputData output)
    {
        Result.Data = new GetDocCategoryDetailResponse(
                                                        output.DocCategory,
                                                        output.ListTemplates,
                                                        output.DocInfs
                                                    );
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetDocCategoryDetailStatus status) => status switch
    {
        GetDocCategoryDetailStatus.Successed => ResponseMessage.Success,
        GetDocCategoryDetailStatus.Failed => ResponseMessage.Failed,
        GetDocCategoryDetailStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetDocCategoryDetailStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        GetDocCategoryDetailStatus.InvalidCategoryCd => ResponseMessage.InvalidDocumentCategoryCd,
        _ => string.Empty
    };
}
