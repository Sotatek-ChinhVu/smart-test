using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Document;
using UseCase.Document.GetListDocCategoryMst;

namespace EmrCloudApi.Tenant.Presenters.Document;

public class GetListDocCategoryMstPresenter : IGetListDocCategoryMstOutputPort
{
    public Response<GetListDocCategoryMstResponse> Result { get; private set; } = new();

    public void Complete(GetListDocCategoryMstOutputData output)
    {
        Result.Data = new GetListDocCategoryMstResponse(
                                                        output.ListDocCategories.Select(item => new DocCategoryDto(item)).ToList(),
                                                        output.ListTemplates.Select(item => new FileDocumentDto(item)).ToList()
                                                    );
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListDocCategoryMstStatus status) => status switch
    {
        GetListDocCategoryMstStatus.Successed => ResponseMessage.Success,
        GetListDocCategoryMstStatus.Failed => ResponseMessage.Failed,
        GetListDocCategoryMstStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        _ => string.Empty
    };
}
