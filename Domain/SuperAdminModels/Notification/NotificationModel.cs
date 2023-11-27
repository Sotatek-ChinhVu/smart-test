namespace Domain.SuperAdminModels.Notification;

public class NotificationModel
{
    public NotificationModel(int id, byte status, string message, bool isDeleted, bool isRead, DateTime createDate)
    {
        Id = id;
        Status = status;
        Message = message;
        IsDeleted = isDeleted;
        IsRead = isRead;
        CreateDate = createDate;
    }

    public NotificationModel(int id, bool isDeleted, bool isRead)
    {
        Id = id;
        IsDeleted = isDeleted;
        IsRead = isRead;
        Message = string.Empty;
    }

    public int Id { get; private set; }

    public byte Status { get; private set; }

    public string Message { get; private set; }

    public bool IsDeleted { get; private set; }

    public bool IsRead { get; private set; }

    public DateTime CreateDate { get; private set; }
}
