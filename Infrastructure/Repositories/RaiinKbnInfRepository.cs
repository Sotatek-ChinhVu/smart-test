using Domain.Models.RaiinKbnInf;
using Entity.Tenant;
using Infrastructure.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class RaiinKbnInfRepository : IRaiinKbnInfRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public RaiinKbnInfRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetDataContext();
    }

    public void Upsert(int hpId, long ptId, int sinDate, long raiinNo, int grpId, int kbnCd)
    {
        // Use Index (HpId, PtId, SinDate, RaiinNo, GrpId, IsDelete) to find the record faster
        var raiinKbnInf = _tenantDataContext.RaiinKbnInfs.FirstOrDefault(r =>
            r.HpId == hpId
            && r.PtId == ptId
            && r.SinDate == sinDate
            && r.RaiinNo == raiinNo
            && r.GrpId == grpId
            && r.IsDelete == DeleteTypes.None);
        if (raiinKbnInf is null)
        {
            // Insert
            _tenantDataContext.RaiinKbnInfs.Add(new RaiinKbnInf
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = sinDate,
                RaiinNo = raiinNo,
                GrpId = grpId,
                KbnCd = kbnCd
            });
        }
        else
        {
            // Update
            raiinKbnInf.KbnCd = kbnCd;
            raiinKbnInf.UpdateDate = DateTime.UtcNow;
        }

        _tenantDataContext.SaveChanges();
    }
}
