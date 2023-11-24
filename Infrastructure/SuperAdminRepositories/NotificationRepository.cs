using Domain.SuperAdminModels.Tenant;
using Entity.SuperAdmin;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.SuperAdminRepositories
{
    public class NotificationRepository : SuperAdminRepositoryBase, INotificationRepository
    {
        public NotificationRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public bool CreateNotification(byte status, string messenge)
        {
            try
            {
                var notification = new Notification();
                notification.Status = status;
                notification.Message = messenge;
                notification.CreateDate = CIUtil.GetJapanDateTimeNow();
                notification.UpdateDate = CIUtil.GetJapanDateTimeNow();
                TrackingDataContext.Notifications.Add(notification);
                TrackingDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
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


        public void ReleaseResource()
        {
            throw new NotImplementedException();
        }
    }
}
