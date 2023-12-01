namespace SuperAdminAPI.Reponse.Notification;

public class GetNotificationResponse
{
    public GetNotificationResponse(List<NotificationDto> notificationList)
    {
        NotificationList = notificationList;
    }

    public List<NotificationDto> NotificationList { get; private set; }
}
