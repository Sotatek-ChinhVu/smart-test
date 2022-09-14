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

        public List<ListSetMstModel> GetTanSetInfs(int hpId, int setUsageKbn, int generationId,int sinDate)
        {
            var list = _tenantDataContext.ListSetMsts.Where(x => x.HpId == hpId
                                                        && x.GenerationId == generationId
                                                        && x.IsDeleted == 0
                                                        && x.SetKbn == setUsageKbn);

            var ItemCds = list.Select(x => x.ItemCd);
            var tenMsts = _tenantDataContext.TenMsts.Where(item => ItemCds.Contains(item.ItemCd)
                                        && item.StartDate <= sinDate
                                        && item.EndDate >= sinDate).Select(x => new { x.ItemCd, x.OdrUnitName });

            return list.Join(tenMsts, list => list.ItemCd, tenMsts => tenMsts.ItemCd, (x, y) =>
                                    new ListSetMstModel(x.HpId,
                                                        x.GenerationId,
                                                        x.SetId,
                                                        x.SetName ?? string.Empty,
                                                        x.ItemCd ?? string.Empty,
                                                        x.IsTitle,
                                                        x.SetKbn,
                                                        x.SelectType,
                                                        x.Suryo,
                                                        x.Level1,
                                                        x.Level2,
                                                        x.Level3,
                                                        x.Level4,
                                                        x.Level5,
                                                        x.CmtName ?? string.Empty,
                                                        x.CmtOpt ?? string.Empty,
                                                        y.OdrUnitName ?? string.Empty)).ToList();
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
                                        && item.EndDate >= sinDate).Select(x => new { x.ItemCd, x.OdrUnitName });

            return list.Join(tenMsts, list => list.ItemCd, tenMsts => tenMsts.ItemCd, (x, y) =>
                                    new ListSetMstModel(x.HpId,
                                                        x.GenerationId,
                                                        x.SetId,
                                                        x.SetName ?? string.Empty,
                                                        x.ItemCd ?? string.Empty,
                                                        x.IsTitle,
                                                        x.SetKbn,
                                                        x.SelectType,
                                                        x.Suryo,
                                                        x.Level1,
                                                        x.Level2,
                                                        x.Level3,
                                                        x.Level4,
                                                        x.Level5,
                                                        x.CmtName ?? string.Empty,
                                                        x.CmtOpt ?? string.Empty,
                                                        y.OdrUnitName ?? string.Empty)).ToList();
        }

        public List<ListSetMstModel> GetAllTanSetInfs(int hpId,int generationId,int sinDate)
        {
            var list = _tenantDataContext.ListSetMsts.Where(x => x.HpId == hpId
                                                        && x.GenerationId == generationId
                                                        && x.IsDeleted == 0);

            var ItemCds = list.Select(x => x.ItemCd);
            var tenMsts = _tenantDataContext.TenMsts.Where(item => ItemCds.Contains(item.ItemCd)
                                        && item.StartDate <= sinDate
                                        && item.EndDate >= sinDate).Select(x=>new {x.ItemCd,x.OdrUnitName});


            return list.Join(tenMsts, list => list.ItemCd, tenMsts => tenMsts.ItemCd, (x, y) =>
                                    new ListSetMstModel(x.HpId,
                                                        x.GenerationId,
                                                        x.SetId,
                                                        x.SetName ?? string.Empty,
                                                        x.ItemCd ?? string.Empty,
                                                        x.IsTitle,
                                                        x.SetKbn,
                                                        x.SelectType,
                                                        x.Suryo,
                                                        x.Level1,
                                                        x.Level2,
                                                        x.Level3,
                                                        x.Level4,
                                                        x.Level5,
                                                        x.CmtName ?? string.Empty,
                                                        x.CmtOpt ?? string.Empty,
                                                        y.OdrUnitName ?? string.Empty)).ToList();
        }
    }
}