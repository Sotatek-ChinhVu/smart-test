using Domain.Models.RaiinCmtInf;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RaiinCmtInfRepository : IRaiinCmtInfRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public RaiinCmtInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetDataContext();
        }
        public RaiinCmtInfModel GetRaiinCmtInf(long ptId, long raiinNo, long sinDate)
        {
            var data = _tenantDataContext.RaiinCmtInfs.SingleOrDefault(p => p.PtId == ptId && p.RaiinNo == raiinNo && p.SinDate <= sinDate);
            return new RaiinCmtInfModel(
                data.HpId,
                ptId,
                data.SinDate, 
                data.RaiinNo,
                data.CmtKbn,
                data.SeqNo,
                data.Text,
                data.IsDelete,
                data.CreateDate,
                data.CreateId,
                data.CreateMachine,
                data.UpdateDate,
                data.UpdateId,
                data.UpdateMachine);
        }
    }
}
