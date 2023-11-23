using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.UpdateNotification;

public class UpdateNotificationOutputData : IOutputData
{
    public UpdateNotificationOutputData(UpdateNotificationStatus status)
    {
        Status = status;
    }

    public UpdateNotificationStatus Status { get; private set; }
}
