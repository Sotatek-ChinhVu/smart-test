using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListReceCmt;

public class SaveReceCmtListInputData : IInputData<SaveReceCmtListOutputData>
{
    public SaveReceCmtListInputData(int hpId, int userId, long ptId, int sinYm, int hokenId, List<ReceCmtItem> receCmtList)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        ReceCmtList = receCmtList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public List<ReceCmtItem> ReceCmtList { get; private set; }
}
