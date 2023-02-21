using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.Recalculation;

public class RecalculationInputData : IInputData<RecalculationOutputData>
{
    public RecalculationInputData(int hpId, int sinYm, List<long> ptIdList)
    {
        HpId = hpId;
        SinYm = sinYm;
        PtIdList = ptIdList;
    }

    public int HpId { get; private set; }

    public int SinYm { get; private set; }

    public List<long> PtIdList { get; private set; }
}
