using Domain.Models.MonshinInf;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MonshinInforRepository : IMonshinInforRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;

        public MonshinInforRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<MonshinInforModel> MonshinInforModels(int hpId, long ptId)
        {
            var monshinList = _tenantDataContext.MonshinInfo
                .Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0).OrderByDescending(x => x.SinDate)
                .Select(x => new MonshinInforModel(
                x.HpId,
                x.PtId,
                x.RaiinNo,
                x.SinDate,
                x.Text))
                .ToList();
            return monshinList;
        }
    }
}
