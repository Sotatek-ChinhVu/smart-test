using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveReceStatus;

public class SaveReceStatusInputData : IInputData<SaveReceStatusOutputData>
{
    public SaveReceStatusInputData(int hpId, int userId, ReceStatusItem receStatus)
    {
        HpId = hpId;
        UserId = userId;
        ReceStatus = receStatus;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public ReceStatusItem ReceStatus { get; private set; }
}
