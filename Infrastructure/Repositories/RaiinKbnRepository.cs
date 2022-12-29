using Domain.Models.RaiinKbn;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class RaiinKbnRepository : RepositoryBase, IRaiinKbnRepository
{
    public RaiinKbnRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void Upsert(int hpId, long ptId, int sinDate, long raiinNo, int grpId, int kbnCd, int userId)
    {
        // Use Index (HpId, PtId, SinDate, RaiinNo, GrpId, IsDelete) to find the record faster
        var raiinKbnInf = TrackingDataContext.RaiinKbnInfs.FirstOrDefault(r =>
            r.HpId == hpId
            && r.PtId == ptId
            && r.SinDate == sinDate
            && r.RaiinNo == raiinNo
            && r.GrpId == grpId
            && r.IsDelete == DeleteTypes.None);
        if (raiinKbnInf is null)
        {
            // Insert
            TrackingDataContext.RaiinKbnInfs.Add(new RaiinKbnInf
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

        TrackingDataContext.SaveChanges();
    }

    public bool SoftDelete(int hpId, long ptId, int sinDate, long raiinNo, int grpId)
    {
        var raiinKbnInf = TrackingDataContext.RaiinKbnInfs.FirstOrDefault(r =>
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
        TrackingDataContext.SaveChanges();
        return true;
    }


    public List<RaiinKbnModel> GetRaiinKbns(int hpId, long ptId, long raiinNo, int sinDate)
    {
        var raiinKbnMstRespo = NoTrackingDataContext.RaiinKbnMsts.Where(p => p.IsDeleted == 0 && p.HpId == hpId);
        var raiinKbnDetailRespo = NoTrackingDataContext.RaiinKbnDetails.Where(p => p.IsDeleted == 0 && p.HpId == hpId);
        var raiinKbnInfRespo = NoTrackingDataContext.RaiinKbnInfs.Where(p => p.IsDelete == 0 && p.HpId == hpId && p.RaiinNo == raiinNo && p.PtId == ptId && p.SinDate == sinDate);
        var r = raiinKbnInfRespo.ToList();
        var result = (from kbnMst in raiinKbnMstRespo.AsEnumerable()
                      join kbnDetail in raiinKbnDetailRespo on
                      new { kbnMst.HpId, kbnMst.GrpCd } equals
                      new { kbnDetail.HpId, kbnDetail.GrpCd } into details
                      join kbnInf in raiinKbnInfRespo on
                      new { kbnMst.HpId, kbnMst.GrpCd } equals
                      new { kbnInf.HpId, GrpCd = kbnInf.GrpId } into infs
                      from inf in infs.OrderByDescending(p => p.SeqNo).Take(1).DefaultIfEmpty()
                      where
                      kbnMst.IsDeleted == 0 &&
                      kbnMst.HpId == hpId
                      select new
                      {
                          KbnMst = kbnMst,
                          KbnDetails = details.OrderBy(p => p.SortNo),
                          KbnInf = inf
                      })
                      ?.OrderBy(p => p.KbnMst.SortNo)
                      ?.Select(obj => new RaiinKbnModel(obj.KbnMst.HpId, obj.KbnMst.GrpCd, obj.KbnMst.SortNo, obj.KbnMst?.GrpName ?? string.Empty, obj.KbnMst?.IsDeleted ?? 0,
                                                             new RaiinKbnInfModel(hpId, ptId, sinDate, raiinNo, obj.KbnInf?.GrpId ?? 0, obj.KbnInf?.SeqNo ?? 0, obj.KbnInf?.KbnCd ?? 0, obj.KbnInf?.IsDelete ?? 0), obj.KbnDetails?.Select(p => new RaiinKbnDetailModel(p.HpId, p.GrpCd, p.KbnCd, p.SortNo, p.KbnName ?? string.Empty, p.ColorCd ?? string.Empty, p.IsConfirmed, p.IsAuto, p.IsAutoDelete, p.IsDeleted)).ToList() ?? new()))?.ToList() ?? new();
        return result;
    }

    public List<RaiinKbnModel> InitDefaultByRsv(int hpId, int frameID, List<RaiinKbnModel> raiinKbns)
    {
        var raiinKbnYoyakus = NoTrackingDataContext.RaiinKbnYayokus
                .Where(x => x.IsDeleted == 0 && x.YoyakuCd == frameID && x.HpId == hpId)
                .ToList();
        foreach (var raiinKbnMst in raiinKbns)
        {
            if (raiinKbnMst.RaiinKbnInfModel.KbnCd != 0) continue;

            foreach (var detail in raiinKbnMst.RaiinKbnDetailModels)
            {
                var raiinKbnRsvs = raiinKbnYoyakus.Where(x => x.GrpId == detail.GrpCd && x.KbnCd == detail.KbnCd).FirstOrDefault();
                if (raiinKbnRsvs != null)
                {
                    raiinKbnMst.RaiinKbnInfModel.ChangeKbnCd(detail.KbnCd);
                    break;
                }
            }
        }

        return raiinKbns;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
