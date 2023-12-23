namespace SuperAdminAPI.Reponse.Notification;

public class GetNotificationResponse
{
    public GetNotificationResponse(int totalItem, List<NotificationDto> notificationList)
    {
        TotalItem = totalItem;
        NotificationList = notificationList;
    }

    public int TotalItem { get; private set; }

    public List<NotificationDto> NotificationList { get; private set; }
}
