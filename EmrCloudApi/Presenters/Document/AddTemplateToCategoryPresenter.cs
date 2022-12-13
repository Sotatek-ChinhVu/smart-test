using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.AddTemplateToCategory;

namespace EmrCloudApi.Presenters.Document;

public class AddTemplateToCategoryPresenter
{
    public Response<AddTemplateToCategoryResponse> Result { get; private set; } = new();

    public void Complete(AddTemplateToCategoryOutputData output)
    {
        Result.Data = new AddTemplateToCategoryResponse(output.Status == AddTemplateToCategoryStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(AddTemplateToCategoryStatus status) => status switch
    {
        AddTemplateToCategoryStatus.Successed => ResponseMessage.Success,
        AddTemplateToCategoryStatus.Failed => ResponseMessage.Failed,
        AddTemplateToCategoryStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        AddTemplateToCategoryStatus.InvalidCategoryCd => ResponseMessage.InvalidDocumentCategoryCd,
        AddTemplateToCategoryStatus.InvalidFileInput => ResponseMessage.InvalidFileInput,
        AddTemplateToCategoryStatus.ExistFileTemplateName => ResponseMessage.ExistFileTemplateName,
        _ => string.Empty
    };
}
