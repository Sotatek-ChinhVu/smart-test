using Domain.Common;

namespace Domain.SuperAdminModels.Notification;

public interface INotificationRepository : IRepositoryBase
{
    public bool CreateNotification(byte status, string messenge);
    List<NotificationModel> GetNotificationList(int skip, int take);
}
