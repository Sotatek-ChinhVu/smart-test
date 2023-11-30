namespace SuperAdminAPI.Request.Notification;

public class UpdateNotificationRequestItem
{
    public int Id { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsRead { get; set; }
}
