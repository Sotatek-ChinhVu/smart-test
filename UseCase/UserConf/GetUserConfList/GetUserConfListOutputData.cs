using UseCase.Core.Sync.Core;

namespace UseCase.User.GetUserConfList;

public class GetUserConfListOutputData : IOutputData
{
    public GetUserConfListOutputData(GetUserConfListStatus status, Dictionary<string, int> values)
    {
        Status = status;
        UserConfs = values;
    }

    public GetUserConfListStatus Status { get; private set; }
    public Dictionary<string, int> UserConfs { get; private set; }
}
