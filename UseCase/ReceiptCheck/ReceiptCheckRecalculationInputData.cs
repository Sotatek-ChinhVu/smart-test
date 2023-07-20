using UseCase.Core.Sync.Core;

namespace UseCase.ReceiptCheck
{
    public class ReceiptCheckRecalculationInputData : IInputData<ReceiptCheckRecalculationOutputData>
    {
        public ReceiptCheckRecalculationInputData(int hpId, int userId, List<long> ptIds, int seikyuYm)
        {
            HpId = hpId;
            UserId = userId;
            PtIds = ptIds;
            SeikyuYm = seikyuYm;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<long> PtIds { get; private set; }

        public int SeikyuYm { get; private set; }
    }
}