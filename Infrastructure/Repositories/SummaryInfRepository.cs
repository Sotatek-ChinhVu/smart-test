using Domain.Models.SummaryInf;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Text;

namespace Infrastructure.Repositories;

public class SummaryInfRepository : ISummaryInfRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public SummaryInfRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<SummaryInfModel> GetList(int hpId, long ptId)
    {
        var summaryInfs = _tenantDataContext.SummaryInfs.Where(x => x.PtId == ptId && x.HpId == hpId).OrderByDescending(u => u.UpdateDate).Select(x => new SummaryInfModel(
               x.Id,
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.Text ?? String.Empty,
               x.Rtext == null ? string.Empty : Encoding.UTF8.GetString(x.Rtext),
               x.CreateDate
            ));
        return summaryInfs.ToList() ?? new List<SummaryInfModel>();
    }
}
