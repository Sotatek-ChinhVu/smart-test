using UseCase.Core.Sync.Core;

namespace UseCase.User.GetUserConfList;

public class GetUserConfListInputData : IInputData<GetUserConfListOutputData>
{
    public GetUserConfListInputData(int hpId, int userId)
    {
        HpId = hpId;
        UserId = userId;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
}
