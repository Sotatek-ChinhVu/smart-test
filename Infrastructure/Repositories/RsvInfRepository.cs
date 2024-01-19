using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.RsvInf;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class RsvInfRepository : RepositoryBase, IRsvInfRepository
{
    public RsvInfRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<RsvInfModel> GetList(int hpId, long ptId, int sinDate)
    {
        int today = CIUtil.GetJapanDateTimeNow().ToString("yyyyMMdd").AsInteger();
        List<RsvInfModel> listRsvInfModel = GetRsvInfoByRsvInf(hpId, ptId, today);
        var listRaiinInfModel = GetRsvInfoByRaiinInf(hpId, ptId, today);
        listRaiinInfModel = listRaiinInfModel.Where(u => !listRsvInfModel.Any(r => r.RaiinNo == u.Item1)).ToList();
        foreach (var raiinInf in listRaiinInfModel)
        {
            listRsvInfModel.Add(new RsvInfModel(raiinInf.Item2, raiinInf.Item3, raiinInf.Item4, raiinInf.Item5, raiinInf.Item6, raiinInf.Item7));
        }
        return listRsvInfModel;
    }

    private List<RsvInfModel> GetRsvInfoByRsvInf(int hpId, long ptId, int sinDate)
    {
        var rsvInfs = NoTrackingDataContext.RsvInfs.Where(u => u.HpId == hpId &&
                                                                         u.PtId == ptId &&
                                                                         u.SinDate >= sinDate);
        var rsvFrmMsts = NoTrackingDataContext.RsvFrameMsts.Where(u => u.HpId == hpId &&
                                                                                      u.IsDeleted == 0);
        var rsvGrpMsts = NoTrackingDataContext.RsvGrpMsts.Where(u => u.HpId == hpId &&
                                                                                     u.IsDeleted == 0);
        var rsvDetailInfs = from rsvFrmMstItem in rsvFrmMsts.AsEnumerable()
                            join rsvGrpMstItem in rsvGrpMsts on rsvFrmMstItem.RsvGrpId equals rsvGrpMstItem.RsvGrpId
                            select new
                            {
                                RsvFrmMst = rsvFrmMstItem,
                                RsvGrpMst = rsvGrpMstItem
                            };
        var query = from rsvInfItem in rsvInfs.AsEnumerable()
                    join rsvDetailInfItem in rsvDetailInfs on rsvInfItem.RsvFrameId equals rsvDetailInfItem.RsvFrmMst.RsvFrameId into listDetail
                    from detail in listDetail.DefaultIfEmpty()
                    select new
                    {
                        RsvInf = rsvInfItem,
                        Detail = listDetail.FirstOrDefault(),
                    };
        var result = query.Select(u => new RsvInfModel(u.RsvInf.HpId, u.RsvInf.RsvFrameId, u.RsvInf.SinDate, u.RsvInf.StartTime, u.RsvInf.RaiinNo, u.RsvInf.PtId, u.RsvInf.RsvSbt, u.RsvInf.TantoId, u.RsvInf.KaId, u.Detail?.RsvFrmMst.RsvFrameName ?? string.Empty, u.Detail?.RsvGrpMst.RsvGrpName ?? string.Empty)).OrderBy(r => r.SinDate).ToList();
        return result;
    }


    public List<Tuple<long, int, string, string, string, string, string>> GetRsvInfoByRaiinInf(int hpId, long ptId, int sinDate)
    {
        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(u => u.HpId == hpId &&
                                                                                  u.PtId == ptId &&
                                                                                  u.SinDate >= sinDate &&
                                                                                  u.IsYoyaku == 1 &&
                                                                                  u.IsDeleted == DeleteTypes.None);
        var raiinCmtInfs = NoTrackingDataContext.RaiinCmtInfs.Where(u => u.HpId == hpId &&
                                                                                  u.PtId == ptId &&
                                                                                  u.SinDate >= sinDate &&
                                                                                  u.CmtKbn == 1 && // 来院コメント
                                                                                  u.IsDelete == DeleteTypes.None);
        var kaMsts = NoTrackingDataContext.KaMsts.Where(u => u.HpId == hpId &&
                                                                              u.IsDeleted == 0);
        var userMsts = NoTrackingDataContext.UserMsts.Where(u => u.HpId == hpId &&
                                                                                u.JobCd == 1 && // JobCd = 1 is Doctor
                                                                                u.IsDeleted == 0);
        var query = from raiinInfItem in raiinInfs.AsEnumerable()
                    join raiinCmtInf in raiinCmtInfs on raiinInfItem.RaiinNo equals raiinCmtInf.RaiinNo into listRaiinCmt
                    join kaMstItem in kaMsts on raiinInfItem.KaId equals kaMstItem.KaId into listKaMst
                    join userMstItem in userMsts on raiinInfItem.TantoId equals userMstItem.UserId into listTanto
                    select new
                    {
                        RaiinInf = raiinInfItem,
                        KaMst = listKaMst.FirstOrDefault(),
                        UserMst = listTanto.FirstOrDefault(),
                        raiinCmtInf = listRaiinCmt.FirstOrDefault()
                    };
        var result = query.Select(u => new Tuple<long, int, string, string, string, string, string>(u.RaiinInf.RaiinNo, u.RaiinInf.SinDate, u.RaiinInf.UketukeTime ?? string.Empty, u.KaMst?.KaSname ?? string.Empty, u.RaiinInf.YoyakuTime ?? string.Empty, u.UserMst?.Sname ?? string.Empty, u.raiinCmtInf?.Text ?? string.Empty)).OrderBy(r => r.Item2).ToList();
        return result;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public List<RsvInfToConfirmModel> GetListRsvInfToConfirmModel(int hpId, int sinDate)
    {
        var listRaiinInf = NoTrackingDataContext.RaiinInfs.Where(u => u.HpId == hpId && u.SinDate == sinDate && u.IsDeleted == DeleteStatus.None && u.Status == RaiinState.Reservation);
        var listPtInf = NoTrackingDataContext.PtInfs.Where(u => u.HpId == hpId && u.IsDelete == DeleteStatus.None && u.IsTester == 0);
        var listPtHokenInf = NoTrackingDataContext.PtHokenInfs.Where(u => u.HpId == hpId && (u.HokenKbn == 1 || u.HokenKbn == 2) && u.StartDate <= sinDate && u.EndDate >= sinDate && u.IsDeleted == DeleteStatus.None);
        var query = from raiinInf in listRaiinInf
                    join ptInf in listPtInf on raiinInf.PtId equals ptInf.PtId
                    join ptHokenInf in listPtHokenInf on ptInf.PtId equals ptHokenInf.PtId into listPtHoken
                    select new
                    {
                        RaiinInf = raiinInf,
                        PtInf = ptInf,
                        ListPtHokenInf = listPtHoken
                    };
        return query.AsEnumerable().Where(u => u.ListPtHokenInf != null && u.ListPtHokenInf?.Count() > 0)
                                   .Select(data => ConvertToModel(data.RaiinInf ?? new RaiinInf(),
                                                                  data.PtInf ?? new PtInf(),
                                                                  data.ListPtHokenInf.ToList()))
                                   .ToList();
    }

    private static RsvInfToConfirmModel ConvertToModel(RaiinInf raiinInf, PtInf ptInf, List<PtHokenInf> ptHokenInfs)
    {
        return new RsvInfToConfirmModel(
                                        ptInf.Name ?? string.Empty,
                                        raiinInf.HpId,
                                        raiinInf.SinDate,
                                        raiinInf.RaiinNo,
                                        raiinInf.PtId,
                                        ptInf.PtNum.AsLong(),
                                        ptInf.Birthday,
                                        raiinInf.TantoId,
                                        raiinInf.KaId,
                                        ptHokenInfs.Select(x => new HokenInfModel(x.PtId,
                                                                                  x.HokenId,
                                                                                  x.SeqNo,
                                                                                  x.HokenNo,
                                                                                  x.HokenEdaNo,
                                                                                  x.HokenKbn,
                                                                                  x.HokensyaNo ?? string.Empty,
                                                                                  x.Kigo ?? string.Empty,
                                                                                  x.Bango ?? string.Empty,
                                                                                  x.EdaNo ?? string.Empty,
                                                                                  x.HonkeKbn,
                                                                                  x.KogakuKbn
                                                                                  )).ToList());
    }
}
