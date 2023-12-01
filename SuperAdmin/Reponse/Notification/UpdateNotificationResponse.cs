namespace SuperAdminAPI.Reponse.Notification;

public class UpdateNotificationResponse
{
    public UpdateNotificationResponse(bool successed)
    {
        Successed = successed;
    }

    public bool Successed { get; private set; }
}
