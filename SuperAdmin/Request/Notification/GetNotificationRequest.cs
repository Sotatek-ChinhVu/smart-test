namespace SuperAdminAPI.Request.Notification;

public class GetNotificationRequest
{
    public int Skip { get; set; }

    public int Take { get; set; }

    public bool OnlyUnreadNotifications { get; set; } = false;
}
