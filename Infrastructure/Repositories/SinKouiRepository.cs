using Domain.Models.SinKoui;
using Domain.Models.User;
using Entity.Tenant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Repositories
{
    public class SinKouiRepository : RepositoryBase, ISinKouiRepository
    {
        public SinKouiRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<string> GetListKaikeiInf(int hpId, long ptId)
        {
            var listSindate = NoTrackingDataContext.KaikeiInfs.Where(item => item.HpId == hpId && item.PtId == ptId)
                .Select(item => new KaikeiInfModel(item))
                .ToList();
            var sinDateFinder = listSindate.Select(x => x.SinYmBinding).Distinct().ToList();
            return sinDateFinder;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
