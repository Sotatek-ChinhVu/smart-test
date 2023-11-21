using Domain.Models.KensaIrai;
using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.UpdateKensaMst
{
    public class UpdateKensaMstInputData : IInputData<UpdateKensaMstOutputData>
    {
        public UpdateKensaMstInputData(int hpId, int userId, List<KensaMstModel> kensaMsts, List<KensaMstModel> childKensaMsts, List<TenItemModel> tenMsts, List<TenItemModel> tenMstListGenDate)
        {
            HpId = hpId;
            UserId = userId;
            KensaMsts = kensaMsts;
            TenMsts = tenMsts;
            ChildKensaMsts = childKensaMsts;
            TenMstListGenDate = tenMstListGenDate;
        }

        public int HpId { get; private set; }

        public int UserId { get; private set; }

        public List<KensaMstModel> KensaMsts { get; private set; }

        public List<KensaMstModel> ChildKensaMsts { get; private set; }

        public List<TenItemModel> TenMsts { get; private set; }

        public List<TenItemModel> TenMstListGenDate { get; private set; }
    }
}
