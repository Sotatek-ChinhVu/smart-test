using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.AddYousiki;

public class AddYousikiInputData : IInputData<AddYousikiOutputData>
{
    public AddYousikiInputData(int hpId, int sinYm, long ptNum, int selectDataType)
    {
        HpId = hpId;
        SinYm = sinYm;
        PtNum = ptNum;
        SelectDataType = selectDataType;
    }

    public int HpId { get; private set; }

    public int SinYm { get; private set; }

    public long PtNum { get; private set; }

    public int SelectDataType { get; private set; }
}
