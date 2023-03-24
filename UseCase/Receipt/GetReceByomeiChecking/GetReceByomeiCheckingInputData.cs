using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetReceByomeiChecking;

public class GetReceByomeiCheckingInputData : IInputData<GetReceByomeiCheckingOutputData>
{
    public GetReceByomeiCheckingInputData(int hpId, long ptId, int sinDate, int hokenId)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        HokenId = hokenId;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public int HokenId { get; private set; }
}
