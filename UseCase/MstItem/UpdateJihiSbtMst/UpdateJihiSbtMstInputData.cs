using Domain.Models.MstItem;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateJihiSbtMst
{
    public sealed class UpdateJihiSbtMstInputData : IInputData<UpdateJihiSbtMstOutputData>
    {
        public UpdateJihiSbtMstInputData(int hpId, int userId, List<JihiSbtMstModel> jihiSbtMsts)
        {
            HpId = hpId;
            UserId = userId;
            JihiSbtMsts = jihiSbtMsts;
        }

        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public List<JihiSbtMstModel> JihiSbtMsts { get; private set; }
    }
}
