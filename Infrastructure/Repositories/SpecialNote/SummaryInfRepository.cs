using Domain.Models.SpecialNote.SummaryInf;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.SpecialNote
{
    public class SummaryInfRepository : ISummaryInfRepository
    {
        private readonly TenantDataContext _tenantDataContext;

        public SummaryInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
        }

        public SummaryInfModel Get(int hpId, long ptId)
        {
            var summaryInfs = _tenantDataContext.SummaryInfs.Where(x => x.PtId == ptId && x.HpId == hpId).OrderByDescending(u => u.UpdateDate).Select(x => new SummaryInfModel(
                   x.Id,
                   x.HpId,
                   x.PtId,
                   x.SeqNo,
                   x.Text ?? String.Empty,
                   x.Rtext == null ? string.Empty : Encoding.UTF8.GetString(x.Rtext),
                   x.CreateDate
                )).ToList();
            return summaryInfs.Any() ? summaryInfs.First() : new SummaryInfModel();
        }
    }
}
