using Domain.Common;

namespace Domain.SuperAdminModels.Notification;

public interface INotificationRepository : IRepositoryBase
{
    List<NotificationModel> GetNotificationList(int skip, int take);

    List<NotificationModel> UpdateNotificationList(List<NotificationModel> notificationList);

    bool CheckExistNotification(List<int> notificationIdList);
}
