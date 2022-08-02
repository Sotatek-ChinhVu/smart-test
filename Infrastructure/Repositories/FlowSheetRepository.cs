using Domain.Constant;
using Domain.Models.FlowSheet;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extendsions;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Collections.ObjectModel;

namespace Infrastructure.Repositories
{
    public class FlowSheetRepository : IFlowSheetRepository
    {
        private readonly TenantDataContext _tenantDataContext;
        public FlowSheetRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }
        public List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo)
        {
            var raiinInfs = _tenantDataContext.RaiinInfs.Where(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == 0 && r.Status >= 3);
            var karteInfs = _tenantDataContext.KarteInfs.Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0 && !string.IsNullOrEmpty(k.Text.Trim()));
            var output = raiinInfs.Join(karteInfs, r => new { r.HpId, r.PtId, r.RaiinNo, r.SinDate }, k => new { k.HpId, k.PtId, k.RaiinNo, k.SinDate }, 
                                    (r, k) => new FlowSheetModel(r.HpId, r.PtId, r.SinDate, k.Text, r.RaiinNo, r.SyosaisinKbn, false, false, r.Status)).ToList();
            var nextOdrs = GetNextOdr(hpId, ptId);
            output.AddRange(nextOdrs);
            output = output.OrderByDescending(item => item.SinDate).ThenByDescending(item => item.RaiinNo).ToList();
            if (raiinNo > 0 && output.FirstOrDefault(item => item.RaiinNo == raiinNo && item.SinDate == sinDate) == null)
            {
                var todayOrd = CreateTodayOrder(hpId, ptId, sinDate, raiinNo);
                output.Insert(0, todayOrd);
            }
            return output;
        }
        public List<RaiinListTag> GetRaiinListTags(int hpId, long ptId)
        {
            return _tenantDataContext.RaiinListTags.Where(tag => tag.HpId == hpId && tag.PtId == ptId && tag.IsDeleted == DeleteTypes.None).ToList();
        }
        public List<RaiinListCmt> GetRaiinListCmts(int hpId, long ptId)
        {
            return _tenantDataContext.RaiinListCmts.Where(cmt => cmt.HpId == hpId && cmt.PtId == ptId && cmt.IsDeleted == DeleteTypes.None).ToList();
        }
        public List<RaiinListInfModel> GetRaiinListInfModels (int hpId, long ptId)
        {
            var raiinListInfs = _tenantDataContext.RaiinListInfs.Where(raiinInf => raiinInf.HpId == hpId && raiinInf.PtId == ptId);
            var raiinListDetails = _tenantDataContext.RaiinListDetails.Where(detail => detail.HpId == hpId && detail.IsDeleted == DeleteTypes.None);
            var output = raiinListInfs.Where(r => r.HpId == hpId && r.PtId == ptId)
                                .Join(raiinListDetails, inf => new { inf.GrpId, inf.KbnCd }, detail => new { detail.GrpId, detail.KbnCd }, 
                                (inf, detail) => new RaiinListInfModel(inf.HpId, inf.PtId, inf.RaiinNo, inf.SinDate, detail.GrpId, detail.KbnCd, inf.RaiinListKbn, detail.SortNo))
                                .ToList();
            return output;
        }
        public IEnumerable<FlowSheetModel> GetNextOdr (int hpId, long ptId)
        {
            var rsvkrtOdrInfs = _tenantDataContext.RsvkrtOdrInfs.Where(r => r.HpId == hpId
                                                                       && r.PtId == ptId
                                                                       && r.IsDeleted == DeleteTypes.None);
            var rsvkrtMsts = _tenantDataContext.RsvkrtMsts.Where(r => r.HpId == hpId
                                                                 && r.PtId == ptId
                                                                 && r.IsDeleted == DeleteTypes.None
                                                                 && r.RsvkrtKbn == 0);
            var nextOdrKarteInfs = _tenantDataContext.RsvkrtKarteInfs.Where(karte => karte.HpId == hpId
                                                                            && karte.PtId == ptId
                                                                            && karte.IsDeleted == 0
                                                                            && !string.IsNullOrEmpty(karte.Text.Trim()))
                                                                            .OrderBy(karte => karte.RsvDate)
                                                                            .ThenBy(karte => karte.KarteKbn);
            var output = rsvkrtOdrInfs.Join(rsvkrtMsts, o => new { o.HpId, o.PtId, o.RsvkrtNo }, m => new { m.HpId, m.PtId, m.RsvkrtNo }, (o, m) => new { RsvkrtOdrInfs = o, RsvkrtMsts = m })
                                        .Join(nextOdrKarteInfs, o => new { o.RsvkrtOdrInfs.HpId, o.RsvkrtOdrInfs.PtId, o.RsvkrtOdrInfs.RsvkrtNo }, k => new { k.HpId, k.PtId, k.RsvkrtNo },
                                        (o, k) => new { RsvkrtOdrInfs = o.RsvkrtOdrInfs, RsvkrtMsts = o.RsvkrtMsts, RsvkrtKarteInfs = k })
                                        .GroupBy( o => new { o.RsvkrtOdrInfs.HpId, o.RsvkrtOdrInfs.PtId, o.RsvkrtOdrInfs.RsvDate, o.RsvkrtOdrInfs.RsvkrtNo })
                                        .Select(g => new { g.Key.HpId, g.Key.PtId, g.Key.RsvDate, g.Key.RsvkrtNo })
                                        .Join(nextOdrKarteInfs, g => new { g.HpId, g.PtId, g.RsvkrtNo }, k => new { k.HpId, k.PtId, k.RsvkrtNo }, 
                                        (g, k) => new { NextOdr = g, Karte = k});
            var nextOdrs = output.AsEnumerable().Select(
                    data => new FlowSheetModel(hpId, ptId, data.NextOdr.RsvDate, data.Karte.Text ?? string.Empty, data.NextOdr.RsvkrtNo, -1, true, false, 0));
            return nextOdrs;
        }

        public FlowSheetModel CreateTodayOrder(int hpId, long ptId, int sinDate, long raiinNo)
        {
            FlowSheetModel newModel = new FlowSheetModel(hpId, ptId, sinDate, string.Empty, raiinNo, 2, false, true, 0);
            RaiinListTagModel tag = new RaiinListTagModel(hpId, ptId, raiinNo, sinDate, true);
            RaiinListCmtModel cmt = new RaiinListCmtModel(hpId, ptId, raiinNo, sinDate, 9, string.Empty, true);
            newModel.RaiinListTag = tag;
            newModel.RaiinListCmt = cmt;
            List<RaiinListInfModel> raiinList = new List<RaiinListInfModel>();
            var raiinListInf = GetRaiinListInfModels(hpId, ptId).FindAll(item => item.HpId == hpId
                                                             && item.PtId == ptId
                                                             && item.SinDate == sinDate
                                                             && (item.RaiinNo == raiinNo || item.RaiinNo == 0)
                                                             || (item.RaiinNo == 0 && item.RaiinListKbn == 4))
                                                            .OrderBy(item => item.SortNo);
            foreach (var item in raiinListInf)
            {
                if (raiinList.Any(r => r.GrpId == item.GrpId)) continue;
                raiinList.Add(new RaiinListInfModel(item)
                {
                    IsContainsFile = raiinListInf?.FirstOrDefault(x =>
                        x.GrpId == item.GrpId && x.KbnCd == item.KbnCd &&
                        item.RaiinListKbn == 4) != null
                });
            }
            newModel.RaiinListInfs = raiinList;
            return newModel;
        }

        public List<RaiinListMstModel> GetRaiinListMsts(int hpId)
        {
            var raiinListMst = _tenantDataContext.RaiinListMsts.Where(m => m.HpId == hpId && m.IsDeleted == DeleteTypes.None).ToList();
            var raiinListDetail = _tenantDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None).ToList();
            var query = from mst in raiinListMst
                        select new
                        {
                            Mst = mst,
                            Detail = raiinListDetail.Where(c => c.HpId == mst.HpId && c.GrpId == mst.GrpId).ToList()
                        };
            var output = query.Select(
                data => new RaiinListMstModel
                {
                    GrpId = data.Mst.GrpId,
                    GrpName = data.Mst.GrpName,
                    SortNo = data.Mst.SortNo,
                    RaiinListDetailsList = data.Detail?.ToList()
                });
            return output.ToList();
        }
        public List<HolidayModel> GetHolidayMst(int hpId)
        {
            var holidayCollection = _tenantDataContext.HolidayMsts.Where(h => h.HpId == hpId && h.IsDeleted == DeleteTypes.None);
            return holidayCollection.Select(h => new HolidayModel(h)).ToList();
        }
        public List<RaiinDateModel> GetListRaiinNo(int hpId, long ptId, int sinDate)
        {
            DateTime now = CIUtil.IntToDate(sinDate);
            var limitFomCurrentAgoDateTime = now.AddMonths(-12);
            int limitFromCurrentAgo = CIUtil.DateTimeToInt(new DateTime(limitFomCurrentAgoDateTime.Year, limitFomCurrentAgoDateTime.Month, 1));
            int limitLastDate = CIUtil.DateTimeToInt(new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)));
            List<RaiinDateModel> listRaiinNo = _tenantDataContext.RaiinInfs.Where(c => c.PtId == ptId && c.HpId == hpId && c.IsDeleted == DeleteTypes.None
                                        && c.SinDate >= limitFromCurrentAgo && c.SinDate <= limitLastDate)
                                        .GroupBy(gr => gr.SinDate)
                                        .Select(g => new RaiinDateModel
                                        {
                                            SinDate = g.Key,
                                            RaiinNo = g.Min(cm => cm.RaiinNo)
                                        }).ToList();
            return listRaiinNo;
        }
    }
}
