using UseCase.Core.Sync.Core;

namespace UseCase.Online.UpdateOnlineHistoryById;

public class UpdateOnlineHistoryByIdInputData : IInputData<UpdateOnlineHistoryByIdOutputData>
{
    public UpdateOnlineHistoryByIdInputData(int userId, long id, long ptId, int uketukeStatus, int confirmationType)
    {
        UserId = userId;
        Id = id;
        PtId = ptId;
        UketukeStatus = uketukeStatus;
        ConfirmationType = confirmationType;
    }

    public int UserId { get; private set; }

    public long Id { get; private set; }

    public long PtId { get; private set; }

    public int UketukeStatus { get; private set; }

    public int ConfirmationType { get; private set; }
}
