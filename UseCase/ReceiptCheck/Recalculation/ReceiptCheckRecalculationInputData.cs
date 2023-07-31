using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceiptCheck.Recalculation
{
    public class ReceiptCheckRecalculationInputData : IInputData<ReceiptCheckRecalculationOutputData>
    {
        public ReceiptCheckRecalculationInputData(int hpId, int userId, List<long> ptIds, int seikyuYm, ReceStatusModel receStatus)
        {
            HpId = hpId;
            UserId = userId;
            PtIds = ptIds;
            SeikyuYm = seikyuYm;
            ReceStatus = receStatus;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<long> PtIds { get; private set; }

        public int SeikyuYm { get; private set; }

        public ReceStatusModel ReceStatus { get; private set; }
    }
}