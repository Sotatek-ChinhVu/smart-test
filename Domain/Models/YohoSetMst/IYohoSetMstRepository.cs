using Domain.Models.OrdInfDetails;

namespace Domain.Models.YohoSetMst
{
    public interface IYohoSetMstRepository
    {
        IEnumerable<YohoSetMstModel> GetByItemCd(int hpId, string itemcd, int startDate);
    }
}
