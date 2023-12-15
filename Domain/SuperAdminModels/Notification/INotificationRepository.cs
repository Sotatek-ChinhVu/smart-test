using Domain.Common;

namespace Domain.SuperAdminModels.Notification;

public interface INotificationRepository : IRepositoryBase
{
    public NotificationModel CreateNotification(byte status, string messenge);

    (List<NotificationModel> NotificationList, int TotalItem) GetNotificationList(int skip, int take, bool onlyUnreadNotifications);

    List<NotificationModel> UpdateNotificationList(List<NotificationModel> notificationList);

    bool CheckExistNotification(List<int> notificationIdList);

    /// <summary>
    /// Read all notifications
    /// </summary>
    List<NotificationModel> ReadAllNotification();
}
