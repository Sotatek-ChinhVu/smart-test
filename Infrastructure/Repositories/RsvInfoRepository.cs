using Domain.Models.RsvInfo;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class RsvInfoRepository : IRsvInfoRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public RsvInfoRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<RsvInfoModel> GetList(int hpId, long ptId, int sinDate)
    {
        var ptRsvInfos = _tenantDataContext.RsvInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.SinDate >= sinDate).Select(x => new RsvInfoModel(
             x.HpId,
             x.RsvFrameId,
             x.SinDate,
             x.StartTime,
             x.RaiinNo,
             x.PtId,
             x.RsvSbt,
             x.TantoId,
             x.KaId));
        return ptRsvInfos.ToList();
    }
}
