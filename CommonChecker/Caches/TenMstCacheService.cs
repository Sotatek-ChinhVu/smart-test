using CommonChecker.Caches.Interface;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace CommonChecker.Caches
{
    public class TenMstCacheService : RepositoryBase, ITenMstCacheService
    {
        private readonly List<string> _itemCodeCacheList = new List<string>();
        private readonly List<TenMst> _tenMstCacheList = new List<TenMst>();
        public TenMstCacheService(ITenantProvider tenantProvider) : base(tenantProvider)
        {

        }

        public void AddCache(List<string> itemCodeList)
        {
            if (itemCodeList == null || itemCodeList.Count == 0)
            {
                return;
            }

            _itemCodeCacheList.AddRange(itemCodeList);

            _tenMstCacheList.AddRange(NoTrackingDataContext.TenMsts.Where(t => itemCodeList.Contains(t.ItemCd) && t.IsDeleted == 0).ToList());
        }

        private void AddCacheIfNeed(List<string> itemCodeList)
        {
            List<string> itemCodeListNotCache = itemCodeList.Where(i => !_itemCodeCacheList.Contains(i)).ToList();
            if (itemCodeListNotCache == null || 
                itemCodeListNotCache.Count == 0)
            {
                return;
            }
            AddCache(itemCodeListNotCache);
        }

        public TenMst? GetTenMst(string itemCode, int sinday)
        {
            AddCacheIfNeed(new List<string>() { itemCode });

            return _tenMstCacheList.FirstOrDefault(t => itemCode == t.ItemCd && t.StartDate <= sinday && sinday <= t.EndDate);
        }

        public List<TenMst> GetTenMstList(List<string> itemCodeList, int sinday)
        {
            AddCacheIfNeed(itemCodeList);

            return _tenMstCacheList.Where(t => itemCodeList.Contains(t.ItemCd) && t.StartDate <= sinday && sinday <= t.EndDate).ToList();
        }
    }
}
