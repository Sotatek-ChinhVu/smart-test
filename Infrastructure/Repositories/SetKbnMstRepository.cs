using Domain.Models.SetKbnMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
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
            if (!_memoryCache.TryGetValue(GetCacheKey(), out IEnumerable<SetKbnMstModel>? setKbnMstList))
            {
                setKbnMstList = ReloadCache();
            }

            return setKbnMstList!.Where(s => s.HpId == hpId && s.SetKbn >= setKbnFrom && s.SetKbn <= setKbnTo && s.IsDeleted == 0).OrderBy(s => s.SetKbn).ToList();
        }

        public bool Upsert(int hpId, int userId, int generationId, List<SetKbnMstModel> setKbnMstModels)
        {
            int maxSetKbn = NoTrackingDataContext.SetKbnMsts.Where(s => s.GenerationId == s.GenerationId && s.HpId == hpId).Select(s => s.SetKbn).ToList().DefaultIfEmpty(0).Max();
            foreach (var model in setKbnMstModels)
            {
                maxSetKbn++;
                var setKbnMst = TrackingDataContext.SetKbnMsts.FirstOrDefault(x => x.HpId == Session.HospitalID &&
                                                                                     x.IsDeleted == 0 &&
                                                                                     x.SetKbn == model.SetKbn &&
                                                                                     x.GenerationId == generationId);
                if (setKbnMst != null)
                {
                    if (setKbnMst.IsDeleted == DeleteTypes.Deleted)
                    {
                        setKbnMst.SetKbn = DeleteTypes.Deleted;
                        setKbnMst.UpdateId = Session.UserID;
                        setKbnMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }
                    else
                    {
                        setKbnMst.SetKbnName = model.SetKbnName;
                        setKbnMst.UpdateId = Session.UserID;
                        setKbnMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    }

                }
                else
                {
                    var newSetKbnMst = new SetKbnMst
                    {
                        HpId = model.HpId,
                        IsDeleted = 0,
                        SetKbnName = model.SetKbnName,
                        GenerationId = generationId,
                        KaCd = model.KaCd,
                        DocCd = model.DocCd,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        CreateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        SetKbnEdaNo = 0,
                        SetKbn = maxSetKbn
                    };
                    TrackingDataContext.SetKbnMsts.Add(newSetKbnMst);
                }
            }

            return TrackingDataContext.SaveChanges() > 0;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
