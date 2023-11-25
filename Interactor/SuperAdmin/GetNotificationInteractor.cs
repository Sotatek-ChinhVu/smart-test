using Domain.SuperAdminModels.Notification;
using UseCase.SuperAdmin.GetNotification;

namespace Interactor.SuperAdmin;

public class GetNotificationInteractor : IGetNotificationInputPort
{
    private readonly INotificationRepository _notificationRepository;

    public GetNotificationInteractor(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public GetNotificationOutputData Handle(GetNotificationInputData inputData)
    {
        try
        {
            var result = _notificationRepository.GetNotificationList(inputData.Skip, inputData.Take);
            return new GetNotificationOutputData(result, GetNotificationStatus.Successed);
        }
        finally
        {
            _notificationRepository.ReleaseResource();
        }
    }
}
