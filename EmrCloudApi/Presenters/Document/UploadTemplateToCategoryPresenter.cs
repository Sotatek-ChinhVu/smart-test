using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Document;
using UseCase.Document.UploadTemplateToCategory;

namespace EmrCloudApi.Presenters.Document;

public class UploadTemplateToCategoryPresenter
{
    public Response<UploadTemplateToCategoryResponse> Result { get; private set; } = new();

    public void Complete(UploadTemplateToCategoryOutputData output)
    {
        Result.Data = new UploadTemplateToCategoryResponse(output.Status == UploadTemplateToCategoryStatus.Successed);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(UploadTemplateToCategoryStatus status) => status switch
    {
        UploadTemplateToCategoryStatus.Successed => ResponseMessage.Success,
        UploadTemplateToCategoryStatus.Failed => ResponseMessage.Failed,
        UploadTemplateToCategoryStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        UploadTemplateToCategoryStatus.InvalidCategoryCd => ResponseMessage.InvalidDocumentCategoryCd,
        UploadTemplateToCategoryStatus.InvalidFileInput => ResponseMessage.InvalidFileInput,
        UploadTemplateToCategoryStatus.ExistFileTemplateName => ResponseMessage.ExistFileTemplateName,
        _ => string.Empty
    };
}
