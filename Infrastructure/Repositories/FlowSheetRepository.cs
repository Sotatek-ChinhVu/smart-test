using Domain.Models.FlowSheet;
using Domain.Models.RaiinListMst;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SetMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using PostgreDataContext;
using System.Diagnostics;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using Infrastructure.CommonDB;
using Microsoft.Extensions.Configuration;

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
            get => $"{key}-HolidayMstCacheKey";
        }

        private string RaiinListMstCacheKey
        {
            get => $"{key}-RaiinListMstCacheKey";
        }

        private readonly TenantDataContext _tenantHistory;
        private readonly TenantDataContext _tenantNextOrder;
        private readonly TenantDataContext _tenantKarteInf;
        private readonly TenantDataContext _tenantNextKarteInf;
        private readonly TenantDataContext _tenantTagInf;
        private readonly TenantDataContext _tenantCmtInf;
        private readonly StackExchange.Redis.IDatabase _cache;
        private readonly IConfiguration _configuration;
        private string key;
        public FlowSheetRepository(ITenantProvider tenantProvider, ITenantProvider tenantRaiinInf, ITenantProvider tenantNextOrder, ITenantProvider tenantNextKarteInf, ITenantProvider tenantKarteInf, ITenantProvider tenantTagInf, ITenantProvider tenantCmtInf, IConfiguration configuration) : base(tenantProvider)
        {
            _tenantHistory = tenantRaiinInf.GetNoTrackingDataContext();
            _tenantNextOrder = tenantNextOrder.GetNoTrackingDataContext();
            _tenantKarteInf = tenantKarteInf.GetNoTrackingDataContext();
            _tenantNextKarteInf = tenantNextKarteInf.GetTrackingTenantDataContext();
            _tenantTagInf = tenantTagInf.GetTrackingTenantDataContext();
            _tenantCmtInf = tenantCmtInf.GetTrackingTenantDataContext();
            _configuration = configuration;
            key = GetCacheKey();
            GetRedis();
            _cache = RedisConnectorHelper.Connection.GetDatabase();
        }

        public void GetRedis()
        {
            string connection = string.Concat(_configuration["Redis:RedisHost"], ":", _configuration["Redis:RedisPort"]);
            if (RedisConnectorHelper.RedisHost != connection)
            {
                RedisConnectorHelper.RedisHost = connection;
            }
        }

        public List<FlowSheetModel> GetListFlowSheet(int hpId, long ptId, int sinDate, long raiinNo, ref long totalCount)
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Start GetListFlowSheet");

            // From History
            var taskRaiinInf = Task<List<FlowSheetModel>>.Factory.StartNew(() => TodayFlowSheet(hpId, ptId));


            Console.WriteLine("Get allRaiinInfList: " + stopwatch.ElapsedMilliseconds);

            // From NextOrder
            var taskNextRaiin = Task<List<FlowSheetModel>>.Factory.StartNew(() => NextOrderFlowSheet(hpId, ptId));

            Console.WriteLine("Get groupNextOdr: " + stopwatch.ElapsedMilliseconds);

            var taskNextKarteInf = Task<List<RsvkrtKarteInf>>.Factory.StartNew(() => RsvkrtKarteInf(hpId, ptId));
            Console.WriteLine("Get nextKarteList: " + stopwatch.ElapsedMilliseconds);

            var taskKarteInf = Task<List<KarteInf>>.Factory.StartNew(() => KarteInf(hpId, ptId));

            Console.WriteLine("Get historyKarteList: " + stopwatch.ElapsedMilliseconds);

            var taskTagInf = Task<List<RaiinListTag>>.Factory.StartNew(() => RaiinListTag(hpId, ptId));

            Console.WriteLine("Get tagInfList: " + stopwatch.ElapsedMilliseconds);

            var taskCmt = Task<List<RaiinListCmt>>.Factory.StartNew(() => RaiinListCmt(hpId, ptId));

            Console.WriteLine("Get commentList: " + stopwatch.ElapsedMilliseconds);
            Task.WaitAll(taskRaiinInf, taskNextRaiin, taskNextKarteInf, taskKarteInf, taskTagInf, taskCmt);
            var allRaiinInfList = taskRaiinInf.Result;
            var groupNextOdr = taskNextRaiin.Result;
            var nextKarteList = taskNextKarteInf.Result;
            var historyKarteList = taskKarteInf.Result;
            var tagInfList = taskTagInf.Result;
            var commentList = taskCmt.Result;

           var allFlowSheetQueryable = allRaiinInfList.Union(groupNextOdr);

            totalCount = allFlowSheetQueryable.Count();
            List<FlowSheetModel> flowSheetModelList =
                allFlowSheetQueryable.OrderByDescending(r => r.SinDate)
                                     .ThenByDescending(r => r.UketukeTime)
                                     .ThenByDescending(r => r.RaiinNo)
                                     .ToList();


            Parallel.ForEach(flowSheetModelList, flowSheetModel  =>
            {
                string karteContent = string.Empty;

                if (flowSheetModel.IsNextOrder)
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
                flowSheetModel.ChangeFlowSheet(tagNoValue, karteContent, commentValue, ptId);
            });

            //stopwatch.Stop();
            Console.WriteLine($"End GetListFlowSheet: {ptId} - {stopwatch.ElapsedMilliseconds}");

            _tenantHistory.Dispose();
            _tenantNextOrder.Dispose();
            _tenantKarteInf.Dispose();
            _tenantNextKarteInf.Dispose();
            _tenantTagInf.Dispose();
            _tenantCmtInf.Dispose();

            return flowSheetModelList;
        }


        private List<FlowSheetModel> TodayFlowSheet(int hpId, long ptId)
        {
            var allRaiinInfList = _tenantHistory.RaiinInfs
           .Where(r => r.HpId == hpId && r.PtId == ptId && r.Status >= RaiinState.TempSave && r.IsDeleted == 0)
           .Select(r => new FlowSheetModel(r.SinDate, r.PtId, r.RaiinNo, r.UketukeTime ?? string.Empty, r.SyosaisinKbn, r.Status, false))
           .ToList();

            return allRaiinInfList;
        }

        private List<FlowSheetModel> NextOrderFlowSheet(int hpId, long ptId)
        {
            var rsvkrtOdrInfs = _tenantNextOrder.RsvkrtOdrInfs.Where(r => r.HpId == hpId
                                                                                     && r.PtId == ptId
                                                                                     && r.IsDeleted == DeleteTypes.None);
            var rsvkrtMsts = _tenantNextOrder.RsvkrtMsts.Where(r => r.HpId == hpId
                                                                                        && r.PtId == ptId
                                                                                        && r.IsDeleted == DeleteTypes.None
                                                                                        && r.RsvkrtKbn == 0);
            var groupNextOdr = (
                                    from rsvkrtOdrInf in rsvkrtOdrInfs.AsEnumerable<RsvkrtOdrInf>()
                                    join rsvkrtMst in rsvkrtMsts on new { rsvkrtOdrInf.HpId, rsvkrtOdrInf.PtId, rsvkrtOdrInf.RsvkrtNo }
                                                     equals new { rsvkrtMst.HpId, rsvkrtMst.PtId, rsvkrtMst.RsvkrtNo }
                                    group rsvkrtOdrInf by new { rsvkrtOdrInf.HpId, rsvkrtOdrInf.PtId, rsvkrtOdrInf.RsvDate, rsvkrtOdrInf.RsvkrtNo } into g
                                    select new FlowSheetModel(g.Key.RsvDate, g.Key.PtId, g.Key.RsvkrtNo, string.Empty, -1, 0, true)
                               ).ToList();

            return groupNextOdr;
        }

        private List<RsvkrtKarteInf> RsvkrtKarteInf(int hpId, long ptId)
        {
            var nextKarteList = _tenantNextKarteInf.RsvkrtKarteInfs
             .Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0 && k.Text != null && !string.IsNullOrEmpty(k.Text.Trim()) && k.KarteKbn == 1)
             .ToList();
            return nextKarteList;
        }

        private List<KarteInf> KarteInf(int hpId, long ptId)
        {
            var historyKarteList = _tenantKarteInf.KarteInfs
                  .Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0 && k.Text != null && !string.IsNullOrEmpty(k.Text.Trim()) && k.KarteKbn == 1).ToList();
            return historyKarteList;
        }

        private List<RaiinListTag> RaiinListTag(int hpId, long ptId)
        {
            var tagInfList = _tenantTagInf.RaiinListTags
                .Where(tag => tag.HpId == hpId && tag.PtId == ptId && tag.IsDeleted == 0)
                .ToList();

            return tagInfList;
        }

        private List<RaiinListCmt> RaiinListCmt(int hpId, long ptId)
        {
            var commentList = _tenantCmtInf.RaiinListCmts
                 .Where(k => k.HpId == hpId && k.PtId == ptId && k.IsDeleted == 0 && k.Text != null && !string.IsNullOrEmpty(k.Text.Trim()))
                 .ToList();

            return commentList;
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
                .Select(data => new RaiinListMstModel(data.Mst.GrpId, data.Mst.GrpName ?? string.Empty, data.Mst.SortNo, data.Mst.IsDeleted, data.Detail.Select(d => new RaiinListDetailModel(d.GrpId, d.KbnCd, d.SortNo, d.KbnName ?? string.Empty, d.ColorCd ?? String.Empty, d.IsDeleted)).ToList()))
                .ToList();
            var json = JsonSerializer.Serialize(raiinListMstModelList);
            _cache.StringSet(RaiinListMstCacheKey, json);

            return raiinListMstModelList;
        }

        public List<RaiinListMstModel> GetRaiinListMsts(int hpId)
        {
            var stopwatch = Stopwatch.StartNew();
            var setKbnMstList = new List<RaiinListMstModel>();
            if (!_cache.KeyExists(RaiinListMstCacheKey))
            {
                setKbnMstList = ReloadRaiinListMstCache(hpId);
            }
            else
            {
                setKbnMstList = ReadCacheRaiinListMst();
            }
            Console.WriteLine($"End RaiinListMst - {stopwatch.ElapsedMilliseconds}");
            return setKbnMstList!;
        }

        private List<RaiinListMstModel> ReadCacheRaiinListMst()
        {
            var results = _cache.StringGet(RaiinListMstCacheKey);
            var json = results.AsString();
            var datas = !string.IsNullOrEmpty(json) ? JsonSerializer.Deserialize<List<RaiinListMstModel>>(json) : new();
            return datas ?? new();
        }

        #endregion

        #region HolidayMst
        private List<HolidayDto> ReloadHolidayCache(int hpId)
        {
            var holidayModelList = NoTrackingDataContext.HolidayMsts
                .Where(h => h.HpId == hpId && h.IsDeleted == DeleteTypes.None)
                .Select(h => new HolidayDto(h.SeqNo, h.SinDate, h.HolidayKbn, h.KyusinKbn, h.HolidayName ?? string.Empty))
                .ToList();
            var json = JsonSerializer.Serialize(holidayModelList);
            _cache.StringSet(HolidayMstCacheKey, json);
            return holidayModelList;
        }

        private List<HolidayDto> ReadCacheHolidayMst()
        {
            var results = _cache.StringGet(HolidayMstCacheKey);
            var json = results.AsString();
            var datas = !string.IsNullOrEmpty(json) ? JsonSerializer.Deserialize<List<HolidayDto>>(json) : new();
            return datas ?? new();
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
            var result =  TrackingDataContext.SaveChanges() > 0;
            if (result)
            {
                ReloadHolidayCache(holiday.HpId);
            }
            return result;
        }

        public List<HolidayDto> GetHolidayMst(int hpId, int holidayFrom, int holidayTo)
        {
            var holidayMstList = new List<HolidayDto>();
            if (!_cache.KeyExists(HolidayMstCacheKey))
            {
                holidayMstList = ReloadHolidayCache(hpId);
            }
            else
            {
                holidayMstList = ReadCacheHolidayMst();
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
            var stopwatch = Stopwatch.StartNew();
            var raiinListInfs =
               (
                  from raiinListInf in NoTrackingDataContext.RaiinListInfs.Where(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo != 0)
                  join raiinListMst in NoTrackingDataContext.RaiinListDetails.Where(r => r.HpId == hpId && r.IsDeleted == DeleteTypes.None)
                  on new { raiinListInf.GrpId, raiinListInf.KbnCd } equals new { raiinListMst.GrpId, raiinListMst.KbnCd }
                  select new { raiinListInf.SinDate, raiinListInf.RaiinNo, raiinListInf.GrpId, raiinListInf.KbnCd, raiinListInf.RaiinListKbn, raiinListMst.KbnName, raiinListMst.ColorCd }
               ).ToList();

            var result = raiinListInfs
                .GroupBy(r => r.RaiinNo)
                .ToDictionary(g => g.Key, g => g.Select(r => new RaiinListInfModel(r.RaiinNo, r.GrpId, r.KbnCd, r.RaiinListKbn, r.KbnName, r.ColorCd)).ToList());
            Console.WriteLine($"End RaiinListInf Today - {ptId} - {stopwatch.ElapsedMilliseconds}");
            return result;
        }

        public Dictionary<int, List<RaiinListInfModel>> GetRaiinListInfForNextOrder(int hpId, long ptId)
        {
            var stopwatch = Stopwatch.StartNew();
            var raiinListInfs =
               (
                  from raiinListInf in NoTrackingDataContext.RaiinListInfs.Where(r => r.HpId == hpId && r.PtId == ptId && r.RaiinNo == 0)
                  join raiinListMst in NoTrackingDataContext.RaiinListDetails.Where(r => r.HpId == hpId && r.IsDeleted == DeleteTypes.None)
                  on new { raiinListInf.GrpId, raiinListInf.KbnCd } equals new { raiinListMst.GrpId, raiinListMst.KbnCd }
                  select new { raiinListInf.SinDate, raiinListInf.RaiinNo, raiinListInf.GrpId, raiinListInf.KbnCd, raiinListInf.RaiinListKbn, raiinListMst.KbnName, raiinListMst.ColorCd }
               ).ToList();

            var result = raiinListInfs
                .GroupBy(r => r.SinDate)
                .ToDictionary(g => g.Key, g => g.Select(r => new RaiinListInfModel(r.RaiinNo, r.GrpId, r.KbnCd, r.RaiinListKbn, r.KbnName, r.ColorCd)).ToList());
            Console.WriteLine($"End RaiinListInf NextOrder - {ptId} - {stopwatch.ElapsedMilliseconds}");
            return result;
        }

        public List<(int date, string tooltip)> GetTooltip(int hpId, long ptId, int sinDate, int startDate, int endDate, bool isAll)
        {
            List<int> dates = new();
            for (int i = startDate; i <= endDate; i++)
            {
                dates.Add(i);
            }

            List<(int, string)> result = new();
            var raiinInfs = isAll ? new() : NoTrackingDataContext.RaiinInfs
                .Where(r => r.HpId == hpId && (isAll || r.PtId == ptId) && r.IsDeleted == DeleteTypes.None && r.SinDate >= startDate && r.SinDate <= endDate && r.Status >= RaiinState.TempSave)
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

                if (!isAll)
                {
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
