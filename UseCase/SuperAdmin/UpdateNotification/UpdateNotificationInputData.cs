using Domain.SuperAdminModels.Notification;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpdateNotification;

public class UpdateNotificationInputData : IInputData<UpdateNotificationOutputData>
{
    public UpdateNotificationInputData(List<NotificationModel> notificationList)
    {
        NotificationList = notificationList;
    }

    public List<NotificationModel> NotificationList { get; private set; }
}
