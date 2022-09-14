using Domain.Models.MaxMoney;
using Entity.Tenant;
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

        public List<LimitListModel> GetListLimitModel(long ptId, int hpId)
        {
            IEnumerable<LimitListInf> maxMoneys = _tenantDataContext.LimitListInfs.Where(u => u.HpId == hpId
                                                                   && u.PtId == ptId)
                                                                   .OrderBy(u => u.SortKey)
                                                                   .ToList();
            return maxMoneys.Select(u => new LimitListModel(u.Id,u.KohiId,u.SinDate,u.HokenPid,u.SortKey,u.RaiinNo,u.FutanGaku,u.TotalGaku,u.Biko ?? string.Empty,u.IsDeleted)).ToList();
        }

        public MaxMoneyInfoHokenModel GetInfoHokenMoney(int hpId,long ptId,int kohiId,int sinYm)
        {
            var kohi = _tenantDataContext.PtKohis.FirstOrDefault(x => x.HpId == hpId
                                                                && x.PtId == ptId
                                                                && x.HokenId == kohiId);

            if (kohi is null) return new MaxMoneyInfoHokenModel(0,0,0,0,0,0,string.Empty, string.Empty,0,0,0,0);

            var hokenMst = _tenantDataContext.HokenMsts.FirstOrDefault(x => x.HpId == hpId
                                                                && x.HokenNo == kohi.HokenNo
                                                                && x.HokenEdaNo == kohi.HokenEdaNo);

            if (hokenMst is null) return new MaxMoneyInfoHokenModel(0, 0, 0, 0, 0, 0, string.Empty, string.Empty, 0, 0, 0, 0);

            int limitFutan = 0;
            if (hokenMst.KaiLimitFutan > 0)
                limitFutan = hokenMst.KaiLimitFutan;
            else if (hokenMst.DayLimitFutan > 0)
                limitFutan = hokenMst.DayLimitFutan;
            else if (hokenMst.MonthLimitFutan > 0)
                limitFutan = hokenMst.MonthLimitFutan;

            return new MaxMoneyInfoHokenModel(kohi.HokenId, 
                                                kohi.Rate, 
                                                sinYm, 
                                                hokenMst.FutanKbn, 
                                                hokenMst.MonthLimitFutan, 
                                                kohi.GendoGaku, 
                                                hokenMst.Houbetu, 
                                                hokenMst.HokenName, 
                                                hokenMst.IsLimitListSum,
                                                hokenMst.IsLimitList,
                                                hokenMst.FutanRate, 
                                                limitFutan);
        }
    }
}
