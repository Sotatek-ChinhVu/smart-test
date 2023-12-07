using Domain.SuperAdminModels.Notification;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.GetNotification;

public class GetNotificationOutputData : IOutputData
{
    public GetNotificationOutputData(List<NotificationModel> notificationList, int totalItem, GetNotificationStatus status)
    {
        NotificationList = notificationList;
        TotalItem = totalItem;
        Status = status;
    }

    public int TotalItem { get; private set; }

    public List<NotificationModel> NotificationList { get; private set; }

    public GetNotificationStatus Status { get; private set; }
}
