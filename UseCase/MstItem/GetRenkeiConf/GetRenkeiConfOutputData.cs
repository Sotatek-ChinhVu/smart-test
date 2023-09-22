using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetRenkeiConf;

public class GetRenkeiConfOutputData : IOutputData
{
    public GetRenkeiConfOutputData(List<RenkeiConfModel> renkeiConfList, GetRenkeiConfStatus status)
    {
        RenkeiConfList = renkeiConfList;
        Status = status;
    }

    public List<RenkeiConfModel> RenkeiConfList { get; private set; }

    public GetRenkeiConfStatus Status { get; private set; }
}
