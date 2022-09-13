using Domain.Models.LockInf;
using Domain.Models.ReceptionLock;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReceptionLockRepository : IReceptionLockRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;

        public ReceptionLockRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public List<ReceptionLockModel> ReceptionLockModel(long raiinNo)
        {
            var listDataRaiinInf = _tenantDataContext.LockInfs
                .Where(x => x.RaiinNo == raiinNo)
                .Select(x => new ReceptionLockModel(
                x.HpId,
                x.PtId,
                x.FunctionCd,
                x.SinDate,
                x.RaiinNo,
                x.OyaRaiinNo,
                x.Machine,
                x.UserId,
                x.LockDate))
                .ToList();
            return listDataRaiinInf;
        }
    }
}