using Domain.SuperAdminModels.Notification;
using UseCase.SuperAdmin.UpdateNotification;

namespace Interactor.SuperAdmin;

public class UpdateNotificationInteractor : IUpdateNotificationInputPort
{
    private readonly INotificationRepository _notificationRepository;

    public UpdateNotificationInteractor(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public UpdateNotificationOutputData Handle(UpdateNotificationInputData inputData)
    {
        try
        {
            if (!_notificationRepository.CheckExistNotification(inputData.NotificationList.Select(item => item.Id).Distinct().ToList()))
            {
                return new UpdateNotificationOutputData(UpdateNotificationStatus.InvalidIdNotification);
            }
            else if (_notificationRepository.UpdateNotificationList(inputData.NotificationList))
            {
                return new UpdateNotificationOutputData(UpdateNotificationStatus.Successed);
            }
            return new UpdateNotificationOutputData(UpdateNotificationStatus.Failed);
        }
        finally
        {
            _notificationRepository.ReleaseResource();
        }
    }
}
