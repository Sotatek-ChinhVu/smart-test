using Domain.Models.SpecialNote.SummaryInf;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using System.Text;

namespace Infrastructure.Repositories.SpecialNote
{
    public class SummaryInfRepository : RepositoryBase, ISummaryInfRepository
    {
        public SummaryInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {

        }

        public SummaryInfModel Get(int hpId, long ptId)
        {
            var summaryInfs = NoTrackingDataContext.SummaryInfs.Where(x => x.PtId == ptId && x.HpId == hpId).OrderByDescending(u => u.UpdateDate).Select(x => new SummaryInfModel(
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

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
