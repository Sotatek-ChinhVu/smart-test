namespace SuperAdminAPI.Request.Notification;

public class UpdateNotificationRequest
{
    public List<UpdateNotificationRequestItem> NotificationList { get; set; } = new();

    public bool IsRealAllNotifications { get; set; } = false;
}
