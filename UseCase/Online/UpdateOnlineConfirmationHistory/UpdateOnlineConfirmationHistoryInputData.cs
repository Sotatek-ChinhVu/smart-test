using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOnlineConfirmationHistory;

public class UpdateOnlineConfirmationHistoryInputData : IInputData<UpdateOnlineConfirmationHistoryOutputData>
{
    public UpdateOnlineConfirmationHistoryInputData(int id, int userId, bool isDeleted, bool isUpdated)
    {
        Id = id;
        UserId = userId;
        IsDeleted = isDeleted;
        IsUpdated = isUpdated;
    }

    public int Id { get; private set; }

    public int UserId { get; private set; }

    public bool IsDeleted { get; private set; }

    public bool IsUpdated { get; private set; }
}
