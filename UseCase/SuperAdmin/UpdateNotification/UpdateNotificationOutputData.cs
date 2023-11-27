using Domain.SuperAdminModels.Notification;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpdateNotification;

public class UpdateNotificationOutputData : IOutputData
{
    public UpdateNotificationOutputData(UpdateNotificationStatus status)
    {
        Status = status;
        NotificationList = new();
    }

    public UpdateNotificationOutputData(List<NotificationModel> notificationList, UpdateNotificationStatus status)
    {
        NotificationList = notificationList;
        Status = status;
    }

    public List<NotificationModel> NotificationList { get; private set; }

    public UpdateNotificationStatus Status { get; private set; }
}
