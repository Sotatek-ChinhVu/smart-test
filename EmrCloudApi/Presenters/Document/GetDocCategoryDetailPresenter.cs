using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Tenant.Responses.Document;
using UseCase.Document.GetDocCategoryDetail;

namespace EmrCloudApi.Tenant.Presenters.Document;

public class GetDocCategoryDetailPresenter : IGetDocCategoryDetailOutputPort
{
    public Response<GetDocCategoryDetailResponse> Result { get; private set; } = new();

    public void Complete(GetDocCategoryDetailOutputData output)
    {
        Result.Data = new GetDocCategoryDetailResponse(
                                                        new DocCategoryDto(output.DocCategory),
                                                        output.ListTemplates.Select(item => new FileDocumentDto(item)).ToList()
                                                    );
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetDocCategoryDetailStatus status) => status switch
    {
        GetDocCategoryDetailStatus.Successed => ResponseMessage.Success,
        GetDocCategoryDetailStatus.Failed => ResponseMessage.Failed,
        GetDocCategoryDetailStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetDocCategoryDetailStatus.InvalidCategoryCd => ResponseMessage.InvalidDocumentCategoryCd,
        _ => string.Empty
    };
}
