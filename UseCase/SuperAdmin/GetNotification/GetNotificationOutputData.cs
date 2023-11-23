using Domain.SuperAdminModels.Notification;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.GetNotification;

public class GetNotificationOutputData : IOutputData
{
    public GetNotificationOutputData(List<NotificationModel> notificationList, GetNotificationStatus status)
    {
        NotificationList = notificationList;
        Status = status;
    }

    public List<NotificationModel> NotificationList { get; private set; }

    public GetNotificationStatus Status { get; private set; }
}
