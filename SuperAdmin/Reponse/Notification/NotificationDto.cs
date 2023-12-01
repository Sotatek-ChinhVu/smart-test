using Domain.SuperAdminModels.Notification;

namespace SuperAdminAPI.Reponse.Notification;

public class NotificationDto
{
    public NotificationDto(NotificationModel model)
    {
        Id = model.Id;
        Status = model.Status;
        Message = model.Message;
        IsDeleted = model.IsDeleted;
        IsRead = model.IsRead;
        CreateDate = model.CreateDate;
    }

    public int Id { get; private set; }

    public byte Status { get; private set; }

    public string Message { get; private set; }

    public bool IsDeleted { get; private set; }

    public bool IsRead { get; private set; }

    public DateTime CreateDate { get; private set; }
}
