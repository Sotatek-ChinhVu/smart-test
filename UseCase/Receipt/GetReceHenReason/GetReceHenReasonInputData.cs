using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceHenReason;

public class GetReceHenReasonInputData : IInputData<GetReceHenReasonOutputData>
{
    public GetReceHenReasonInputData(int hpId, int seikyuYm, int sinDate, long ptId, int hokenId)
    {
        HpId = hpId;
        SeikyuYm = seikyuYm;
        SinDate = sinDate;
        PtId = ptId;
        HokenId = hokenId;
    }

    public int HpId { get; private set; }

    public int SeikyuYm { get; private set; }

    public int SinDate { get; private set; }

    public long PtId { get; private set; }

    public int HokenId { get; private set; }
}
