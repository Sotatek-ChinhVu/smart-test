using Domain.Models.Receipt;
using Helper.Messaging;
using UseCase.Core.Sync.Core;

namespace UseCase.ReceiptCheck.Recalculation
{
    public class ReceiptCheckRecalculationInputData : IInputData<ReceiptCheckRecalculationOutputData>
    {
        public ReceiptCheckRecalculationInputData(int hpId, int userId, List<long> ptIds, int seikyuYm, ReceStatusModel receStatus, IMessenger messenger)
        {
            HpId = hpId;
            UserId = userId;
            PtIds = ptIds;
            SeikyuYm = seikyuYm;
            ReceStatus = receStatus;
            Messenger = messenger;
        }

        public IMessenger Messenger { get; set; }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<long> PtIds { get; private set; }

        public int SeikyuYm { get; private set; }

        public ReceStatusModel ReceStatus { get; private set; }
    }
}