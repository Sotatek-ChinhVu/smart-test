using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.CheckExistSyobyoKeika;

public class CheckExistSyobyoKeikaInputData : IInputData<CheckExistSyobyoKeikaOutputData>
{
    public CheckExistSyobyoKeikaInputData(int hpId, long ptId, int sinYm, int hokenId, int sinDay)
    {
        HpId = hpId;
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        SinDay = sinDay;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int SinDay { get; private set; }

}
