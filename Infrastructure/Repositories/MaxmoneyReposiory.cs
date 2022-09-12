using Domain.Models.MaxMoney;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class MaxmoneyReposiory : IMaxmoneyReposiory
    {
        private readonly TenantDataContext _tenantDataContext;

        public MaxmoneyReposiory(TenantDataContext tenantDataContext)
        {
            _tenantDataContext = tenantDataContext;
        }

        public List<MaxMoneyModel> GetListMaxMoney(long ptId, int hpId)
        {
            var maxMoneys = _tenantDataContext.LimitListInfs.Where(u => u.HpId == hpId
                                                                   && u.PtId == ptId)
                                                                   .OrderBy(u => u.SortKey)
                                                                   .ToList();
            return maxMoneys.Select(u => new MaxMoneyModel(u.HokenPid,u.SortKey,u.RaiinNo,u.FutanGaku,u.TotalGaku,u.Biko,u.IsDeleted)).ToList();
        }
    }
}
