using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetSinDateRaiinInfList;

public class GetSinDateRaiinInfListInputData : IInputData<GetSinDateRaiinInfListOutputData>
{
    public GetSinDateRaiinInfListInputData(int hpId, long ptId, int sinYm, int hokenId)
    {
        HpId = hpId;
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }
}
