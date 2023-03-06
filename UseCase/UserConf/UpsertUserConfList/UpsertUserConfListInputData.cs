using Domain.Models.UserConf;
using UseCase.Core.Sync.Core;

namespace UseCase.User.UpsertUserConfList;

public class UpsertUserConfListInputData : IInputData<UpsertUserConfListOutputData>
{
    public UpsertUserConfListInputData(int userId, int hpId, List<UserConfModel> userConfs)
    {
        UserId = userId;
        HpId = hpId;
        UserConfs = userConfs;
    }

    public int UserId { get; private set; }

    public int HpId { get; private set; }

    public List<UserConfModel> UserConfs { get; private set; }
}
