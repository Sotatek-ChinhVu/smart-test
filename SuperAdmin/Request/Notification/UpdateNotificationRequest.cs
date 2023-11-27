namespace SuperAdminAPI.Request.Notification;

public class UpdateNotificationRequest
{
    public List<UpdateNotificationRequestItem> NotificationList { get; set; } = new();
}
