using Domain.Common;
using Domain.Models.OrdInfDetails;

namespace Domain.Models.YohoSetMst
{
    public interface IYohoSetMstRepository : IRepositoryBase
    {
        IEnumerable<YohoSetMstModel> GetByItemCd(int hpId, string itemcd, int startDate);
    }
}
