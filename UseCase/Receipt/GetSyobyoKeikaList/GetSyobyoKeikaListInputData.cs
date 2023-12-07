using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetListSyobyoKeika;

public class GetSyobyoKeikaListInputData : IInputData<GetSyobyoKeikaListOutputData>
{
    public GetSyobyoKeikaListInputData(int hpId, int sinYm, long ptId, int hokenId, int hokenKbn)
    {
        HpId = hpId;
        SinYm = sinYm;
        PtId = ptId;
        HokenId = hokenId;
        HokenKbn = hokenKbn;
    }

    public int HpId { get; private set; }

    public int SinYm { get; private set; }

    public long PtId { get; private set; }

    public int HokenId { get; private set; }

    public int HokenKbn { get; private set; }
}
