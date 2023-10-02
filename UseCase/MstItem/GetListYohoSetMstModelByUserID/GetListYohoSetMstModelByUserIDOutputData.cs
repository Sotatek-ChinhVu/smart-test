using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.GetListYohoSetMstModelByUserID
{
    public class GetListYohoSetMstModelByUserIDOutputData : IOutputData
    {
        public GetListYohoSetMstModelByUserIDOutputData(List<YohoSetMstModel> yohoSetMsts, GetListYohoSetMstModelByUserIDStatus status)
        {
            YohoSetMsts = yohoSetMsts;
            Status = status;
        }

        public List<YohoSetMstModel> YohoSetMsts { get; private set; }
        public GetListYohoSetMstModelByUserIDStatus Status { get; private set; }
    }
}
