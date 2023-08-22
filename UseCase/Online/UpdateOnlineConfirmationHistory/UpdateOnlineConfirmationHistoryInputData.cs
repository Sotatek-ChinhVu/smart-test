using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOnlineConfirmationHistory;

public class UpdateOnlineConfirmationHistoryInputData : IInputData<UpdateOnlineConfirmationHistoryOutputData>
{
    public UpdateOnlineConfirmationHistoryInputData(int id, int userId, bool isDeleted)
    {
        Id = id;
        UserId = userId;
        IsDeleted = isDeleted;
    }

    public int Id { get; private set; }

    public int UserId { get; private set; }

    public bool IsDeleted { get; private set; }
}
