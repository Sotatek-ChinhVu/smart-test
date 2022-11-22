using Domain.Models.OrdInfDetails;
using UseCase.Core.Sync.Core;

namespace UseCase.YohoSetMst.GetByItemCd
{
    public class GetYohoMstByItemCdOutputData : IOutputData
    {
        public List<YohoSetMstModel> YohoSetMsts { get; private set; }

        public GetYohoMstByItemCdStatus Status { get; private set; }

        public GetYohoMstByItemCdOutputData(List<YohoSetMstModel> yohoSetMsts, GetYohoMstByItemCdStatus status)
        {
            YohoSetMsts = yohoSetMsts;
            Status = status;
        }
    }
}
