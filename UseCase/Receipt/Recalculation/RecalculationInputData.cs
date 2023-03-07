using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.Recalculation;

public class RecalculationInputData : IInputData<RecalculationOutputData>
{
    public RecalculationInputData(int hpId, int userId, int sinYm, List<long> ptIdList, bool isStopCalc)
    {
        HpId = hpId;
        UserId = userId;
        SinYm = sinYm;
        PtIdList = ptIdList;
        IsStopCalc = isStopCalc;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int SinYm { get; private set; }

    public List<long> PtIdList { get; private set; }

    public bool IsStopCalc { get; private set; }
}
