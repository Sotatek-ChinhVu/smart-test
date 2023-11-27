using SuperAdmin.Constants;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Notification;
using UseCase.SuperAdmin.UpdateNotification;

namespace SuperAdminAPI.Presenters.Notification;

public class UpdateNotificationPresenter : IUpdateNotificationOutputPort
{
    public Response<UpdateNotificationResponse> Result { get; private set; } = new();

    public void Complete(UpdateNotificationOutputData outputData)
    {
        Result.Data = new UpdateNotificationResponse(outputData.Status == UpdateNotificationStatus.Successed);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(UpdateNotificationStatus status) => status switch
    {
        UpdateNotificationStatus.Successed => ResponseMessage.Success,
        UpdateNotificationStatus.Failed => ResponseMessage.Fail,
        UpdateNotificationStatus.InvalidIdNotification => ResponseMessage.InvalidIdNotification,
        _ => string.Empty
    };
}
