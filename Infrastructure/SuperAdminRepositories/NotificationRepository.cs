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

        public void ReleaseResource()
        {
            throw new NotImplementedException();
        }
    }
}
