using UseCase.Core.Sync.Core;

namespace UseCase.SuperAdmin.GetNotification;

public class GetNotificationInputData : IInputData<GetNotificationOutputData>
{
    public GetNotificationInputData(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }

    public int Skip { get; private set; }

    public int Take { get; private set; }
}
