using Domain.Models.OrdInfDetails;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateYohoSetMst
{
    public sealed class UpdateYohoSetMstInputData : IInputData<UpdateYohoSetMstOutputData>
    {
        public UpdateYohoSetMstInputData(int hpId, int userId, List<YohoSetMstModel> yohoSetMsts)
        {
            HpId = hpId;
            UserId = userId;
            YohoSetMsts = yohoSetMsts;
        }
        public int HpId { get; private set; }
        public int UserId { get; private set; }
        public List<YohoSetMstModel> YohoSetMsts { get; private set; }
    }
}
