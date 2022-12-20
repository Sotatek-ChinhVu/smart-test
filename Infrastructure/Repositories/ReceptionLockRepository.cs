using Domain.Models.LockInf;
using Domain.Models.ReceptionLock;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
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
    public class ReceptionLockRepository : RepositoryBase, IReceptionLockRepository
    {
        public ReceptionLockRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<ReceptionLockModel> ReceptionLockModel(long sinDate, long ptId, long raiinNo, string functionCd)
        {
            var listData = NoTrackingDataContext.LockInfs
                .Where(x => x.SinDate == sinDate && x.PtId == ptId && x.RaiinNo == raiinNo && x.FunctionCd == functionCd)
                .Select(x => new ReceptionLockModel(
                x.HpId,
                x.PtId,
                x.FunctionCd ?? string.Empty,
                x.SinDate,
                x.RaiinNo,
                x.OyaRaiinNo,
                x.Machine ?? string.Empty,
                x.UserId,
                x.LockDate))
                .ToList();
            return listData;
        }
    }
}