using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetSinDateRaiinInfList;

public class GetSinDateRaiinInfListOutputData : IOutputData
{
    public GetSinDateRaiinInfListOutputData(List<int> sinDateList, GetSinDateRaiinInfListStatus status)
    {
        SinDateList = sinDateList;
        Status = status;
    }

    public List<int> SinDateList { get; private set; }

    public GetSinDateRaiinInfListStatus Status { get; private set; }
}
