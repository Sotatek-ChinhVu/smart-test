using Domain.Models.SetGenerationMst;
using Domain.Models.SetKbnMst;
using Infrastructure.Base;
using Infrastructure.Interfaces;
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
            if (!_memoryCache.TryGetValue(GetCacheKey(), out IEnumerable<SetKbnMstModel> setKbnMstList))
            {
                setKbnMstList = ReloadCache();
            }

            return setKbnMstList!.Where(s => s.HpId == hpId && s.SetKbn >= setKbnFrom && s.SetKbn <= setKbnTo && s.IsDeleted == 0).OrderBy(s => s.SetKbn).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
