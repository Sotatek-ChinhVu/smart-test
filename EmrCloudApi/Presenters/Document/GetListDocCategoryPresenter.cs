using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Tenant.Responses.Document;
using UseCase.Document.GetListDocCategory;

namespace EmrCloudApi.Tenant.Presenters.Document;

public class GetListDocCategoryPresenter : IGetListDocCategoryOutputPort
{
    public Response<GetListDocCategoryResponse> Result { get; private set; } = new();

    public void Complete(GetListDocCategoryOutputData output)
    {
        Result.Data = new GetListDocCategoryResponse(
                                                        output.ListDocCategories.Select(item => new DocCategoryDto(item)).ToList(),
                                                        output.ListTemplates.Select(item => new FileDocumentDto(item)).ToList()
                                                    );
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListDocCategoryStatus status) => status switch
    {
        GetListDocCategoryStatus.Successed => ResponseMessage.Success,
        GetListDocCategoryStatus.Failed => ResponseMessage.Failed,
        GetListDocCategoryStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        _ => string.Empty
    };
}
