using Domain.Models.Futan;
using Domain.Models.SinKoui;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Repositories
{
    public class SinKouiRepository : RepositoryBase, ISinKouiRepository
    {
        public SinKouiRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<string> GetListKaikeiInf(int hpId, long ptId)
        {
            var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();
            var result = kaikeiInfs.Select(x => ToModel(x)).Distinct().ToList();
            return result.Select(x => x.SinYmBinding).Distinct().ToList();
        }

        private static KaikeiInfModel ToModel(KaikeiInf u)
        {
            return new KaikeiInfModel(
                u.PtId,
                u.SinDate);
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
