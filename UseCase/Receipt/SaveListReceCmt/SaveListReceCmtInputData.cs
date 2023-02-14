using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListReceCmt;

public class SaveListReceCmtInputData : IInputData<SaveListReceCmtOutputData>
{
    public SaveListReceCmtInputData(int hpId, int userId, long ptId, int sinYm, int hokenId, List<ReceCmtItem> listReceCmt)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        ListReceCmt = listReceCmt;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public List<ReceCmtItem> ListReceCmt { get; private set; }
}
