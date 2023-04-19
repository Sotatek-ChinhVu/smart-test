using UseCase.Core.Sync.Core;

namespace UseCase.User.GetUserConfModelList;

public class GetUserConfModelListInputData : IInputData<GetUserConfModelListOutputData>
{
    public GetUserConfModelListInputData(int hpId, int userId)
    {
        HpId = hpId;
        UserId = userId;
    }

    public int HpId { get; private set; }
    public int UserId { get; private set; }
}
