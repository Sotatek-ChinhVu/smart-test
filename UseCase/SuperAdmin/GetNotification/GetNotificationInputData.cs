using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.GetNotification;

public class GetNotificationInputData : IInputData<GetNotificationOutputData>
{
    public GetNotificationInputData(int skip, int take, bool onlyUnreadNotifications)
    {
        Skip = skip;
        Take = take;
        OnlyUnreadNotifications = onlyUnreadNotifications;
    }

    public int Skip { get; private set; }

    public int Take { get; private set; }

    public bool OnlyUnreadNotifications { get; private set; }
}
