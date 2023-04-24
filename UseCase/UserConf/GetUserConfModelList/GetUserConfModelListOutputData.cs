using Domain.Models.UserConf;
using UseCase.Core.Sync.Core;

namespace UseCase.User.GetUserConfModelList;

public class GetUserConfModelListOutputData : IOutputData
{
    public GetUserConfModelListOutputData(GetUserConfModelListStatus status, List<UserConfModel> userConfs)
    {
        Status = status;
        UserConfs = userConfs;
    }

    public GetUserConfModelListStatus Status { get; private set; }
    public List<UserConfModel> UserConfs { get; private set; }
}
