using Domain.Models.SetGenerationMst;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories
{
    public class SetGenerationMstRepository : RepositoryBase, ISetGenerationMstRepository
    {
        private readonly IMemoryCache _memoryCache;
        public SetGenerationMstRepository(ITenantProvider tenantProvider, IMemoryCache memoryCache) : base(tenantProvider)
        {
            _memoryCache = memoryCache;
        }

        private IEnumerable<SetGenerationMstModel> ReloadCache()
        {
            var setGenerationMstList = NoTrackingDataContext.SetGenerationMsts.Where(s => s.HpId == 1 && s.IsDeleted == 0).Select(s =>
                    new SetGenerationMstModel(
                        s.HpId,
                        s.GenerationId,
                        s.StartDate,
                        s.IsDeleted
                    )
                  ).ToList();
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.Normal);
            _memoryCache.Set(GetCacheKey(), setGenerationMstList, cacheEntryOptions);

            return setGenerationMstList;
        }

        public IEnumerable<SetGenerationMstModel> GetList(int hpId, int sinDate)
        {
            if (!_memoryCache.TryGetValue(GetCacheKey(), out IEnumerable<SetGenerationMstModel>? setGenerationMstList))
            {
                setGenerationMstList = ReloadCache();
            }

            return setGenerationMstList!.Where(s => s.StartDate <= sinDate).OrderByDescending(x => x.StartDate).ToList();
        }

        public int GetGenerationId(int hpId, int sinDate)
        {
            int generationId = 0;
            try
            {
                var setGenerationMstList = GetList(hpId, sinDate);
                var generation = setGenerationMstList.OrderByDescending(x => x.StartDate).FirstOrDefault();
                if (generation != null)
                {
                    generationId = generation.GenerationId;
                }
            }
            catch
            {
                return 0;
            }
            return generationId;
        }

        public List<SetSendaiGenerationModel> GetListSendaiGeneration(int hpId)
        {
            var result = new List<SetSendaiGenerationModel>();

            // Get List Data DB
            var setGenerationMsts = NoTrackingDataContext.SetGenerationMsts.Where(x => x.HpId == hpId && x.IsDeleted == 0).OrderByDescending(x => x.StartDate).ToList();
            int k = 1;
            for (int i = 0; i < setGenerationMsts.Count; i++)
            {
                if (i == 0)
                {
                    result.Add(new SetSendaiGenerationModel(hpId, setGenerationMsts[i].GenerationId, setGenerationMsts[i].StartDate, convertDateDisplay(setGenerationMsts[i].StartDate), 0, convertDateDisplay(0), i));
                }
                else
                {
                    DateTime endTimeDate = CIUtil.IntToDate(setGenerationMsts[i - 1].StartDate);
                    endTimeDate = endTimeDate == DateTime.MinValue ? DateTime.MinValue : endTimeDate.AddDays(-1);
                    var endDateInt = CIUtil.DateTimeToInt(endTimeDate);
                    result.Add(new SetSendaiGenerationModel(hpId, setGenerationMsts[i].GenerationId, setGenerationMsts[i].StartDate, convertDateDisplay(setGenerationMsts[i].StartDate), endDateInt, convertDateDisplay(endDateInt), i));
                }
                k++;
            }
            return result;
        }

        private string convertDateDisplay(int date)
        {
            if(date == 0)
            {
                return "xxx/xx";
            }
            if(date > 0)
            {
                var formatDate = CIUtil.IntToDate(date);
                return formatDate.Year.ToString() + "/" + (formatDate.Month > 9 ? formatDate.Month.ToString() : "0" + formatDate.Month.ToString());
            }
            return "";
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
