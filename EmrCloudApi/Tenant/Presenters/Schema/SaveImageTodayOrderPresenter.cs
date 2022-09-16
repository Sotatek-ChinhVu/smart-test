using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Schema;
using UseCase.Schema.SaveImageTodayOrder;

namespace EmrCloudApi.Tenant.Presenters.Schema;

public class SaveImageTodayOrderPresenter
{
    public Response<SaveImageResponse> Result { get; private set; } = new();

    public void Complete(SaveImageTodayOrderOutputData output)
    {
        Result.Data = new SaveImageResponse(output.UrlImage);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveImageTodayOrderStatus status) => status switch
    {
        SaveImageTodayOrderStatus.Successed => ResponseMessage.Success,
        SaveImageTodayOrderStatus.Failed => ResponseMessage.Failed,
        SaveImageTodayOrderStatus.InvalidOldImage => ResponseMessage.InvalidOldImage,
        SaveImageTodayOrderStatus.InvalidPtId => ResponseMessage.InvalidPtId,
        SaveImageTodayOrderStatus.InvalidFileImage => ResponseMessage.InvalidFileImage,
        SaveImageTodayOrderStatus.DeleteSuccessed => ResponseMessage.DeleteSuccessed,
        _ => string.Empty
    };
}
