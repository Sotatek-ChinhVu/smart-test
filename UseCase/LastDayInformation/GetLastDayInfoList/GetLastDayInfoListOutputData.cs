using Domain.Models.TodayOdr;
using UseCase.Core.Sync.Core;

namespace UseCase.LastDayInformation.GetLastDayInfoList;

public class GetLastDayInfoListOutputData : IOutputData
{
    public GetLastDayInfoListOutputData(List<OdrDateInfModel> odrDateInfList, GetLastDayInfoListStatus status)
    {
        OdrDateInfList = odrDateInfList;
        Status = status;
    }

    public List<OdrDateInfModel> OdrDateInfList { get; private set; }

    public GetLastDayInfoListStatus Status { get; private set; }
}
