using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.AddYousiki;

public class AddYousikiInputData : IInputData<AddYousikiOutputData>
{
    public AddYousikiInputData(int hpId, int userId, int sinYm, long ptNum, int selectDataType, ReactAddYousiki reactAddYousiki)
    {
        HpId = hpId;
        UserId = userId;
        SinYm = sinYm;
        PtNum = ptNum;
        SelectDataType = selectDataType;
        ReactAddYousiki = reactAddYousiki;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int SinYm { get; private set; }

    public long PtNum { get; private set; }

    public int SelectDataType { get; private set; }

    public ReactAddYousiki ReactAddYousiki { get; private set; }
}
