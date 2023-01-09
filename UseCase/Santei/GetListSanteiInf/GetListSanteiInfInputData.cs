using UseCase.Core.Sync.Core;

namespace UseCase.Santei.GetListSanteiInf;

public class GetListSanteiInfInputData : IInputData<GetListSanteiInfOutputData>
{
    public GetListSanteiInfInputData(int hpId, long ptId, int sinDate, int hokenPid)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        HokenPid = hokenPid;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public int HokenPid { get; private set; }
}
