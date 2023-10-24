using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateKensaStdMst
{
    public class UpdateKensaStdMstInputData : IInputData<UpdateKensaStdMstOutputData>
    {
        public UpdateKensaStdMstInputData(int hpId, int userId, List<KensaStdMstModel> kensaStdMsts)
        {
            HpId = hpId;
            UserId = userId;
            KensaStdMsts = kensaStdMsts;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<KensaStdMstModel> KensaStdMsts { get; private set; }
    }
}
