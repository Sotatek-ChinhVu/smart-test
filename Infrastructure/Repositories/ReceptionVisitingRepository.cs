using Domain.Models.Insurance;
using Domain.Models.ReceptionVisitingModel;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReceptionVisitingRepository : IReceptionVisitingRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public ReceptionVisitingRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }
        public List<ReceptionVisitingModel> GetReceptionVisiting(long raiinNo)
        {
            var listDataRaiinInf = _tenantDataContext.RaiinInfs.Where(x => x.RaiinNo == raiinNo).Select(x => new ReceptionVisitingModel(
                x.PtId, x.UketukeId, x.KaId, x.UketukeTime ?? String.Empty, x.Status, x.YoyakuId, x.TantoId)).ToList();
            return listDataRaiinInf;
        }

    }
}
