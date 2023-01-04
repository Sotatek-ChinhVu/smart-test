using UseCase.Core.Sync.Core;

namespace UseCase.User.Sagaku;

public class SagakuInputData : IInputData<SagakuOutputData>
{
    public SagakuInputData(int hpId, int userId, bool fromRece)
    {
        HpId = hpId;
        UserId = userId;
        FromRece = fromRece;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
    public bool FromRece { get; private set; }
}
