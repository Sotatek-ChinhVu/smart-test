using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveReceCheckCmtList;

public class SaveReceCheckCmtListInputData : IInputData<SaveReceCheckCmtListOutputData>
{
    public SaveReceCheckCmtListInputData(int hpId, int userId, long ptId, int sinYm, int hokenId, List<ReceCheckCmtItem> receCheckCmtList, List<ReceCheckErrorItem> receCheckErrorList)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        ReceCheckCmtList = receCheckCmtList;
        ReceCheckErrorList = receCheckErrorList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public List<ReceCheckCmtItem> ReceCheckCmtList { get; private set; }

    public List<ReceCheckErrorItem> ReceCheckErrorList { get; private set; }
}
