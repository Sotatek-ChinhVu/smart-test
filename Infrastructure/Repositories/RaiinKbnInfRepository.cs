﻿using Domain.Models.RaiinKbnInf;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class RaiinKbnInfRepository : IRaiinKbnInfRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public RaiinKbnInfRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public void Upsert(int hpId, long ptId, int sinDate, long raiinNo, int grpId, int kbnCd, int userId)
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
                KbnCd = kbnCd,
                CreateDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                UpdateId = userId,
                CreateId = userId
            });
        }
        else
        {
            // Update
            raiinKbnInf.KbnCd = kbnCd;
            raiinKbnInf.UpdateDate = DateTime.UtcNow;
            raiinKbnInf.UpdateId = userId;
        }

        _tenantDataContext.SaveChanges();
    }

    public bool SoftDelete(int hpId, long ptId, int sinDate, long raiinNo, int grpId)
    {
        var raiinKbnInf = _tenantDataContext.RaiinKbnInfs.FirstOrDefault(r =>
            r.HpId == hpId
            && r.PtId == ptId
            && r.SinDate == sinDate
            && r.RaiinNo == raiinNo
            && r.GrpId == grpId
            && r.IsDelete == DeleteTypes.None);
        if (raiinKbnInf is null)
        {
            return false;
        }

        raiinKbnInf.IsDelete = DeleteTypes.Deleted;
        _tenantDataContext.SaveChanges();
        return true;
    }
}
