using UseCase.SuperAdmin.GetNotification;
using SuperAdmin.Responses;
using SuperAdminAPI.Reponse.Notification;
using SuperAdmin.Constants;

namespace SuperAdminAPI.Presenters.Notification;

public class GetNotificationPresenter : IGetNotificationOutputPort
{
    public Response<GetNotificationResponse> Result { get; private set; } = new();

    public void Complete(GetNotificationOutputData outputData)
    {
        Result.Data = new GetNotificationResponse(outputData.NotificationList.Select(item => new NotificationDto(item)).ToList());
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetNotificationStatus status) => status switch
    {
        GetNotificationStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
