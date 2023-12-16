using Domain.SuperAdminModels.Notification;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpdateNotification;

public class UpdateNotificationInputData : IInputData<UpdateNotificationOutputData>
{
    public UpdateNotificationInputData(List<NotificationModel> notificationList, bool isRealAllNotifications)
    {
        NotificationList = notificationList;
        IsRealAllNotifications = isRealAllNotifications;
    }

    public List<NotificationModel> NotificationList { get; private set; }

    public bool IsRealAllNotifications { get; private set; }
}
