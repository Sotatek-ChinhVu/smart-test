using Domain.SuperAdminModels.Notification;
using Entity.SuperAdmin;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System.Linq;

namespace Infrastructure.SuperAdminRepositories;

public class NotificationRepository : SuperAdminRepositoryBase, INotificationRepository
{
    public NotificationRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<NotificationModel> GetNotificationList(int skip, int take)
    {
        var result = NoTrackingDataContext.Notifications.Where(item => item.IsDeleted == 0)
                                                        .OrderBy(item => item.IsRead)
                                                        .ThenByDescending(item => item.Id)
                                                        .Skip(skip)
                                                        .Take(take)
                                                        .Select(item => new NotificationModel(
                                                                            item.Id,
                                                                            item.Status,
                                                                            item.Message ?? string.Empty,
                                                                            item.IsDeleted == 1,
                                                                            item.IsRead == 1,
                                                                            item.CreateDate))
                                                        .ToList();
        return result;
    }

    public List<NotificationModel> UpdateNotificationList(List<NotificationModel> notificationList)
    {
        var notificationIdList = notificationList.Select(item => item.Id).Distinct().ToList();
        var notificationDBList = TrackingDataContext.Notifications.Where(item => notificationIdList.Contains(item.Id)
                                                                                 && item.IsDeleted == 0)
                                                                  .ToList();
        foreach (var model in notificationList)
        {
            var notification = notificationDBList.FirstOrDefault(item => item.Id == model.Id);
            if (notification == null)
            {
                continue;
            }
            notification.IsDeleted = model.IsDeleted ? 1 : 0;
            notification.IsRead = (byte)(model.IsRead ? 1 : 0);
            notification.UpdateDate = CIUtil.GetJapanDateTimeNow();
        }
        TrackingDataContext.SaveChanges();
        var result = notificationDBList.Select(notification => new NotificationModel(
                                                                   notification.Id,
                                                                   notification.Status,
                                                                   notification.Message ?? string.Empty,
                                                                   notification.IsDeleted == 1,
                                                                   notification.IsRead == 1,
                                                                   notification.CreateDate))
                                        .ToList();
        return result;
    }

    public bool CheckExistNotification(List<int> notificationIdList)
    {
        notificationIdList = notificationIdList.Distinct().ToList();
        return NoTrackingDataContext.Notifications.Count(item => notificationIdList.Contains(item.Id)) == notificationIdList.Count;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
