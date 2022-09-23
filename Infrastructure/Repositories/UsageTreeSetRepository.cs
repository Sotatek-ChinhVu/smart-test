using Domain.Models.UsageTreeSet;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Data;

namespace Infrastructure.Repositories
{
    public class UsageTreeSetRepository : IUsageTreeSetRepository
    {
        private readonly TenantDataContext _tenantDataContext;

        public UsageTreeSetRepository(ITenantProvider _tenantProvider)
        {
            _tenantDataContext = _tenantProvider.GetTrackingTenantDataContext();
        }

        public int GetGenerationId(int sinDate)
        {
            ListSetGenerationMst? generation = _tenantDataContext.ListSetGenerationMsts.Where
                            (item => item.StartDate < sinDate)
                            .OrderByDescending(item => item.StartDate)
                            .FirstOrDefault();

            return generation?.GenerationId ?? 0;
        }

        public List<ListSetMstModel> GetTanSetInfs(int hpId, int setUsageKbn, int generationId, int sinDate)
        {
            var list = _tenantDataContext.ListSetMsts.Where(x => x.HpId == hpId
                                                        && x.GenerationId == generationId
                                                        && x.IsDeleted == 0
                                                        && x.SetKbn == setUsageKbn);

            var ItemCds = list.Select(x => x.ItemCd);
            var tenMsts = _tenantDataContext.TenMsts.Where(item => ItemCds.Contains(item.ItemCd)
                                        && item.StartDate <= sinDate
                                        && item.EndDate >= sinDate).Select(x => new
                                        {
                                            x.ItemCd,
                                            x.OdrUnitName,
                                            x.SinKouiKbn
                                        });

            return (from item in list
                    join tenMst in tenMsts on item.ItemCd equals tenMst.ItemCd into gj
                    from subpet in gj.DefaultIfEmpty()
                    select new ListSetMstModel(item.HpId,
                                               item.GenerationId,
                                               item.SetId,
                                               item.SetName ?? string.Empty,
                                               item.ItemCd ?? string.Empty,
                                               item.IsTitle,
                                               item.SetKbn,
                                               item.SelectType,
                                               item.Suryo,
                                               item.Level1,
                                               item.Level2,
                                               item.Level3,
                                               item.Level4,
                                               item.Level5,
                                               item.CmtName ?? string.Empty,
                                               item.CmtOpt ?? string.Empty,
                                               subpet.OdrUnitName ?? string.Empty,
                                               (int?)subpet.SinKouiKbn ?? 0)).ToList();
        }

        public List<ListSetMstModel> GetTanSetInfs(int hpId, IEnumerable<int> usageContains, int generationId, int sinDate)
        {
            var list = _tenantDataContext.ListSetMsts.Where(x => x.HpId == hpId
                                                        && x.GenerationId == generationId
                                                        && x.IsDeleted == 0
                                                        && usageContains.Contains(x.SetKbn));

            var ItemCds = list.Select(x => x.ItemCd);
            var tenMsts = _tenantDataContext.TenMsts.Where(item => ItemCds.Contains(item.ItemCd)
                                        && item.StartDate <= sinDate
                                        && item.EndDate >= sinDate).Select(x => new
                                        {
                                            x.ItemCd,
                                            x.OdrUnitName,
                                            x.SinKouiKbn
                                        });

            return (from item in list
                    join tenMst in tenMsts on item.ItemCd equals tenMst.ItemCd into gj
                    from subpet in gj.DefaultIfEmpty()
                    select new ListSetMstModel(item.HpId,
                                               item.GenerationId,
                                               item.SetId,
                                               item.SetName ?? string.Empty,
                                               item.ItemCd ?? string.Empty,
                                               item.IsTitle,
                                               item.SetKbn,
                                               item.SelectType,
                                               item.Suryo,
                                               item.Level1,
                                               item.Level2,
                                               item.Level3,
                                               item.Level4,
                                               item.Level5,
                                               item.CmtName ?? string.Empty,
                                               item.CmtOpt ?? string.Empty,
                                               subpet.OdrUnitName ?? string.Empty,
                                               (int?)subpet.SinKouiKbn ?? 0)).ToList();
        }

        public List<ListSetMstModel> GetAllTanSetInfs(int hpId, int generationId, int sinDate)
        {
            var list = _tenantDataContext.ListSetMsts.Where(x => x.HpId == hpId
                                                        && x.GenerationId == generationId
                                                        && x.IsDeleted == 0);

            var ItemCds = list.Select(x => x.ItemCd);
            var tenMsts = _tenantDataContext.TenMsts.Where(item => ItemCds.Contains(item.ItemCd)
                                        && item.StartDate <= sinDate
                                        && item.EndDate >= sinDate).Select(x => new
                                        {
                                            x.ItemCd,
                                            x.OdrUnitName,
                                            x.SinKouiKbn
                                        });

            return (from item in list
                    join tenMst in tenMsts on item.ItemCd equals tenMst.ItemCd into gj
                    from subpet in gj.DefaultIfEmpty()
                    select new ListSetMstModel(item.HpId,
                                               item.GenerationId,
                                               item.SetId,
                                               item.SetName ?? string.Empty,
                                               item.ItemCd ?? string.Empty,
                                               item.IsTitle,
                                               item.SetKbn,
                                               item.SelectType,
                                               item.Suryo,
                                               item.Level1,
                                               item.Level2,
                                               item.Level3,
                                               item.Level4,
                                               item.Level5,
                                               item.CmtName ?? string.Empty,
                                               item.CmtOpt ?? string.Empty,
                                               subpet.OdrUnitName ?? string.Empty,
                                               (int?)subpet.SinKouiKbn ?? 0)).ToList();
        }
    }
}