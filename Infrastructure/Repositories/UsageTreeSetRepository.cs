﻿using Domain.Models.UsageTreeSet;
using Entity.Tenant;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class UsageTreeSetRepository : IUsageTreeSetRepository
    {
        private readonly TenantDataContext _tenantDataContext;

        public UsageTreeSetRepository(TenantDataContext tenantDataContext)
        {
            _tenantDataContext = tenantDataContext;
        }

        public int GetGenerationId(int sinDate)
        {
            ListSetGenerationMst? generation = _tenantDataContext.ListSetGenerationMsts.Where
                            (item => item.StartDate < sinDate)
                            .OrderByDescending(item => item.StartDate)
                            .FirstOrDefault();

            return generation?.GenerationId ?? 0;
        }

        public List<ListSetMstModel> GetTanSetInfs(int hpId, int setUsageKbn, int GenerationId)
        {
            IQueryable<ListSetMst> list = _tenantDataContext.ListSetMsts.Where(x => x.HpId == hpId
                                                        && x.GenerationId == GenerationId
                                                        && x.IsDeleted == 0
                                                        && x.SetKbn == setUsageKbn);

            return list.Select(x => new ListSetMstModel(x.HpId,
                                                        x.GenerationId,
                                                        x.SetName,
                                                        x.ItemCd,
                                                        x.IsTitle,
                                                        x.SetKbn,
                                                        x.SelectType,
                                                        x.Suryo,
                                                        x.Level1,
                                                        x.Level2,
                                                        x.Level3,
                                                        x.Level4,
                                                        x.Level5,
                                                        x.CmtName,
                                                        x.CmtOpt)).ToList();
        }

        public List<ListSetMstModel> GetTanSetInfs(int hpId, IEnumerable<int> usageContains, int GenerationId)
        {
            IQueryable<ListSetMst> list = _tenantDataContext.ListSetMsts.Where(x => x.HpId == hpId
                                                        && x.GenerationId == GenerationId
                                                        && x.IsDeleted == 0
                                                        && usageContains.Contains(x.SetKbn));

            return list.Select(x => new ListSetMstModel(x.HpId,
                                                        x.GenerationId,
                                                        x.SetName,
                                                        x.ItemCd,
                                                        x.IsTitle,
                                                        x.SetKbn,
                                                        x.SelectType,
                                                        x.Suryo,
                                                        x.Level1,
                                                        x.Level2,
                                                        x.Level3,
                                                        x.Level4,
                                                        x.Level5,
                                                        x.CmtName,
                                                        x.CmtOpt)).ToList();
        }
    }
}