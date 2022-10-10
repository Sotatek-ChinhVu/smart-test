using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Schema;
using UseCase.Schema.SaveImageSuperSetDetail;

namespace EmrCloudApi.Tenant.Presenters.SetMst;

public class SaveImageSuperSetDetailPresenter
{
    public Response<SaveImageResponse> Result { get; private set; } = new();

    public void Complete(SaveImageSuperSetDetailOutputData output)
    {
        Result.Data = new SaveImageResponse(output.UrlImage);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveImageSuperSetDetailStatus status) => status switch
    {
        SaveImageSuperSetDetailStatus.Successed => ResponseMessage.Success,
        SaveImageSuperSetDetailStatus.Failed => ResponseMessage.Failed,
        SaveImageSuperSetDetailStatus.InvalidOldImage => ResponseMessage.InvalidOldImage,
        SaveImageSuperSetDetailStatus.InvalidSetCd => ResponseMessage.InvalidSetCd,
        SaveImageSuperSetDetailStatus.InvalidFileImage => ResponseMessage.InvalidFileImage,
        SaveImageSuperSetDetailStatus.DeleteSuccessed => ResponseMessage.DeleteSuccessed,
        _ => string.Empty
    };
}
