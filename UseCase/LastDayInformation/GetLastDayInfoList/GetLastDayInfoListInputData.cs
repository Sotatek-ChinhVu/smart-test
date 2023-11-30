using UseCase.Core.Sync.Core;

namespace UseCase.LastDayInformation.GetLastDayInfoList;

public class GetLastDayInfoListInputData : IInputData<GetLastDayInfoListOutputData>
{
    public GetLastDayInfoListInputData(int hpId, long ptId, int sinDate)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }
}
