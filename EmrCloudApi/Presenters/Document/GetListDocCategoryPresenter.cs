using EmrCloudApi.Constants;
using UseCase.Document.GetListDocCategory;

namespace EmrCloudApi.Responses.Document;

public class GetListDocCategoryPresenter : IGetListDocCategoryOutputPort
{
    public Response<GetListDocCategoryResponse> Result { get; private set; } = new();

    public void Complete(GetListDocCategoryOutputData output)
    {
        Result.Data = new GetListDocCategoryResponse(
                                                        output.ListDocCategories,
                                                        output.ListTemplates,
                                                        output.DocInfs
                                                    );
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListDocCategoryStatus status) => status switch
    {
        GetListDocCategoryStatus.Successed => ResponseMessage.Success,
        GetListDocCategoryStatus.Failed => ResponseMessage.Failed,
        GetListDocCategoryStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetListDocCategoryStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        _ => string.Empty
    };
}
