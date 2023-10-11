using Domain.Models.KensaInfDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.KensaHistory.UpdateKensaInfDetail
{
    public class UpdateKensaInfDetailInputData : IInputData<UpdateKensaInfDetailOutputData>
    {
        public UpdateKensaInfDetailInputData(int hpId, int userId, List<KensaInfDetailUpdateModel> kensaInfDetails)
        {
            HpId = hpId;
            UserId = userId;
            KensaInfDetails = kensaInfDetails;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public List<KensaInfDetailUpdateModel> KensaInfDetails { get; private set; }
    }
}
