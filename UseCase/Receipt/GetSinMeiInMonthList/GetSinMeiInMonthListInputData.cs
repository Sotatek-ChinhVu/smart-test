using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetSinMeiInMonthList;

public class GetSinMeiInMonthListInputData : IInputData<GetSinMeiInMonthListOutputData>
{
    public GetSinMeiInMonthListInputData(int hpId, long ptId, int sinYm, int hokenId, int seikyuYm)
    {
        HpId = hpId;
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        SeikyuYm = seikyuYm;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int SeikyuYm { get; private set; }
}
