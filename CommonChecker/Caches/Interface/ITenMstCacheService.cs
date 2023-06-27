using Entity.Tenant;

namespace CommonChecker.Caches.Interface
{
    public interface ITenMstCacheService
    {
        void AddCache(List<string> itemCodeList);

        TenMst? GetTenMst(string itemCode, int sinday);

        List<TenMst> GetTenMstList(List<string> itemCodeList, int sinday);
    }
}
