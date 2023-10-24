using Domain.Models.KensaInfDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.UpdateKensaInfDetail
{
    public class UpdateKensaInfDetailInputData : IInputData<UpdateKensaInfDetailOutputData>
    {
        public UpdateKensaInfDetailInputData(int hpId, int userId, int ptId, int iraiCd, int iraiDate, List<KensaInfDetailUpdateModel> kensaInfDetails)
        {
            HpId = hpId;
            UserId = userId;
            PtId = ptId;
            IraiCd = iraiCd;
            IraiDate = iraiDate;
            KensaInfDetails = kensaInfDetails;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public int PtId { get; private set; }

        public int IraiCd { get; private set; }

        public int IraiDate { get; private set; }

        public List<KensaInfDetailUpdateModel> KensaInfDetails { get; private set; }
    }
}
