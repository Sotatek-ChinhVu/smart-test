using UseCase.Core.Sync.Core;

namespace UseCase.ReceiptCheck
{
    public class RecalculationInputData : IInputData<RecalculationOutputData>
    {
        public int HpId { get; private set; }
        public List<long> PtIds { get; private set; }
        public int SeikyuYm { get; private set; }

        public RecalculationInputData(int hpId, List<long> ptIds, int seikyuYm)
        {
            HpId = hpId;
            PtIds = ptIds;
            SeikyuYm = seikyuYm;
        }
    }
}