using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetRenkeiTiming;

public class GetRenkeiTimingOutputData : IOutputData
{
    public GetRenkeiTimingOutputData(GetRenkeiTimingStatus status, List<RenkeiTimingModel> renkeiTimingList)
    {
        Status = status;
        RenkeiTimingList = renkeiTimingList;
    }

    public GetRenkeiTimingStatus Status { get; private set; }

    public List<RenkeiTimingModel> RenkeiTimingList { get; private set; }
}
