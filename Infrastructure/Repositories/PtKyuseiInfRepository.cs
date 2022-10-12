using Domain.Models.PtKyuseiInf;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class PtKyuseiInfRepository : IPtKyuseiInfRepository
    {
        private readonly TenantDataContext _tenantDataContextTracking;

        public PtKyuseiInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
        }

        public List<PtKyuseiInfModel> PtKyuseiInfModels(int hpId, long ptId, bool isDeleted)
        {
            var listPtKyusei = _tenantDataContextTracking.PtKyuseis
                .Where(x => x.HpId == hpId && x.PtId == ptId && (isDeleted || x.IsDeleted == 0))
                .OrderByDescending(x => x.CreateDate)
                .Select(x => new PtKyuseiInfModel(
                    x.HpId,
                    x.PtId,
                    x.KanaName ?? string.Empty,
                    x.Name ?? string.Empty,
                    x.EndDate,
                    x.IsDeleted))
                .ToList();
            return listPtKyusei;
        }
    }
}
