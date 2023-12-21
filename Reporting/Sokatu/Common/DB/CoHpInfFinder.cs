using Domain.Constant;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.Sokatu.Common.Models;

namespace Reporting.Sokatu.Common.DB
{
    public class CoHpInfFinder : RepositoryBase, ICoHpInfFinder
    {
        public CoHpInfFinder(ITenantProvider tenantProvider) : base(tenantProvider) { }

        public CoHpInfModel GetHpInf(int hpId, int seikyuYm)
        {
            HpInf hpInf = NoTrackingDataContext.HpInfs.Where(h =>
                h.HpId == hpId &&
                h.StartDate <= seikyuYm * 100 + 31
            ).OrderByDescending(h => h.StartDate).FirstOrDefault() ?? new();

            return new CoHpInfModel(hpInf);
        }

        public List<CoKaMstModel> GetKaMst(int hpId)
        {
            var kaMsts = NoTrackingDataContext.KaMsts.Where(k =>
                k.HpId == hpId &&
                k.IsDeleted == DeleteStatus.None
            ).OrderBy(k => k.KaId).ToList();

            return kaMsts.Select(k => new CoKaMstModel(k)).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
