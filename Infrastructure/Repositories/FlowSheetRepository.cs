using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Repositories
{
    public class FlowSheetRepository : RepositoryBase, IFlowSheetRepository
    {
        private readonly int cmtKbn = 9;
        private readonly string sinDate = "sindate";
        private readonly string tagNo = "tagno";
        private readonly string fullLineOfKarte = "fulllineofkarte";
        private readonly string syosaisinKbn = "syosaisinkbn";
        private readonly string comment = "comment";

        public FlowSheetRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo, ref long totalCount)
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Start GetListFlowSheet");

            // From History
            var allRaiinInfList = NoTrackingDataContext.RaiinInfs
                .Where(r => r.HpId == hpId && r.PtId == ptId && r.Status > 3 && r.IsDeleted == 0)  
                .Select(r => new FlowSheetModel(r.SinDate, r.PtId, r.RaiinNo, r.SyosaisinKbn, r.Status))
                .ToList();

            Console.WriteLine("Get allRaiinInfList: " + stopwatch.ElapsedMilliseconds);

            // From NextOrder
            var rsvkrtOdrInfs = NoTrackingDataContext.RsvkrtOdrInfs.Where(r => r.HpId == hpId
                                                                                        && r.PtId == ptId
                                                                                        && r.IsDeleted == DeleteTypes.None);
            var rsvkrtMsts = NoTrackingDataContext.RsvkrtMsts.Where(r => r.HpId == hpId
                                                                                        && r.PtId == ptId
                                                                                        && r.IsDeleted == DeleteTypes.None
                                                                                        && r.RsvkrtKbn == 0);

            var groupNextOdr = (
                                    from rsvkrtOdrInf in rsvkrtOdrInfs.AsEnumerable<RsvkrtOdrInf>()
                                    join rsvkrtMst in rsvkrtMsts on new { rsvkrtOdrInf.HpId, rsvkrtOdrInf.PtId, rsvkrtOdrInf.RsvkrtNo }
                                                     equals new { rsvkrtMst.HpId, rsvkrtMst.PtId, rsvkrtMst.RsvkrtNo }
                                    group rsvkrtOdrInf by new { rsvkrtOdrInf.HpId, rsvkrtOdrInf.PtId, rsvkrtOdrInf.RsvDate, rsvkrtOdrInf.RsvkrtNo } into g
                                    select new FlowSheetModel(g.Key.RsvDate, g.Key.PtId, g.Key.RsvkrtNo, -1, 0)
                               ).ToList();

            Console.WriteLine("Get groupNextOdr: " + stopwatch.ElapsedMilliseconds);


            var allFlowSheetQueryable = allRaiinInfList.Union(groupNextOdr);
            
            totalCount = allFlowSheetQueryable.Count();
            List<FlowSheetModel> flowSheetModelList = 
                allFlowSheetQueryable.OrderByDescending(r => r.SinDate)
                                     .ThenByDescending(r => r.RaiinNo)
                                     .ToList();

            var nextKarteList = NoTrackingDataContext.RsvkrtKarteInfs
                .Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0 && k.Text != null && !string.IsNullOrEmpty(k.Text.Trim()))
                .ToList();
            Console.WriteLine("Get nextKarteList: " + stopwatch.ElapsedMilliseconds);

            var historyKarteList = NoTrackingDataContext.KarteInfs
                .Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0 && k.Text != null && !string.IsNullOrEmpty(k.Text.Trim()))
                .ToList();
            Console.WriteLine("Get historyKarteList: " + stopwatch.ElapsedMilliseconds);

            var tagInfList = NoTrackingDataContext.RaiinListTags
                .Where(tag => tag.HpId == hpId && tag.PtId == ptId && tag.IsDeleted == 0)
                .ToList();
            Console.WriteLine("Get tagInfList: " + stopwatch.ElapsedMilliseconds);

            var commentList = NoTrackingDataContext.RaiinListCmts
                .Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0 && k.Text != null && !string.IsNullOrEmpty(k.Text.Trim()))
                .ToList();
            Console.WriteLine("Get commentList: " + stopwatch.ElapsedMilliseconds);

            //var result =
            //    from flowsheet in flowSheetModelList
            //    join nextKarte in nextKarteList on flowsheet.RaiinNo equals nextKarte.RsvkrtNo into gj
            //    from subNextKarte in gj.DefaultIfEmpty()
            //    join historyKarte in historyKarteList on flowsheet.RaiinNo equals historyKarte.RaiinNo into gj1
            //    from subHistoryKarte in gj1.DefaultIfEmpty()
            //    join tagInf in tagInfList on flowsheet.RaiinNo equals tagInf.RaiinNo into gj2
            //    from subTagInf in gj2.DefaultIfEmpty()
            //    join commentInf in commentList on flowsheet.RaiinNo equals commentInf.RaiinNo into gj3
            //    from subCommentInf in gj3.DefaultIfEmpty()
            //    select new FlowSheetModel(
            //        flowsheet.SinDate,
            //        subTagInf != null ? subTagInf.TagNo : 0,
            //        (subNextKarte == null || string.IsNullOrEmpty(subNextKarte.Text)) ? ((subHistoryKarte == null || string.IsNullOrEmpty(subHistoryKarte.Text)) ? string.Empty : subHistoryKarte.Text!) : subNextKarte.Text!,
            //        flowsheet.RaiinNo,
            //        flowsheet.SyosaisinKbn,
            //        (subCommentInf == null || string.IsNullOrEmpty(subCommentInf.Text)) ? string.Empty : subCommentInf.Text,
            //        flowsheet.Status,
            //        flowsheet.IsNext,
            //        !flowsheet.IsNext,
            //        new List<RaiinListInfModel>(),
            //        ptId,
            //        false
            //        );
            //return result.ToList();

            List<FlowSheetModel> result = new List<FlowSheetModel>();
            foreach (var flowSheetModel in flowSheetModelList)
            {
                string karteContent = string.Empty;
                if (flowSheetModel.IsNext)
                {
                    var nextKarte = nextKarteList.FirstOrDefault(n => n.RsvkrtNo == flowSheetModel.RaiinNo);
                    karteContent = nextKarte?.Text ?? string.Empty;
                }
                else
                {
                    var historyKarte = historyKarteList.FirstOrDefault(n => n.RaiinNo == flowSheetModel.RaiinNo);
                    karteContent = historyKarte?.Text ?? string.Empty;
                }

                int tagNoValue = 0;
                var tagInf = tagInfList.FirstOrDefault(t => t.RaiinNo == flowSheetModel.RaiinNo);
                if (tagInf != null)
                {
                    tagNoValue = tagInf.TagNo;
                }

                var commentInf = commentList.FirstOrDefault(t => t.RaiinNo == flowSheetModel.RaiinNo);
                string commentValue = (commentInf == null || commentInf.Text == null) ? string.Empty : commentInf.Text;

                result.Add(new FlowSheetModel
                    (
                        flowSheetModel.SinDate,
                        tagNoValue,
                        karteContent,
                        flowSheetModel.RaiinNo,
                        flowSheetModel.SyosaisinKbn,
                        commentValue,
                        flowSheetModel.Status,
                        flowSheetModel.IsNext,
                        !flowSheetModel.IsNext,
                        new List<RaiinListInfModel>(),
                        ptId,
                        false
                    ));
            }

            Console.WriteLine("End GetListFlowSheet");

            return result;
        }

        public List<RaiinListMstModel> GetRaiinListMsts(int hpId)
        {
            var raiinListMst = NoTrackingDataContext.RaiinListMsts.Where(m => m.HpId == hpId && m.IsDeleted == DeleteTypes.None).ToList();
            var raiinListDetail = NoTrackingDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None).ToList();
            var query = from mst in raiinListMst
                        select new
                        {
                            Mst = mst,
                            Detail = raiinListDetail.Where(c => c.HpId == mst.HpId && c.GrpId == mst.GrpId).ToList()
                        };
            var output = query.Select(
                data => new RaiinListMstModel(data.Mst.GrpId, data.Mst.GrpName ?? string.Empty, data.Mst.SortNo, data.Detail.Select(d => new RaiinListDetailModel(d.GrpId, d.KbnCd, d.SortNo, d.KbnName ?? string.Empty, d.ColorCd ?? String.Empty, d.IsDeleted)).ToList()));
            return output.ToList();
        }

        public List<HolidayModel> GetHolidayMst(int hpId, int holidayFrom, int holidayTo)
        {
            var holidayCollection = NoTrackingDataContext.HolidayMsts.Where(h => h.HpId == hpId && h.IsDeleted == DeleteTypes.None && holidayFrom <= h.SinDate && h.SinDate <= holidayTo);
            return holidayCollection.Select(h => new HolidayModel(h.SinDate, h.HolidayKbn, h.KyusinKbn, h.HolidayName ?? string.Empty)).ToList();
        }

        public void UpsertTag(List<FlowSheetModel> inputDatas, int hpId, int userId)
        {
            foreach (var inputData in inputDatas)
            {
                var raiinListTag = TrackingDataContext.RaiinListTags
                           .OrderByDescending(p => p.UpdateDate)
                           .FirstOrDefault(p => p.RaiinNo == inputData.RaiinNo);
                if (raiinListTag is null)
                {
                    TrackingDataContext.RaiinListTags.Add(new RaiinListTag
                    {
                        HpId = hpId,
                        PtId = inputData.PtId,
                        SinDate = inputData.SinDate,
                        RaiinNo = inputData.RaiinNo,
                        TagNo = inputData.TagNo,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        CreateId = userId
                    });
                }
                else
                {
                    raiinListTag.TagNo = inputData.TagNo;
                    raiinListTag.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    raiinListTag.UpdateId = userId;
                }
            }
            TrackingDataContext.SaveChanges();
        }
        public void UpsertCmt(List<FlowSheetModel> inputDatas, int hpId, int userId)
        {
            foreach (var inputData in inputDatas)
            {
                var raiinListCmt = TrackingDataContext.RaiinListCmts
                               .OrderByDescending(p => p.UpdateDate)
                               .FirstOrDefault(p => p.RaiinNo == inputData.RaiinNo);

                if (raiinListCmt is null)
                {
                    TrackingDataContext.RaiinListCmts.Add(new RaiinListCmt
                    {
                        HpId = hpId,
                        PtId = inputData.PtId,
                        SinDate = inputData.SinDate,
                        RaiinNo = inputData.RaiinNo,
                        CmtKbn = cmtKbn,
                        Text = inputData.Comment,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        CreateId = userId
                    });
                }
                else
                {
                    raiinListCmt.Text = inputData.Comment;
                    raiinListCmt.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    raiinListCmt.UpdateId = userId;
                }
            }
            TrackingDataContext.SaveChanges();
        }

        private List<FlowSheetModel> SortAll(string sort, List<FlowSheetModel> todayNextOdrs)
        {
            try
            {
                var childrenOfSort = sort.Trim().Split(",");
                var order = todayNextOdrs.OrderBy(o => o.PtId);
                foreach (var item in childrenOfSort)
                {
                    var elementDynamics = item.Trim().Split(" ");
                    var checkGroupId = int.TryParse(elementDynamics[0], out int groupId);

                    if (!checkGroupId)
                    {
                        order = SortStaticColumn(elementDynamics.FirstOrDefault() ?? string.Empty, elementDynamics?.Count() > 1 ? elementDynamics.LastOrDefault() ?? string.Empty : string.Empty, order);
                    }
                    else
                    {
                        if (elementDynamics.Length > 1)
                        {
                            if (elementDynamics[1].ToLower() == "desc")
                            {
                                order = order.ThenByDescending(o => o.RaiinListInfs.FirstOrDefault(r => r.GrpId == groupId)?.KbnName);
                            }
                            else
                            {
                                order = order.ThenBy(o => o.RaiinListInfs.FirstOrDefault(r => r.GrpId == groupId)?.KbnName);
                            }
                        }
                        else
                        {
                            order = order.ThenBy(o => o.RaiinListInfs.FirstOrDefault(r => r.GrpId == groupId)?.KbnName);
                        }
                    }
                }
                todayNextOdrs = order.ToList();
            }
            catch
            {
                todayNextOdrs = todayNextOdrs.OrderByDescending(o => o.SinDate).ToList();
            }

            return todayNextOdrs;
        }

        private IOrderedEnumerable<FlowSheetModel> SortStaticColumn(string fieldName, string sortType, IOrderedEnumerable<FlowSheetModel> order)
        {
            if (fieldName.ToLower().Equals(sinDate))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.SinDate);

                }
                else
                {
                    order = order.ThenByDescending(o => o.SinDate);
                }
            }
            if (fieldName.ToLower().Equals(tagNo))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.TagNo);

                }
                else
                {
                    order = order.ThenByDescending(o => o.TagNo);
                }
            }
            if (fieldName.ToLower().Equals(fullLineOfKarte))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.FullLineOfKarte);

                }
                else
                {
                    order = order.ThenByDescending(o => o.FullLineOfKarte);
                }
            }
            if (fieldName.ToLower().Equals(syosaisinKbn))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.SyosaisinKbn);

                }
                else
                {
                    order = order.ThenByDescending(o => o.SyosaisinKbn);
                }
            }
            if (fieldName.ToLower().Equals(comment))
            {
                if (string.IsNullOrEmpty(fieldName) || sortType.ToLower() == "asc")
                {
                    order = order.ThenBy(o => o.Comment);

                }
                else
                {
                    order = order.ThenByDescending(o => o.Comment);
                }
            }

            return order;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public Dictionary<long, List<RaiinListInfModel>> GetRaiinListInf(int hpId, long ptId)
        {
            var raiinListInfs =
                     (
                        from raiinListInf in NoTrackingDataContext.RaiinListInfs.Where(r => r.HpId == hpId && r.PtId == ptId)
                        join raiinListMst in NoTrackingDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None)
                        on new { raiinListInf.GrpId, raiinListInf.KbnCd } equals new { raiinListMst.GrpId , raiinListMst.KbnCd }
                        select new { raiinListInf.RaiinNo, raiinListInf.GrpId, raiinListInf.KbnCd, raiinListInf.RaiinListKbn, raiinListMst.KbnName, raiinListMst.ColorCd }
                     );

            var result = raiinListInfs
                .GroupBy(r => r.RaiinNo)
                .ToDictionary(g => g.Key, g => g.Select(r => new RaiinListInfModel(r.RaiinNo, r.GrpId, r.KbnCd, r.RaiinListKbn, r.KbnName, r.ColorCd)).ToList());

            return result;
        }
    }
}