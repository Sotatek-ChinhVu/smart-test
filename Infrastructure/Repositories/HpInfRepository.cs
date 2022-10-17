using Domain.Models.HpMst;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class HpInfRepository : IHpInfRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public HpInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public bool CheckHpId(int hpId)
        {
            var check = _tenantDataContext.HpInfs.Any(hp => hp.HpId == hpId);

            return check;

        }
    }
}
