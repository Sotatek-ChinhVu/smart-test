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

        public List<KaikeiInfModel> GetListKaikeiInf(int hpId, long ptId)
        {
            return  NoTrackingDataContext.KaikeiInfs
                .Where(item => item.HpId == hpId && item.PtId == ptId)
                .Select(item => new KaikeiInfModel(item))
                .ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
