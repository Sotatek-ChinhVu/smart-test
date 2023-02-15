using Domain.Models.RsvInf;
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
        List<RsvInfModel> result = new List<RsvInfModel>();
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
        result = query.Select(u => new RsvInfModel(u.RsvInf.HpId, u.RsvInf.RsvFrameId, u.RsvInf.SinDate, u.RsvInf.StartTime, u.RsvInf.RaiinNo, u.RsvInf.PtId, u.RsvInf.RsvSbt, u.RsvInf.TantoId, u.RsvInf.KaId, u.Detail?.RsvFrmMst.RsvFrameName ?? string.Empty, u.Detail?.RsvGrpMst.RsvGrpName ?? string.Empty)).OrderBy(r => r.SinDate).ToList();
        return result;
    }


    public List<Tuple<long, int, string, string, string, string, string>> GetRsvInfoByRaiinInf(int hpId, long ptId, int sinDate)
    {
        List<Tuple<long, int, string, string, string, string, string>> result = new();

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
        result = query.Select(u => new Tuple<long, int, string, string, string, string, string>(u.RaiinInf.RaiinNo, u.RaiinInf.SinDate, u.RaiinInf.UketukeTime ?? string.Empty, u.KaMst.KaSname ?? string.Empty, u.RaiinInf.YoyakuTime ?? string.Empty, u.UserMst.Sname ?? string.Empty, u.raiinCmtInf.Text ?? string.Empty)).OrderBy(r => r.Item2).ToList();
        return result;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
