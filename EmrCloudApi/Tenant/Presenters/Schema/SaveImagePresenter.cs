using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Schema;
using UseCase.Schema.SaveImage;

namespace EmrCloudApi.Tenant.Presenters.Schema;

public class SaveImagePresenter
{
    public Response<SaveImageResponse> Result { get; private set; } = new();

    public void Complete(SaveImageOutputData output)
    {
        Result.Data = new SaveImageResponse(output.UrlImage);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveImageStatus status) => status switch
    {
        SaveImageStatus.Successed => ResponseMessage.Success,
        SaveImageStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
