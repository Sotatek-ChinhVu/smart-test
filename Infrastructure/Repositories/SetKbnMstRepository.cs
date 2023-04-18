using Domain.Models.SetGenerationMst;
using Domain.Models.SetKbnMst;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories
{
    public class SetKbnMstRepository : RepositoryBase, ISetKbnMstRepository
    {
        private readonly IMemoryCache _memoryCache;
        public SetKbnMstRepository(ITenantProvider tenantProvider, IMemoryCache memoryCache) : base(tenantProvider)
        {
            _memoryCache = memoryCache;
        }

        private IEnumerable<SetKbnMstModel> ReloadCache()
        {
            var setKbnMstList = NoTrackingDataContext.SetKbnMsts.Where(s => s.HpId == 1 && s.IsDeleted == 0).Select(s =>
                    new SetKbnMstModel(
                        s.HpId,
                        s.SetKbn,
                        s.SetKbnEdaNo,
                        string.IsNullOrEmpty(s.SetKbnName) ? String.Empty : s.SetKbnName,
                        s.KaCd,
                        s.DocCd,
                        s.IsDeleted,
                        s.GenerationId
                    )
                  ).ToList();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.Normal);
            _memoryCache.Set(GetCacheKey(), setKbnMstList, cacheEntryOptions);

            return setKbnMstList;
        }

        public IEnumerable<SetKbnMstModel> GetList(int hpId, int setKbnFrom, int setKbnTo)
        {
            if (!_memoryCache.TryGetValue(GetCacheKey(), out IEnumerable<SetKbnMstModel>? setKbnMstList))
            {
                setKbnMstList = ReloadCache();
            }

            return setKbnMstList!.Where(s => s.HpId == hpId && s.SetKbn >= setKbnFrom && s.SetKbn <= setKbnTo && s.IsDeleted == 0).OrderBy(s => s.SetKbn).ToList();
        }


        public List<SetKbnMstModel> GetListKbnMst(int hpId, List<int> grpCodes, int sinDate)
        {
            if (!_memoryCache.TryGetValue(GetCacheKey(), out IEnumerable<SetKbnMstModel>? setKbnMstList))
            {
                setKbnMstList = ReloadCache();
            }
            List<SetKbnMstModel> result = new List<SetKbnMstModel>();
            var lowerSetGenarationMsts = NoTrackingDataContext.SetGenerationMsts.Where(x => x.HpId == hpId &&
                                                                            x.IsDeleted == 0 &&
                                                                            x.StartDate <= sinDate)
                                                                          .OrderByDescending(x => x.StartDate)
                                                                          .ToList();
            var setGenarationMst = lowerSetGenarationMsts.FirstOrDefault();

            if (setGenarationMst != null)
            {
                var setKbnMsts = NoTrackingDataContext.SetKbnMsts.Where(x => x.HpId == hpId &&
                                                                               grpCodes.Contains(x.SetKbn) &&
                                                                               x.IsDeleted == 0 &&
                                                                               x.GenerationId == setGenarationMst.GenerationId)
                                                               .OrderBy(x => x.SetKbn)
                                                               .ToList();
                foreach (var item in setKbnMsts)
                {
                    result.Add(new SetKbnMstModel(item.HpId, item.SetKbn, item.SetKbnEdaNo, item.SetKbnName ?? string.Empty, item.KaCd, item.DocCd, item.IsDeleted, item.GenerationId));
                }
            }
            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
