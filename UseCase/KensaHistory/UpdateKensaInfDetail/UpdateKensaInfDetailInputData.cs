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
            this.kensaInfDetails = kensaInfDetails;
        }

        public int HpId { get; set; }
        public int UserId { get; set; }
        public List<KensaInfDetailUpdateModel> kensaInfDetails { get; set; }
    }
}
