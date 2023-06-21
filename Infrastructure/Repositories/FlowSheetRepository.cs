using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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

        private string HolidayMstCacheKey
        {
            get => $"{GetCacheKey()}-HolidayMstCacheKey";
        }

        private string RaiinListMstCacheKey
        {
            get => $"{GetCacheKey()}-RaiinListMstCacheKey";
        }

        private readonly IMemoryCache _memoryCache;
        public FlowSheetRepository(ITenantProvider tenantProvider, IMemoryCache memoryCache) : base(tenantProvider)
        {
            _memoryCache = memoryCache;
        }

        public List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo, ref long totalCount)
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Start GetListFlowSheet");

            // From History
            var allRaiinInfList = NoTrackingDataContext.RaiinInfs
                .Where(r => r.HpId == hpId && r.PtId == ptId && r.Status >= RaiinState.TempSave && r.IsDeleted == 0)
                .Select(r => new FlowSheetModel(r.SinDate, r.PtId, r.RaiinNo, r.UketukeTime ?? string.Empty, r.SyosaisinKbn, r.Status))
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
                                    select new FlowSheetModel(g.Key.RsvDate, g.Key.PtId, g.Key.RsvkrtNo, string.Empty, -1, 0)
                               ).ToList();

            Console.WriteLine("Get groupNextOdr: " + stopwatch.ElapsedMilliseconds);


            var allFlowSheetQueryable = allRaiinInfList.Union(groupNextOdr);

            totalCount = allFlowSheetQueryable.Count();
            List<FlowSheetModel> flowSheetModelList =
                allFlowSheetQueryable.OrderByDescending(r => r.SinDate)
                                     .ThenByDescending(r => r.UketukeTime)
                                     .ThenByDescending(r => r.RaiinNo)
                                     .ToList();

            var nextKarteList = NoTrackingDataContext.RsvkrtKarteInfs
                .Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0 && k.Text != null && !string.IsNullOrEmpty(k.Text.Trim()) && k.KarteKbn == 1)
                .ToList();
            Console.WriteLine("Get nextKarteList: " + stopwatch.ElapsedMilliseconds);

            var historyKarteList = NoTrackingDataContext.KarteInfs
                .Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0 && k.Text != null && !string.IsNullOrEmpty(k.Text.Trim()) && k.KarteKbn == 1)
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
                        flowSheetModel.UketukeTime,
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

        #region RaiinListMst

        private List<RaiinListMstModel> ReloadRaiinListMstCache(int hpId)
        {
            var raiinListMst = NoTrackingDataContext.RaiinListMsts.Where(m => m.HpId == hpId && m.IsDeleted == DeleteTypes.None).ToList();
            var raiinListDetail = NoTrackingDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None).ToList();
            var query = from mst in raiinListMst
                        select new
                        {
                            Mst = mst,
                            Detail = raiinListDetail.Where(c => c.HpId == mst.HpId && c.GrpId == mst.GrpId).ToList()
                        };
            var raiinListMstModelList = query
                .Select(data => new RaiinListMstModel(data.Mst.GrpId, data.Mst.GrpName ?? string.Empty, data.Mst.SortNo, data.Mst.IsDeleted , data.Detail.Select(d => new RaiinListDetailModel(d.GrpId, d.KbnCd, d.SortNo, d.KbnName ?? string.Empty, d.ColorCd ?? String.Empty, d.IsDeleted)).ToList()))
                .ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.Normal);
            _memoryCache.Set(RaiinListMstCacheKey, raiinListMstModelList, cacheEntryOptions);

            return raiinListMstModelList;
        }

        public List<RaiinListMstModel> GetRaiinListMsts(int hpId)
        {
            if (!_memoryCache.TryGetValue(RaiinListMstCacheKey, out List<RaiinListMstModel>? setKbnMstList))
            {
                setKbnMstList = ReloadRaiinListMstCache(hpId);
            }

            return setKbnMstList!;
        }

        #endregion

        #region HolidayMst
        private List<HolidayDto> ReloadHolidayCache(int hpId)
        {
            var holidayModelList = NoTrackingDataContext.HolidayMsts
                .Where(h => h.HpId == hpId && h.IsDeleted == DeleteTypes.None)
                .Select(h => new HolidayDto(h.SeqNo, h.SinDate, h.HolidayKbn, h.KyusinKbn, h.HolidayName ?? string.Empty))
                .ToList();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetPriority(CacheItemPriority.Normal);
            _memoryCache.Set(HolidayMstCacheKey, holidayModelList, cacheEntryOptions);

            return holidayModelList;
        }

        public bool SaveHolidayMst(HolidayModel holiday, int userId)
        {
            var holidayUpdate = TrackingDataContext.HolidayMsts.FirstOrDefault(x => x.HpId == holiday.HpId && x.SinDate == holiday.SinDate);
            if (holidayUpdate == null)
            {
                TrackingDataContext.HolidayMsts.Add(new HolidayMst()
                {
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    CreateId = userId,
                    HolidayKbn = holiday.HolidayKbn,
                    HolidayName = holiday.HolidayKbn == 0 ? string.Empty : holiday.HolidayName,
                    IsDeleted = DeleteTypes.None,
                    HpId = holiday.HpId,
                    KyusinKbn = holiday.KyusinKbn,
                    UpdateId = userId,
                    SeqNo = 0,
                    SinDate = holiday.SinDate,
                    UpdateDate = CIUtil.GetJapanDateTimeNow()
                });
            }
            else
            {
                holidayUpdate.KyusinKbn = holiday.KyusinKbn;
                holidayUpdate.HolidayKbn = holiday.HolidayKbn;
                holidayUpdate.HolidayName = holiday.HolidayName;
                holidayUpdate.UpdateDate = CIUtil.GetJapanDateTimeNow();
                holidayUpdate.UpdateId = userId;
                if (holidayUpdate.HolidayKbn == 0)
                    holidayUpdate.HolidayName = string.Empty;
            }
            _memoryCache.Remove(HolidayMstCacheKey);
            return TrackingDataContext.SaveChanges() > 0;
        }

        public List<HolidayDto> GetHolidayMst(int hpId, int holidayFrom, int holidayTo)
        {

            if (!_memoryCache.TryGetValue(HolidayMstCacheKey, out IEnumerable<HolidayDto>? holidayMstList))
            {
                holidayMstList = ReloadHolidayCache(hpId);
            }
            return holidayMstList!.Where(h => holidayFrom <= h.SinDate && h.SinDate <= holidayTo).ToList();
        }

        #endregion

        public void UpsertTag(List<FlowSheetModel> inputDatas, int hpId, int userId)
        {
            foreach (var inputData in inputDatas)
            {
                var raiinListTag = TrackingDataContext.RaiinListTags
                           .OrderByDescending(p => p.UpdateDate)
                           .FirstOrDefault(p => p.RaiinNo == inputData.RaiinNo);
                if (raiinListTag is null)
                {
                    if (inputData.TagNo != -1)
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
                }
                else
                {
                    if (raiinListTag.TagNo != inputData.TagNo)
                    {
                        raiinListTag.TagNo = inputData.TagNo;
                        raiinListTag.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        raiinListTag.UpdateId = userId;
                    }
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
                    if (!string.IsNullOrEmpty(inputData.Comment))
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
                }
                else
                {
                    if (raiinListCmt.Text != inputData.Comment)
                    {
                        raiinListCmt.Text = inputData.Comment;
                        raiinListCmt.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        raiinListCmt.UpdateId = userId;
                    }
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
                        on new { raiinListInf.GrpId, raiinListInf.KbnCd } equals new { raiinListMst.GrpId, raiinListMst.KbnCd }
                        where raiinListInf.RaiinNo != 0
                        select new { raiinListInf.RaiinNo, raiinListInf.GrpId, raiinListInf.KbnCd, raiinListInf.RaiinListKbn, raiinListMst.KbnName, raiinListMst.ColorCd }
                     );

            var result = raiinListInfs
                .GroupBy(r => r.RaiinNo)
                .ToDictionary(g => g.Key, g => g.Select(r => new RaiinListInfModel(r.RaiinNo, r.GrpId, r.KbnCd, r.RaiinListKbn, r.KbnName, r.ColorCd)).ToList());

            return result;
        }

        public Dictionary<int, List<RaiinListInfModel>> GetRaiinListInfForNextOrder(int hpId, long ptId)
        {
            var raiinListInfs =
                     (
                        from raiinListInf in NoTrackingDataContext.RaiinListInfs.Where(r => r.HpId == hpId && r.PtId == ptId)
                        join raiinListMst in NoTrackingDataContext.RaiinListDetails.Where(d => d.HpId == hpId && d.IsDeleted == DeleteTypes.None)
                        on new { raiinListInf.GrpId, raiinListInf.KbnCd } equals new { raiinListMst.GrpId, raiinListMst.KbnCd }
                        where raiinListInf.RaiinNo == 0
                        select new { raiinListInf.SinDate, raiinListInf.RaiinNo, raiinListInf.GrpId, raiinListInf.KbnCd, raiinListInf.RaiinListKbn, raiinListMst.KbnName, raiinListMst.ColorCd }
                     );

            var result = raiinListInfs
                .GroupBy(r => r.SinDate)
                .ToDictionary(g => g.Key, g => g.Select(r => new RaiinListInfModel(r.RaiinNo, r.GrpId, r.KbnCd, r.RaiinListKbn, r.KbnName, r.ColorCd)).ToList());

            return result;
        }

        public List<(int date, string tooltip)> GetTooltip(int hpId, long ptId, int sinDate, int startDate, int endDate)
        {
            List<int> dates = new();
            for (int i = startDate; i <= endDate; i++)
            {
                dates.Add(i);
            }

            List<(int, string)> result = new();
            var raiinInfs = NoTrackingDataContext.RaiinInfs
                .Where(r => r.HpId == hpId && r.PtId == ptId && r.IsDeleted == DeleteTypes.None && r.SinDate >= startDate && r.SinDate <= endDate)
                .Select(r => new { r.SinDate, r.SyosaisinKbn, r.Status }).ToList();
            var holidays = NoTrackingDataContext.HolidayMsts.Where(r => r.HpId == hpId && r.IsDeleted == DeleteTypes.None && r.SinDate >= startDate && r.SinDate <= endDate).Select(r => new { r.SinDate, r.HolidayName }).ToList();

            object obj = new object();
            Parallel.ForEach(dates, date =>
            {
                string tooltip = "";
                var holiday = holidays.FirstOrDefault(h => h.SinDate == date);
                if (!string.IsNullOrEmpty(holiday?.HolidayName ?? string.Empty))
                {
                    tooltip = string.Format("{0} {1}", CIUtil.IntToDate(date).ToString("MM/dd"), holiday?.HolidayName ?? string.Empty);
                }

                var dateSyosaiItems = raiinInfs.Where(item => item.SinDate == date);
                var datetateItem = raiinInfs.FirstOrDefault(item => item.SinDate == date);
                foreach (var dateSyosaiItem in dateSyosaiItems)
                {
                    if (!dateSyosaiItem.Equals(default(KeyValuePair<int, int>)))
                    {
                        if (!(!datetateItem?.Equals(default(KeyValuePair<int, int>)) == true && date == sinDate && datetateItem?.Status < RaiinState.TempSave))
                        {
                            tooltip = (string.IsNullOrEmpty(tooltip) ? "" : tooltip + Environment.NewLine) + (SyosaiConst.FlowSheetCalendarDict.ContainsKey(dateSyosaiItem.SyosaisinKbn) ? SyosaiConst.FlowSheetCalendarDict[dateSyosaiItem.SyosaisinKbn] : string.Empty);
                        }

                    }
                }
                if (!string.IsNullOrEmpty(tooltip))
                {
                    lock (obj)
                    {
                        result.Add(new(date, tooltip));
                    }
                }
            });

            return result;
        }
    }
}