using Domain.Models.InputItem;
using Domain.Models.IpnKasanExcludeItem;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class IpnKasanExcludeRepository : IIpnKasanExcludeRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public IpnKasanExcludeRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public bool CheckIsGetYakkaPrice(int hpId, InputItemModel? tenMst, int sinDate)
        {
            if (tenMst == null) return false;
            var ipnKasanExclude = _tenantDataContext.ipnKasanExcludes.Where(u => u.HpId == hpId && u.IpnNameCd == tenMst.IpnNameCd && u.StartDate <= sinDate && u.EndDate >= sinDate).FirstOrDefault();

            var ipnKasanExcludeItem = _tenantDataContext.ipnKasanExcludeItems.Where(u => u.HpId == hpId && u.ItemCd == tenMst.ItemCd && u.StartDate <= sinDate && u.EndDate >= sinDate).FirstOrDefault();
            return ipnKasanExclude == null && ipnKasanExcludeItem == null;
        }
    }
}
