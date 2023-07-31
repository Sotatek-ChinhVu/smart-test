using UseCase.Core.Sync.Core;

namespace UseCase.ReceSeikyu.CancelSeikyu;

public class CancelSeikyuInputData : IInputData<CancelSeikyuOutputData>
{
    public CancelSeikyuInputData(int hpId, int seikyuYm, int seikyuKbn, long ptId, int sinYm, int hokenId, int userId)
    {
        HpId = hpId;
        SeikyuYm = seikyuYm;
        SeikyuKbn = seikyuKbn;
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        UserId = userId;
    }

    public int HpId { get; private set; }

    public int SeikyuYm { get; private set; }

    public int SeikyuKbn { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int UserId { get; private set; }
}
