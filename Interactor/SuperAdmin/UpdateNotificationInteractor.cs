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
            List<NotificationModel> notificationResult;

            // if condition is realAllNotifications
            if (inputData.IsRealAllNotifications)
            {
                notificationResult = _notificationRepository.ReadAllNotification();
                return new UpdateNotificationOutputData(notificationResult, UpdateNotificationStatus.Successed);
            }

            if (!_notificationRepository.CheckExistNotification(inputData.NotificationList.Select(item => item.Id).Distinct().ToList()))
            {
                return new UpdateNotificationOutputData(UpdateNotificationStatus.InvalidIdNotification);
            }
            notificationResult = _notificationRepository.UpdateNotificationList(inputData.NotificationList);
            if (notificationResult.Any())
            {
                return new UpdateNotificationOutputData(notificationResult, UpdateNotificationStatus.Successed);
            }
            return new UpdateNotificationOutputData(UpdateNotificationStatus.Failed);
        }
        finally
        {
            _notificationRepository.ReleaseResource();
        }
    }
}
