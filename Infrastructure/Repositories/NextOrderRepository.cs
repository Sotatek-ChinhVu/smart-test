﻿using Domain.Constant;
using Domain.Models.NextOrder;
using Domain.Models.OrdInfDetails;
using Domain.Models.RaiinKubunMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Repositories
{
    public class NextOrderRepository : RepositoryBase, INextOrderRepository
    {
        private readonly IAmazonS3Service? _amazonS3Service;
        private readonly AmazonS3Options? _options;
        private readonly string key;
        private readonly IDatabase _cache;
        private readonly IConfiguration _configuration;

        public NextOrderRepository(ITenantProvider tenantProvider, IAmazonS3Service amazonS3Service, IConfiguration configuration, IOptions<AmazonS3Options> optionsAccessor) : base(tenantProvider)
        {
            key = GetDomainKey() + CacheKeyConstant.GetNextOrderList;
            _configuration = configuration;
            GetRedis();
            _cache = RedisConnectorHelper.Connection.GetDatabase();
            _amazonS3Service = amazonS3Service;
            _options = optionsAccessor.Value;
        }

        public NextOrderRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
            key = string.Empty;
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

        public List<RsvkrtByomeiModel> GetByomeis(int hpId, long ptId, long rsvkrtNo, int rsvkrtKbn)
        {
            List<RsvkrtByomei> byomeis = new();
            if (rsvkrtKbn == 0)
            {
                byomeis = NoTrackingDataContext.RsvkrtByomeis.Where(b => b.HpId == hpId && b.PtId == ptId && b.RsvkrtNo == rsvkrtNo && b.IsDeleted == DeleteTypes.None).ToList();
            }
            List<PrefixSuffixModel> prefixSuffixModels = new();
            foreach (var item in byomeis)
            {
                prefixSuffixModels.AddRange(SyusyokuCdToList(item));
            }

            var byomeiModels = byomeis.Select(b => ConvertByomeiToModel(b)).ToList();

            return byomeiModels;
        }

        public RsvkrtKarteInfModel GetKarteInf(int hpId, long ptId, long rsvkrtNo)
        {

            var karteInf = NoTrackingDataContext.RsvkrtKarteInfs.FirstOrDefault(k => k.HpId == hpId && k.PtId == ptId && k.RsvkrtNo == rsvkrtNo && k.IsDeleted == DeleteTypes.None);

            var karteModel = ConvertKarteInfToModel(karteInf ?? new());

            return karteModel;
        }

        public List<RsvkrtOrderInfModel> GetOrderInfs(int hpId, long ptId, long rsvkrtNo, int sinDate, int userId)
        {
            var orderInfs = NoTrackingDataContext.RsvkrtOdrInfs.Where(o => o.HpId == hpId && o.PtId == ptId && o.RsvkrtNo == rsvkrtNo && o.IsDeleted == DeleteTypes.None).ToList();
            var orderInfDetails = NoTrackingDataContext.RsvkrtOdrInfDetails.Where(o => o.HpId == hpId && o.PtId == ptId && o.RsvkrtNo == rsvkrtNo).ToList();

            var itemCds = orderInfDetails.Select(od => od.ItemCd ?? string.Empty);
            var ipnCds = orderInfDetails.Select(od => od.IpnCd ?? string.Empty);
            var sinKouiKbns = orderInfDetails.Select(od => od.SinKouiKbn);
            var tenMsts = NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && (t.StartDate <= sinDate && t.EndDate >= sinDate) && (itemCds != null && itemCds.Contains(t.ItemCd))).ToList();
            var kensaMsts = NoTrackingDataContext.KensaMsts.Where(t => t.HpId == hpId).ToList();
            var yakkas = NoTrackingDataContext.IpnMinYakkaMsts.Where(t => t.HpId == hpId && (t.StartDate <= sinDate && t.EndDate >= sinDate) && (ipnCds != null && ipnCds.Contains(t.IpnNameCd))).ToList();
            var ipnKasanExcludes = NoTrackingDataContext.ipnKasanExcludes.Where(t => t.HpId == hpId && (t.StartDate <= sinDate && t.EndDate >= sinDate)).ToList();
            var ipnKasanExcludeItems = NoTrackingDataContext.ipnKasanExcludeItems.Where(t => t.HpId == hpId && (t.StartDate <= sinDate && t.EndDate >= sinDate)).ToList();
            var ipnKansanMsts = NoTrackingDataContext.IpnKasanMsts.Where(ipn => (ipnCds != null && ipnCds.Contains(ipn.IpnNameCd)) && ipn.HpId == hpId && ipn.StartDate <= sinDate && ipn.IsDeleted == 0).ToList();
            var listYohoSets = NoTrackingDataContext.YohoSetMsts.Where(y => y.HpId == hpId && y.IsDeleted == 0 && y.UserId == userId).ToList();
            var itemCdYohos = listYohoSets?.Select(od => od.ItemCd ?? string.Empty);

            var tenMstYohos = NoTrackingDataContext.TenMsts.Where(t => t.HpId == hpId && t.IsNosearch == 0 && t.StartDate <= sinDate && t.EndDate >= sinDate && (sinKouiKbns != null && sinKouiKbns.Contains(t.SinKouiKbn)) && (itemCdYohos != null && itemCdYohos.Contains(t.ItemCd))).ToList();

            var checkKensaIrai = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 0);
            var kensaIrai = checkKensaIrai?.Val ?? 0;
            var checkKensaIraiCondition = NoTrackingDataContext.SystemConfs.FirstOrDefault(p => p.GrpCd == 2019 && p.GrpEdaNo == 1);
            var kensaIraiCondition = checkKensaIraiCondition?.Val ?? 0;

            var oderInfModels = orderInfs.Select(o => ConvertOrderInfToModel(o, orderInfDetails, tenMsts, kensaMsts, yakkas, ipnKasanExcludes, ipnKasanExcludeItems, ipnKansanMsts, listYohoSets ?? new(), tenMstYohos, kensaIrai, kensaIraiCondition)).ToList();
            return oderInfModels;
        }

        public List<RsvkrtOrderInfModel> GetCheckOrderInfs(int hpId, long ptId)
        {
            var orderInfs = NoTrackingDataContext.RsvkrtOdrInfs.Where(o => o.HpId == hpId && o.PtId == ptId && o.IsDeleted == DeleteTypes.None).ToList();

            var oderInfModels = orderInfs.Select(o => ConvertOrderInfToModel(o)).ToList();

            return oderInfModels;
        }

        public long Upsert(int userId, int hpId, long ptId, List<NextOrderModel> nextOrderModels)
        {
            #region get common data by hpId and ptId
            // get all data RsvkrtMst
            var rsvkrtMstList = TrackingDataContext.RsvkrtMsts.Where(item => item.HpId == hpId && item.PtId == ptId).ToList();
            var rsvkrtByomeiList = TrackingDataContext.RsvkrtByomeis.Where(item => item.HpId == hpId && item.PtId == ptId).ToList();
            var rsvkrtKarteInfList = TrackingDataContext.RsvkrtKarteInfs.Where(item => item.HpId == hpId && item.PtId == ptId).ToList();
            var rsvkrtOdrInfList = TrackingDataContext.RsvkrtOdrInfs.Where(item => item.HpId == hpId && item.PtId == ptId).ToList();
            var kouiKbnMstList = NoTrackingDataContext.KouiKbnMsts.ToList();

            // Get Raiin List Koui
            var raiinListMstQuery = NoTrackingDataContext.RaiinListMsts.Where(item => item.IsDeleted == 0);
            var raiinListDetailQuery = NoTrackingDataContext.RaiinListDetails.Where(item => item.IsDeleted == 0);
            var raiinListKouiQuery = NoTrackingDataContext.RaiinListKouis.Where(item => item.IsDeleted == 0);
            var raiinListKouiList = from raiinListKoui in raiinListKouiQuery
                                    join raiinListDetail in raiinListDetailQuery on new { raiinListKoui.GrpId, raiinListKoui.KbnCd }
                                                                                 equals new { raiinListDetail.GrpId, raiinListDetail.KbnCd }
                                    join raiinListMst in raiinListMstQuery on new { raiinListKoui.GrpId }
                                                                           equals new { raiinListMst.GrpId }
                                    select new { raiinListKoui };

            List<RaiinListKoui> raiinListKouis = raiinListKouiList.Select(item => item.raiinListKoui).ToList();

            // Get Raiin List Item
            var raiinListItemQuery = NoTrackingDataContext.RaiinListItems.Where(item => item.IsExclude == 0
                                                                                        && item.IsDeleted == 0);
            var raiinListItemList = from raiinListItem in raiinListItemQuery
                                    join raiinListDetail in raiinListDetailQuery on new { raiinListItem.GrpId, raiinListItem.KbnCd }
                                                                                 equals new { raiinListDetail.GrpId, raiinListDetail.KbnCd }
                                    join raiinListMst in raiinListMstQuery on new { raiinListItem.GrpId }
                                                                           equals new { raiinListMst.GrpId }
                                    select new { raiinListItem };

            List<RaiinListItem> raiinListItems = raiinListItemList.Select(item => item.raiinListItem).ToList();
            #endregion get common data by hpId and ptId

            long rsvkrtNo = 0;
            long ptNum = GetPtNum(hpId, ptId);
            var odrInfs = new List<RsvkrtOrderInfModel>();
            var isDeletedRsvKrtDate = nextOrderModels.Where(n => n.IsDeleted == DeleteTypes.Deleted).Select(n => n.RsvDate).ToList();
            foreach (var nextOrderModel in nextOrderModels)
            {
                var oldNextOrder = rsvkrtMstList.FirstOrDefault(m => m.RsvkrtNo == nextOrderModel.RsvkrtNo);
                odrInfs.AddRange(nextOrderModel.RsvkrtOrderInfs);
                var maxRpNo = GetMaxRpNo(hpId, ptId);
                var seqNo = GetMaxSeqNo(ptId, hpId, nextOrderModel.RsvkrtNo);
                if (nextOrderModel.IsDeleted == DeleteTypes.Deleted || nextOrderModel.IsDeleted == DeleteTypes.Confirm)
                {
                    if (oldNextOrder != null && oldNextOrder.IsDeleted == DeleteTypes.None)
                    {
                        oldNextOrder.IsDeleted = nextOrderModel.IsDeleted;
                        foreach (var item in nextOrderModel.RsvkrtOrderInfs.Where(o => o.IsDeleted == DeleteTypes.Deleted || o.IsDeleted == DeleteTypes.Confirm))
                        {
                            var orderInf = TrackingDataContext.RsvkrtOdrInfs.FirstOrDefault(o => o.HpId == item.HpId && o.PtId == item.PtId && item.IsDeleted == DeleteTypes.None && o.RsvkrtNo == item.RsvkrtNo);
                            if (orderInf != null)
                            {
                                orderInf.IsDeleted = item.IsDeleted;
                                orderInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                orderInf.UpdateId = userId;
                            }
                        }
                    }
                }
                else
                {

                    if (oldNextOrder != null)
                    {
                        if (oldNextOrder.IsDeleted == DeleteTypes.None)
                        {
                            oldNextOrder.RsvkrtKbn = nextOrderModel.RsvkrtKbn;
                            oldNextOrder.RsvDate = nextOrderModel.RsvDate;
                            oldNextOrder.RsvName = nextOrderModel.RsvName;
                            oldNextOrder.SortNo = nextOrderModel.SortNo;
                            oldNextOrder.IsDeleted = nextOrderModel.IsDeleted;
                            oldNextOrder.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            oldNextOrder.UpdateId = userId;
                            rsvkrtNo = oldNextOrder.RsvkrtNo;
                            UpsertByomei(ref rsvkrtByomeiList, userId, nextOrderModel.RsvkrtByomeis, rsvkrtNo);
                            UpsertKarteInf(ref rsvkrtKarteInfList, userId, seqNo, nextOrderModel.RsvkrtKarteInf, rsvkrtNo);
                            UpsertOrderInf(ref rsvkrtOdrInfList, userId, maxRpNo, nextOrderModel.RsvkrtOrderInfs, rsvkrtNo, nextOrderModel.RsvDate);
                        }

                    }
                    else
                    {
                        var checkExistRsvkrtOrder = rsvkrtMstList.Any(x => x.RsvkrtKbn == 0 &&
                                                                         x.RsvDate == nextOrderModel.RsvDate && x.IsDeleted == DeleteTypes.None);

                        if (checkExistRsvkrtOrder && !isDeletedRsvKrtDate.Contains(nextOrderModel.RsvDate)) continue;

                        var nextOrderEntity = ConvertModelToRsvkrtNextOrder(userId, nextOrderModel, oldNextOrder);
                        TrackingDataContext.RsvkrtMsts.Add(nextOrderEntity);
                        TrackingDataContext.SaveChanges();

                        // add new rsvkMst to rsvkMstList
                        rsvkrtMstList.Add(nextOrderEntity);

                        rsvkrtNo = nextOrderEntity.RsvkrtNo;
                        UpsertByomei(ref rsvkrtByomeiList, userId, nextOrderModel.RsvkrtByomeis, rsvkrtNo);
                        UpsertKarteInf(ref rsvkrtKarteInfList, userId, seqNo, nextOrderModel.RsvkrtKarteInf, rsvkrtNo);
                        UpsertOrderInf(ref rsvkrtOdrInfList, userId, maxRpNo, nextOrderModel.RsvkrtOrderInfs, rsvkrtNo, nextOrderModel.RsvDate);
                    }
                    if (oldNextOrder?.IsDeleted == DeleteTypes.None)
                    {
                        SaveFileNextOrder(hpId, ptId, ptNum, rsvkrtNo, nextOrderModel);
                        SaveNextOrderRaiinListInf(userId, odrInfs, kouiKbnMstList, raiinListKouis, raiinListItems);
                    }
                }
            }
            TrackingDataContext.SaveChanges();

            // delete cache key
            string finalKey = key + ptId;
            if (_cache.KeyExists(finalKey))
            {
                _cache.KeyDelete(finalKey);
            }
            return rsvkrtNo;
        }

        public void SaveNextOrderRaiinListInf(int userId, List<RsvkrtOrderInfModel> nextOrderModels, List<KouiKbnMst> kouiKbnMst, List<RaiinListKoui> raiinListKouis, List<RaiinListItem> raiinListItems)
        {
            // Define Added RaiinListInf
            List<RaiinListInf> raiinListInfList = new();

            foreach (var odrInf in nextOrderModels)
            {
                HashSet<int> deleteKouiSet = new();
                HashSet<int> currentKouiSet = new();
                HashSet<string> deleteItemCdSet = new();
                HashSet<string> currentItemCdSet = new();

                int hpId = odrInf.HpId;
                long ptId = odrInf.PtId;
                int sinDate = odrInf.RsvDate;

                // Get Raiin List Inf
                List<RaiinListInf> raiinListInfs = TrackingDataContext.RaiinListInfs
                                                    .Where(item => item.HpId == hpId
                                                                    && item.RaiinNo == 0
                                                                    && item.PtId == ptId
                                                                    && item.SinDate == sinDate).ToList();

                foreach (var nextOdrDetail in odrInf.OrdInfDetails)
                {
                    if (odrInf.IsDeleted != 0)
                    {
                        deleteKouiSet.Add(nextOdrDetail.SinKouiKbn);
                        deleteItemCdSet.Add(nextOdrDetail.ItemCd);
                        continue;
                    }
                    currentKouiSet.Add(nextOdrDetail.SinKouiKbn);
                    currentItemCdSet.Add(nextOdrDetail.ItemCd);
                }
                // Delete with SinKouiKbn
                foreach (int koui in deleteKouiSet.ToArray())
                {
                    if (currentKouiSet.Contains(koui))
                    {
                        continue;
                    }
                    // Get KouiKbnMst
                    var kouiMst = kouiKbnMst?.Find(item => item.KouiKbn1 == koui || item.KouiKbn2 == koui);
                    if (kouiMst == null) continue;

                    // Get List RaiinListKoui contains koui 
                    List<RaiinListKoui> kouiItemList = raiinListKouis.FindAll(item => item.KouiKbnId == kouiMst.KouiKbnId);
                    foreach (RaiinListKoui kouiItem in kouiItemList)
                    {
                        var raiinListInf = raiinListInfs.Find(item => item.GrpId == kouiItem.GrpId && item.KbnCd == kouiItem.KbnCd);
                        if (raiinListInf != null)
                        {
                            TrackingDataContext.RaiinListInfs.Remove(raiinListInf);
                        }
                    }
                }

                // Delete with ItemCd
                foreach (string itemCd in deleteItemCdSet.ToArray())
                {
                    if (currentItemCdSet.Contains(itemCd))
                    {
                        continue;
                    }
                    List<RaiinListItem> itemCdList = raiinListItems.FindAll(item => item.ItemCd == itemCd);
                    foreach (RaiinListItem raiinListItem in itemCdList)
                    {
                        var raiinListInf = raiinListInfs?.Find(item => item.GrpId == raiinListItem.GrpId && item.KbnCd == raiinListItem.KbnCd);
                        if (raiinListInf != null)
                        {
                            TrackingDataContext.RaiinListInfs.Remove(raiinListInf);
                        }
                    }
                }
                // Add or Update with ItemCd
                foreach (string itemCd in currentItemCdSet.ToArray())
                {
                    List<RaiinListItem> itemCdList = raiinListItems.FindAll(item => item.ItemCd == itemCd);
                    foreach (RaiinListItem raiinListItem in itemCdList)
                    {
                        var raiinListInf = raiinListInfs?.Find(item => item.GrpId == raiinListItem.GrpId);
                        if (raiinListInf == null)
                        {
                            // Check contains with grpId
                            if (raiinListInfList.Find(item => item.RaiinNo == 0
                                                             && item.SinDate == sinDate
                                                             && item.GrpId == raiinListItem.GrpId) == null)
                            {
                                // create new 
                                raiinListInf = new RaiinListInf()
                                {
                                    HpId = hpId,
                                    PtId = ptId,
                                    RaiinNo = 0,
                                    SinDate = sinDate,
                                    GrpId = raiinListItem.GrpId,
                                    KbnCd = raiinListItem.KbnCd,
                                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                    UpdateId = userId,
                                };
                                raiinListInfList.Add(raiinListInf);
                            }
                        }
                        else
                        {
                            // update
                            raiinListInf.KbnCd = raiinListItem.KbnCd;
                            raiinListInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            raiinListInf.UpdateId = userId;
                        }
                    }
                }

                // Add or Update with SinKouiKbn
                foreach (int koui in currentKouiSet.ToArray())
                {
                    var kouiMst = kouiKbnMst?.Find(item => item.KouiKbn1 == koui);
                    if (kouiMst == null) continue;

                    List<RaiinListKoui> kouiItemList = raiinListKouis.FindAll(item => item.KouiKbnId == kouiMst.KouiKbnId);
                    foreach (RaiinListKoui kouiItem in kouiItemList)
                    {
                        var raiinListInf = raiinListInfs?.Find(item => item.GrpId == kouiItem.GrpId);
                        if (raiinListInf == null)
                        {
                            // Check contains with grpId
                            if (raiinListInfList.Find(item => item.RaiinNo == 0
                                                             && item.SinDate == sinDate
                                                             && item.GrpId == kouiItem.GrpId) == null)
                            {
                                // create new 
                                raiinListInf = new RaiinListInf()
                                {
                                    HpId = hpId,
                                    PtId = ptId,
                                    RaiinNo = 0,
                                    SinDate = sinDate,
                                    GrpId = kouiItem.GrpId,
                                    KbnCd = kouiItem.KbnCd,
                                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                                    UpdateId = userId
                                };
                                raiinListInfList.Add(raiinListInf);
                            }
                        }
                        else
                        {
                            // update
                            raiinListInf.KbnCd = kouiItem.KbnCd;
                            raiinListInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            raiinListInf.UpdateId = userId;
                        }
                    }
                }
            }

            TrackingDataContext.RaiinListInfs.AddRange(raiinListInfList);
            TrackingDataContext.SaveChanges();
        }

        private void UpsertOrderInf(ref List<RsvkrtOdrInf> rsvkrtOrderInfEntityList, int userId, long maxRpNo, List<RsvkrtOrderInfModel> rsvkrtOrderInfModels, long rsvkrtNo = 0, int rsvDate = 0)
        {
            var oldOrderInfs = rsvkrtOrderInfEntityList.Where(o => o.RsvkrtNo == rsvkrtNo).ToList();

            foreach (var orderInf in rsvkrtOrderInfModels)
            {
                var oldOrderInf = oldOrderInfs.FirstOrDefault(o => o.HpId == orderInf.HpId && o.PtId == orderInf.PtId && o.RsvkrtNo == rsvkrtNo && o.RpNo == orderInf.RpNo && o.RpEdaNo == orderInf.RpEdaNo && o.IsDeleted == DeleteTypes.None);
                if (orderInf.IsDeleted == DeleteTypes.Deleted || orderInf.IsDeleted == DeleteTypes.Confirm)
                {
                    if (oldOrderInf != null && oldOrderInf.IsDeleted == DeleteTypes.None)
                    {
                        oldOrderInf.IsDeleted = orderInf.IsDeleted;
                        oldOrderInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        oldOrderInf.CreateId = userId;
                    }
                }
                else
                {
                    RsvkrtOdrInf? orderInfEntity = null;
                    if (oldOrderInf != null)
                    {
                        if (oldOrderInf.IsDeleted == DeleteTypes.None)
                        {
                            oldOrderInf.IsDeleted = DeleteTypes.Deleted;
                            oldOrderInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            oldOrderInf.CreateId = userId;
                            orderInf.ChangeDate(rsvDate);
                            orderInfEntity = ConvertModelToRsvkrtOrderInf(userId, oldOrderInf.RpNo, orderInf, oldOrderInf.RsvkrtNo, oldOrderInf.RpEdaNo + 1);
                            TrackingDataContext.RsvkrtOdrInfs.Add(orderInfEntity);
                            var orderInfDetailEntity = orderInf.OrdInfDetails.Select(od => ConvertModelToRsvkrtOrderInfDetail(oldOrderInf.RpNo, od, oldOrderInf.RsvkrtNo, oldOrderInf.RpEdaNo + 1));
                            TrackingDataContext.RsvkrtOdrInfDetails.AddRange(orderInfDetailEntity);
                        }
                    }
                    else
                    {
                        maxRpNo++;
                        orderInf.ChangeDate(rsvDate);
                        orderInfEntity = ConvertModelToRsvkrtOrderInf(userId, maxRpNo, orderInf, rsvkrtNo);
                        TrackingDataContext.RsvkrtOdrInfs.Add(orderInfEntity);
                        var orderInfDetailEntity = orderInf.OrdInfDetails.Select(od => ConvertModelToRsvkrtOrderInfDetail(orderInfEntity.RpNo, od, rsvkrtNo));
                        TrackingDataContext.RsvkrtOdrInfDetails.AddRange(orderInfDetailEntity);
                    }

                    // add orderInfEntity to rsvkrtOrderInfEntityList
                    if (orderInfEntity != null)
                    {
                        rsvkrtOrderInfEntityList.Add(orderInfEntity);
                    }
                }
            }
        }

        /// <summary>
        /// UpsertKarteInf item
        /// </summary>
        /// <param name="rsvkrtKarteInfEntityList"></param>
        /// <param name="userId"></param>
        /// <param name="seqNo"></param>
        /// <param name="karteInf"></param>
        /// <param name="rsvkrtNo"></param>
        private void UpsertKarteInf(ref List<RsvkrtKarteInf> rsvkrtKarteInfEntityList, int userId, long seqNo, RsvkrtKarteInfModel karteInf, long rsvkrtNo = 0)
        {
            var oldKarteInf = rsvkrtKarteInfEntityList.FirstOrDefault(o => o.RsvkrtNo == rsvkrtNo);
            if (karteInf.IsDeleted == DeleteTypes.Deleted || karteInf.IsDeleted == DeleteTypes.Confirm)
            {
                if (oldKarteInf != null && oldKarteInf.IsDeleted == DeleteTypes.None)
                {
                    oldKarteInf.IsDeleted = karteInf.IsDeleted != DeleteTypes.Confirm ? DeleteTypes.Deleted : karteInf.IsDeleted;
                    oldKarteInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    oldKarteInf.CreateId = userId;
                }
            }
            else
            {
                RsvkrtKarteInf? karteInfEntity = null;
                if (oldKarteInf != null)
                {
                    if (oldKarteInf.IsDeleted == DeleteTypes.None)
                    {
                        seqNo++;
                        oldKarteInf.IsDeleted = karteInf.IsDeleted != DeleteTypes.Confirm ? DeleteTypes.Deleted : karteInf.IsDeleted;
                        oldKarteInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        oldKarteInf.CreateId = userId;
                        karteInfEntity = ConvertModelToRsvkrtKarteInf(userId, karteInf, karteInf.RsvkrtNo, seqNo);
                        TrackingDataContext.Add(karteInfEntity);
                    }
                }
                else
                {
                    karteInfEntity = ConvertModelToRsvkrtKarteInf(userId, karteInf, rsvkrtNo);
                    TrackingDataContext.Add(karteInfEntity);
                }

                // add karteInfEntity to rsvkrtKarteInfEntityList
                if (karteInfEntity != null)
                {
                    rsvkrtKarteInfEntityList.Add(karteInfEntity);
                }
            }
        }

        private long GetMaxSeqNo(long ptId, int hpId, long rsvkrtNo)
        {
            var karteInf = NoTrackingDataContext.RsvkrtKarteInfs.Where(k => k.HpId == hpId && k.KarteKbn == 1 && k.PtId == ptId && k.RsvkrtNo == rsvkrtNo).OrderByDescending(k => k.SeqNo).FirstOrDefault();

            return karteInf != null ? karteInf.SeqNo : 0;
        }

        /// <summary>
        /// UpsertByomei item
        /// </summary>
        /// <param name="rsvkrtByomeiEntityList"></param>
        /// <param name="userId"></param>
        /// <param name="byomeis"></param>
        /// <param name="rsvkrtNo"></param>
        private void UpsertByomei(ref List<RsvkrtByomei> rsvkrtByomeiEntityList, int userId, List<RsvkrtByomeiModel> byomeis, long rsvkrtNo = 0)
        {
            var allOldByomeis = rsvkrtByomeiEntityList.Where(o => o.RsvkrtNo == rsvkrtNo).ToList();
            foreach (var byomei in byomeis)
            {
                var oldByomei = allOldByomeis.FirstOrDefault(item => item.HpId == byomei.HpId
                                                                  && item.PtId == byomei.PtId
                                                                  && item.RsvkrtNo == rsvkrtNo
                                                                  && item.SeqNo == byomei.SeqNo);

                if (byomei.IsDeleted == DeleteTypes.Deleted)
                {
                    if (oldByomei != null && oldByomei.IsDeleted == DeleteTypes.None)
                    {
                        oldByomei.IsDeleted = DeleteTypes.Deleted;
                        oldByomei.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        oldByomei.CreateId = userId;
                    }
                }
                else
                {
                    if (oldByomei != null)
                    {
                        if (oldByomei.IsDeleted == DeleteTypes.None)
                        {
                            oldByomei.ByomeiCd = byomei.ByomeiCd;
                            oldByomei.SyusyokuCd1 = byomei.PrefixSuffixList.FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd2 = byomei.PrefixSuffixList.Skip(1).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd3 = byomei.PrefixSuffixList.Skip(2).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd4 = byomei.PrefixSuffixList.Skip(3).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd5 = byomei.PrefixSuffixList.Skip(4).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd6 = byomei.PrefixSuffixList.Skip(5).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd7 = byomei.PrefixSuffixList.Skip(6).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd8 = byomei.PrefixSuffixList.Skip(7).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd9 = byomei.PrefixSuffixList.Skip(8).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd10 = byomei.PrefixSuffixList.Skip(9).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd11 = byomei.PrefixSuffixList.Skip(10).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd12 = byomei.PrefixSuffixList.Skip(11).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd13 = byomei.PrefixSuffixList.Skip(12).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd14 = byomei.PrefixSuffixList.Skip(13).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd15 = byomei.PrefixSuffixList.Skip(14).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd16 = byomei.PrefixSuffixList.Skip(15).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd17 = byomei.PrefixSuffixList.Skip(16).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd18 = byomei.PrefixSuffixList.Skip(17).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd19 = byomei.PrefixSuffixList.Skip(18).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd20 = byomei.PrefixSuffixList.Skip(19).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.SyusyokuCd21 = byomei.PrefixSuffixList.Skip(20).FirstOrDefault()?.Code ?? string.Empty;
                            oldByomei.Byomei = byomei.Byomei;
                            oldByomei.SyobyoKbn = byomei.SyobyoKbn;
                            oldByomei.SikkanKbn = byomei.SikkanKbn;
                            oldByomei.NanbyoCd = byomei.NanbyoCd;
                            oldByomei.HosokuCmt = byomei.HosokuCmt;
                            oldByomei.IsNodspKarte = byomei.IsNodspKarte;
                            oldByomei.IsNodspRece = byomei.IsNodspRece;
                            oldByomei.IsDeleted = byomei.IsDeleted;
                            oldByomei.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            oldByomei.UpdateId = userId;
                        }
                    }
                    else
                    {
                        var orderInfEntity = ConvertModelToRsvkrtByomei(userId, byomei, rsvkrtNo);
                        TrackingDataContext.Add(orderInfEntity);

                        // add new orderInfEntity to rsvkrtByomeiEntityList
                        rsvkrtByomeiEntityList.Add(orderInfEntity);
                    }
                }
            }
        }

        private RsvkrtByomeiModel ConvertByomeiToModel(RsvkrtByomei byomei)
        {
            //prefix and suffix
            var byomeiMstMain = NoTrackingDataContext.ByomeiMsts.FirstOrDefault(item => byomei.ByomeiCd == item.ByomeiCd) ?? new ByomeiMst();

            return new RsvkrtByomeiModel(
                    byomei.Id,
                    byomei.HpId,
                    byomei.PtId,
                    byomei.RsvkrtNo,
                    byomei.SeqNo,
                    byomei.ByomeiCd ?? string.Empty,
                    byomei.Byomei ?? string.Empty,
                    byomei.SyobyoKbn,
                    byomei.SikkanKbn,
                    byomei.NanbyoCd,
                    byomei.HosokuCmt ?? string.Empty,
                    byomei.IsNodspRece,
                    byomei.IsNodspKarte,
                    byomei.IsDeleted,
                    SyusyokuCdToList(byomei),
                    byomeiMstMain?.Icd101 ?? string.Empty,
                    byomeiMstMain?.Icd1012013 ?? string.Empty,
                    byomeiMstMain?.Icd1012013 ?? string.Empty,
                    byomeiMstMain?.Icd1022013 ?? string.Empty
                );
        }

        private RsvkrtKarteInfModel ConvertKarteInfToModel(RsvkrtKarteInf karteInf)
        {
            return new RsvkrtKarteInfModel(
                    karteInf.HpId,
                    karteInf.PtId,
                    karteInf.RsvDate,
                    karteInf.RsvkrtNo,
                    karteInf.SeqNo,
                    karteInf.Text ?? string.Empty,
                    karteInf.RichText == null ? string.Empty : Encoding.UTF8.GetString(karteInf.RichText),
                    karteInf.IsDeleted
                );
        }

        private RsvkrtOrderInfModel ConvertOrderInfToModel(RsvkrtOdrInf odrInf, List<RsvkrtOdrInfDetail> odrInfDetails, List<TenMst> tenMsts, List<KensaMst> kensaMsts, List<IpnMinYakkaMst> yakkas, List<IpnKasanExclude> ipnKasanExcludes, List<IpnKasanExcludeItem> ipnKasanExcludeItems, List<IpnKasanMst> ipnKasanMsts, List<YohoSetMst> yohoSetMsts, List<TenMst> tenMstYohos, double kensaIrai, double kensaIraiCondition)
        {

            odrInfDetails = odrInfDetails.Where(od => od.RpNo == odrInf.RpNo && od.RpEdaNo == odrInf.RpEdaNo).ToList();
            int index = 0;
            var odrInfDetailModels = new List<RsvKrtOrderInfDetailModel>();
            var obj = new object();
            Parallel.ForEach(odrInfDetails, odrInfDetail =>
            {
                var tenMst = tenMsts.FirstOrDefault(t => t.ItemCd == odrInfDetail.ItemCd) ?? new();
                var yakka = yakkas.FirstOrDefault(y => y.IpnNameCd == odrInfDetail.IpnCd) ?? new();
                var isGetPriceInYakka = IsGetPriceInYakka(tenMst, ipnKasanExcludes, ipnKasanExcludeItems);
                var usage = odrInfDetails.FirstOrDefault(d => d.YohoKbn == 1 || d.ItemCd == ItemCdConst.TouyakuChozaiNaiTon || d.ItemCd == ItemCdConst.TouyakuChozaiGai);
                var bunkatuKoui = 0;
                if (odrInfDetail.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                {
                    bunkatuKoui = usage?.SinKouiKbn ?? 0;
                }
                var alternationIndex = index % 2;
                var kensaMst = tenMst == null ? null : kensaMsts.FirstOrDefault(k => k.KensaItemCd == tenMst.KensaItemCd && k.KensaItemSeqNo == tenMst.KensaItemSeqNo) ?? new();
                int kensaGaichu = GetKensaGaichu(odrInfDetail, tenMst ?? new(), odrInf.InoutKbn, odrInf.OdrKouiKbn, kensaMst, (int)kensaIraiCondition, (int)kensaIrai);
                var yohoSet = GetListYohoSetMstModelByUserID(yohoSetMsts ?? new List<YohoSetMst>(), tenMstYohos?.Where(t => t.SinKouiKbn == odrInfDetail.SinKouiKbn)?.ToList() ?? new List<TenMst>());
                var kasan = ipnKasanMsts?.FirstOrDefault(ipn => ipn.IpnNameCd == odrInfDetail.IpnCd);

                var odrInfDetailModel = new RsvKrtOrderInfDetailModel(
                         odrInfDetail.HpId,
                         odrInfDetail.PtId,
                         odrInfDetail.RsvkrtNo,
                         odrInfDetail.RpNo,
                         odrInfDetail.RpEdaNo,
                         odrInfDetail.RowNo,
                         odrInfDetail.RsvDate,
                         odrInfDetail.SinKouiKbn,
                         odrInfDetail.ItemCd ?? string.Empty,
                         odrInfDetail.ItemName ?? string.Empty,
                         odrInfDetail.Suryo,
                         odrInfDetail.UnitName ?? string.Empty,
                         odrInfDetail.UnitSbt,
                         odrInfDetail.TermVal,
                         odrInfDetail.KohatuKbn,
                         odrInfDetail.SyohoKbn,
                         odrInfDetail.SyohoLimitKbn,
                         odrInfDetail.DrugKbn,
                         odrInfDetail.YohoKbn,
                         odrInfDetail.Kokuji1 ?? string.Empty,
                         odrInfDetail.Kokuji2 ?? string.Empty,
                         odrInfDetail.IsNodspRece,
                         odrInfDetail.IpnCd ?? string.Empty,
                         odrInfDetail.IpnName ?? string.Empty,
                         odrInfDetail.Bunkatu ?? string.Empty,
                         odrInfDetail.CmtName ?? string.Empty,
                         odrInfDetail.CmtOpt ?? string.Empty,
                         odrInfDetail.FontColor ?? string.Empty,
                         odrInfDetail.CommentNewline,
                         tenMst?.MasterSbt ?? string.Empty,
                         odrInf.InoutKbn,
                         yakka.Yakka,
                         isGetPriceInYakka,
                         tenMst?.Ten ?? 0,
                         bunkatuKoui,
                         alternationIndex,
                         kensaGaichu,
                         0,
                         0,
                         tenMst?.OdrTermVal ?? 0,
                         tenMst?.CnvTermVal ?? 0,
                         tenMst?.YjCd ?? string.Empty,
                         yohoSet,
                         kasan?.Kasan1 ?? 0,
                         kasan?.Kasan2 ?? 0,
                         kensaMst?.CenterItemCd1 ?? string.Empty,
                         kensaMst?.CenterItemCd2 ?? string.Empty,
                         tenMst?.HandanGrpKbn ?? 0,
                         kensaMst == null
                        );
                lock (obj)
                {
                    odrInfDetailModels.Add(odrInfDetailModel);
                    index++;
                }
            });
            var createName = NoTrackingDataContext.UserMsts.FirstOrDefault(u => u.UserId == odrInf.CreateId && u.HpId == odrInf.HpId)?.Sname ?? string.Empty;

            return new RsvkrtOrderInfModel(
                    odrInf.HpId,
                    odrInf.PtId,
                    odrInf.RsvDate,
                    odrInf.RsvkrtNo,
                    odrInf.RpNo,
                    odrInf.RpEdaNo,
                    odrInf.Id,
                    odrInf.HokenPid,
                    odrInf.OdrKouiKbn,
                    odrInf.RpName ?? string.Empty,
                    odrInf.InoutKbn,
                    odrInf.SikyuKbn,
                    odrInf.SyohoSbt,
                    odrInf.SanteiKbn,
                    odrInf.TosekiKbn,
                    odrInf.DaysCnt,
                    odrInf.IsDeleted,
                    odrInf.SortNo,
                    odrInf.CreateDate,
                    odrInf.CreateId,
                    createName,
                    odrInfDetailModels,
                    odrInf.UpdateDate,
                    odrInf.CreateMachine ?? string.Empty,
                    odrInf.UpdateMachine ?? string.Empty
                );
        }

        private RsvkrtOrderInfModel ConvertOrderInfToModel(RsvkrtOdrInf odrInf)
        {
            return new RsvkrtOrderInfModel(
                    odrInf.HpId,
                    odrInf.PtId,
                    odrInf.RsvDate,
                    odrInf.RsvkrtNo,
                    odrInf.RpNo,
                    odrInf.RpEdaNo,
                    odrInf.Id,
                    odrInf.HokenPid,
                    odrInf.OdrKouiKbn,
                    odrInf.RpName ?? string.Empty,
                    odrInf.InoutKbn,
                    odrInf.SikyuKbn,
                    odrInf.SyohoSbt,
                    odrInf.SanteiKbn,
                    odrInf.TosekiKbn,
                    odrInf.DaysCnt,
                    odrInf.IsDeleted,
                    odrInf.SortNo,
                    odrInf.CreateDate,
                    odrInf.CreateId,
                    string.Empty,
                    new(),
                    odrInf.UpdateDate,
                    odrInf.CreateMachine ?? string.Empty,
                    odrInf.UpdateMachine ?? string.Empty
                );
        }

        private static List<YohoSetMstModel> GetListYohoSetMstModelByUserID(List<YohoSetMst> listYohoSetMst, List<TenMst> listTenMst)
        {
            var query = from yoho in listYohoSetMst
                        join ten in listTenMst on yoho.ItemCd?.Trim() equals ten.ItemCd.Trim()
                        select new
                        {
                            Yoho = yoho,
                            ItemName = ten.Name,
                            ten.YohoKbn
                        };

            return query.OrderBy(u => u.Yoho.SortNo).AsEnumerable().Select(u => new YohoSetMstModel(u.ItemName, u.YohoKbn, u.Yoho?.SetId ?? 0, u.Yoho?.UserId ?? 0, u.Yoho?.ItemCd ?? string.Empty)).ToList();
        }

        private static bool IsGetPriceInYakka(TenMst tenMst, List<IpnKasanExclude> ipnKasanExcludes, List<IpnKasanExcludeItem> ipnKasanExcludeItems)
        {
            if (tenMst == null) return false;

            var ipnKasanExclude = ipnKasanExcludes.FirstOrDefault(u => u.IpnNameCd == tenMst.IpnNameCd);

            var ipnKasanExcludeItem = ipnKasanExcludeItems.FirstOrDefault(u => u.ItemCd == tenMst.ItemCd);

            return ipnKasanExclude == null && ipnKasanExcludeItem == null;
        }

        private static int GetKensaGaichu(RsvkrtOdrInfDetail odrInfDetail, TenMst tenMst, int inOutKbn, int odrKouiKbn, KensaMst? kensaMst, int kensaIraiCondition, int kensaIrai)
        {
            if (string.IsNullOrEmpty(odrInfDetail?.ItemCd) &&
                   string.IsNullOrEmpty(odrInfDetail?.ItemName?.Trim()) &&
                   odrInfDetail?.SinKouiKbn == 0)
            {
                return KensaGaichuTextConst.NONE;
            }

            if (odrInfDetail?.SinKouiKbn == 61 || odrInfDetail?.SinKouiKbn == 64)
            {
                bool kensaCondition;
                if (kensaIraiCondition == 0)
                {
                    kensaCondition = (odrInfDetail.SinKouiKbn == 61 || odrInfDetail.SinKouiKbn == 64) && odrInfDetail.Kokuji1 != "7" && odrInfDetail.Kokuji1 != "9";
                }
                else
                {
                    kensaCondition = odrInfDetail.SinKouiKbn == 61 && odrInfDetail.Kokuji1 != "7" && odrInfDetail.Kokuji1 != "9" && (tenMst == null ? 0 : tenMst.HandanGrpKbn) != 6;
                }

                if (kensaCondition && inOutKbn == 1)
                {
                    int kensaSetting = kensaIrai;
                    if (kensaMst == null)
                    {
                        if (kensaSetting > 0)
                        {
                            return KensaGaichuTextConst.GAICHU_NONE;
                        }
                    }
                    else if (string.IsNullOrEmpty(kensaMst.CenterItemCd1)
                        && string.IsNullOrEmpty(kensaMst.CenterItemCd2) && kensaSetting > 1)
                    {
                        return KensaGaichuTextConst.GAICHU_NOT_SET;
                    }
                }
            }

            if (!string.IsNullOrEmpty(odrInfDetail?.ItemName) && string.IsNullOrEmpty(odrInfDetail.ItemCd))
            {
                if (inOutKbn == 1 && (odrKouiKbn >= 20 && odrKouiKbn <= 23) || odrKouiKbn == 28)
                {
                    if (odrInfDetail.IsNodspRece == 0)
                    {
                        return KensaGaichuTextConst.IS_DISPLAY_RECE_ON;
                    }
                }
                else
                {
                    if (odrInfDetail.IsNodspRece == 1)
                    {
                        return KensaGaichuTextConst.IS_DISPLAY_RECE_OFF;
                    }
                }
            }
            return KensaGaichuTextConst.NONE;
        }

        private List<PrefixSuffixModel> SyusyokuCdToList(RsvkrtByomei mst)
        {
            List<string> codeList = new()
            {
                mst.SyusyokuCd1 ?? string.Empty,
                mst.SyusyokuCd2 ?? string.Empty,
                mst.SyusyokuCd3 ?? string.Empty,
                mst.SyusyokuCd4 ?? string.Empty,
                mst.SyusyokuCd5 ?? string.Empty,
                mst.SyusyokuCd6 ?? string.Empty,
                mst.SyusyokuCd7 ?? string.Empty,
                mst.SyusyokuCd8 ?? string.Empty,
                mst.SyusyokuCd9 ?? string.Empty,
                mst.SyusyokuCd10 ?? string.Empty,
                mst.SyusyokuCd11 ?? string.Empty,
                mst.SyusyokuCd12 ?? string.Empty,
                mst.SyusyokuCd13 ?? string.Empty,
                mst.SyusyokuCd14 ?? string.Empty,
                mst.SyusyokuCd15 ?? string.Empty,
                mst.SyusyokuCd16 ?? string.Empty,
                mst.SyusyokuCd17 ?? string.Empty,
                mst.SyusyokuCd18 ?? string.Empty,
                mst.SyusyokuCd19 ?? string.Empty,
                mst.SyusyokuCd20 ?? string.Empty,
                mst.SyusyokuCd21 ?? string.Empty
            };
            codeList = codeList.Where(c => c != string.Empty).Distinct().ToList();
            if (codeList.Count == 0)
            {
                return new List<PrefixSuffixModel>();
            }

            var byomeiMstList = NoTrackingDataContext.ByomeiMsts.Where(b => codeList.Contains(b.ByomeiCd)).ToList();

            List<PrefixSuffixModel> result = new();
            foreach (var code in codeList)
            {
                var byomeiMst = byomeiMstList.FirstOrDefault(b => b.ByomeiCd == code);
                if (byomeiMst == null)
                {
                    continue;
                }
                result.Add(new PrefixSuffixModel(code, byomeiMst.Byomei ?? string.Empty));
            }

            return result;
        }

        public List<NextOrderModel> GetList(int hpId, long ptId, bool isDeleted)
        {
            List<NextOrderModel> result;

            // check if exit cache, get data from cache
            string finalKey = key + ptId;
            if (_cache.KeyExists(finalKey))
            {
                var cacheString = _cache.StringGet(finalKey).ToString();
                result = !string.IsNullOrEmpty(cacheString) ? JsonSerializer.Deserialize<List<NextOrderModel>>(cacheString) ?? new() : new();
                return result;
            }

            // if not exist cache, get data from database
            var allRsvkrtMst = TrackingDataContext.RsvkrtMsts.Where(rsv => rsv.HpId == hpId && rsv.PtId == ptId && (isDeleted || rsv.IsDeleted == 0))?.AsEnumerable();
            result = allRsvkrtMst?.Select(rsv => ConvertToModel(rsv)).ToList() ?? new();

            // set cache data
            var json = JsonSerializer.Serialize(result);
            _cache.StringSet(finalKey, json);
            return result;
        }

        private static NextOrderModel ConvertToModel(RsvkrtMst rsvkrtMst)
        {
            return new NextOrderModel(
                        rsvkrtMst.HpId,
                        rsvkrtMst.PtId,
                        rsvkrtMst.RsvkrtNo,
                        rsvkrtMst.RsvkrtKbn,
                        rsvkrtMst.RsvDate,
                        rsvkrtMst.RsvName ?? string.Empty,
                        rsvkrtMst.IsDeleted,
                        rsvkrtMst.SortNo,
                        new(),
                        new(),
                        new(),
                        new()
                   );
        }

        private static RsvkrtMst ConvertModelToRsvkrtNextOrder(int userId, NextOrderModel nextOrderModel, RsvkrtMst? oldNextOrder)
        {
            return new RsvkrtMst
            {
                HpId = nextOrderModel.HpId,
                PtId = nextOrderModel.PtId,
                RsvkrtNo = nextOrderModel.RsvkrtNo,
                RsvkrtKbn = nextOrderModel.RsvkrtKbn,
                RsvDate = nextOrderModel.RsvDate,
                RsvName = nextOrderModel.RsvName,
                SortNo = nextOrderModel.SortNo,
                IsDeleted = nextOrderModel.IsDeleted,
                CreateDate = oldNextOrder == null ? CIUtil.GetJapanDateTimeNow() : oldNextOrder.CreateDate,
                CreateId = oldNextOrder == null ? userId : oldNextOrder.CreateId,
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId
            };
        }

        private static RsvkrtByomei ConvertModelToRsvkrtByomei(int userId, RsvkrtByomeiModel byomei, long rsvkrtNo = 0)
        {
            return new RsvkrtByomei
            {
                Id = byomei.Id,
                HpId = byomei.HpId,
                PtId = byomei.PtId,
                RsvkrtNo = rsvkrtNo == 0 ? byomei.RsvkrtNo : rsvkrtNo,
                SeqNo = byomei.SeqNo,
                ByomeiCd = byomei.ByomeiCd,
                SyusyokuCd1 = byomei.PrefixSuffixList.FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd2 = byomei.PrefixSuffixList.Skip(1).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd3 = byomei.PrefixSuffixList.Skip(2).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd4 = byomei.PrefixSuffixList.Skip(3).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd5 = byomei.PrefixSuffixList.Skip(4).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd6 = byomei.PrefixSuffixList.Skip(5).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd7 = byomei.PrefixSuffixList.Skip(6).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd8 = byomei.PrefixSuffixList.Skip(7).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd9 = byomei.PrefixSuffixList.Skip(8).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd10 = byomei.PrefixSuffixList.Skip(9).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd11 = byomei.PrefixSuffixList.Skip(10).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd12 = byomei.PrefixSuffixList.Skip(11).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd13 = byomei.PrefixSuffixList.Skip(12).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd14 = byomei.PrefixSuffixList.Skip(13).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd15 = byomei.PrefixSuffixList.Skip(14).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd16 = byomei.PrefixSuffixList.Skip(15).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd17 = byomei.PrefixSuffixList.Skip(16).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd18 = byomei.PrefixSuffixList.Skip(17).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd19 = byomei.PrefixSuffixList.Skip(18).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd20 = byomei.PrefixSuffixList.Skip(19).FirstOrDefault()?.Code ?? string.Empty,
                SyusyokuCd21 = byomei.PrefixSuffixList.Skip(20).FirstOrDefault()?.Code ?? string.Empty,
                Byomei = byomei.Byomei,
                SyobyoKbn = byomei.SyobyoKbn,
                SikkanKbn = byomei.SikkanKbn,
                NanbyoCd = byomei.NanbyoCd,
                HosokuCmt = byomei.HosokuCmt,
                IsNodspKarte = byomei.IsNodspKarte,
                IsNodspRece = byomei.IsNodspRece,
                IsDeleted = byomei.IsDeleted,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                CreateId = userId,
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId
            };
        }

        private static RsvkrtKarteInf ConvertModelToRsvkrtKarteInf(int userId, RsvkrtKarteInfModel rsvkrtKarteInfModel, long rsvkrtNo = 0, long seqNo = 1)
        {
            return new RsvkrtKarteInf
            {
                HpId = rsvkrtKarteInfModel.HpId,
                PtId = rsvkrtKarteInfModel.PtId,
                RsvDate = rsvkrtKarteInfModel.RsvDate,
                RsvkrtNo = rsvkrtNo == 0 ? rsvkrtKarteInfModel.RsvkrtNo : rsvkrtNo,
                KarteKbn = 1,
                SeqNo = seqNo,
                Text = rsvkrtKarteInfModel.Text,
                RichText = Encoding.UTF8.GetBytes(rsvkrtKarteInfModel.RichText),
                IsDeleted = rsvkrtKarteInfModel.IsDeleted,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                CreateId = userId,
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId
            };
        }

        private static RsvkrtOdrInf ConvertModelToRsvkrtOrderInf(int userId, long rpNo, RsvkrtOrderInfModel rsvkrtOrderInfModel, long rsvkrtNo = 0, long rpEdaNo = 1)
        {
            return new RsvkrtOdrInf
            {
                Id = rsvkrtOrderInfModel.Id,
                HpId = rsvkrtOrderInfModel.HpId,
                PtId = rsvkrtOrderInfModel.PtId,
                RsvkrtNo = rsvkrtNo == 0 ? rsvkrtOrderInfModel.RsvkrtNo : rsvkrtNo,
                RsvDate = rsvkrtOrderInfModel.RsvDate,
                RpNo = rpNo,
                RpEdaNo = rpEdaNo,
                HokenPid = rsvkrtOrderInfModel.HokenPid,
                OdrKouiKbn = rsvkrtOrderInfModel.OdrKouiKbn,
                RpName = rsvkrtOrderInfModel.RpName,
                InoutKbn = rsvkrtOrderInfModel.InoutKbn,
                SikyuKbn = rsvkrtOrderInfModel.SikyuKbn,
                SyohoSbt = rsvkrtOrderInfModel.SyohoSbt,
                SanteiKbn = rsvkrtOrderInfModel.SanteiKbn,
                TosekiKbn = rsvkrtOrderInfModel.TosekiKbn,
                DaysCnt = rsvkrtOrderInfModel.DaysCnt,
                IsDeleted = rsvkrtOrderInfModel.IsDeleted,
                SortNo = rsvkrtOrderInfModel.SortNo,
                CreateDate = CIUtil.GetJapanDateTimeNow(),
                CreateId = userId,
                UpdateDate = CIUtil.GetJapanDateTimeNow(),
                UpdateId = userId
            };
        }

        private static RsvkrtOdrInfDetail ConvertModelToRsvkrtOrderInfDetail(long rpNo, RsvKrtOrderInfDetailModel rsvkrtOrderInfModel, long rsvkrtNo = 0, long rpEdaNo = 1)
        {
            return new RsvkrtOdrInfDetail
            {
                HpId = rsvkrtOrderInfModel.HpId,
                PtId = rsvkrtOrderInfModel.PtId,
                RsvkrtNo = rsvkrtNo == 0 ? rsvkrtOrderInfModel.RsvkrtNo : rsvkrtNo,
                RpNo = rpNo,
                RpEdaNo = rpEdaNo,
                RowNo = rsvkrtOrderInfModel.RowNo,
                RsvDate = rsvkrtOrderInfModel.RsvDate,
                SinKouiKbn = rsvkrtOrderInfModel.SinKouiKbn,
                ItemCd = rsvkrtOrderInfModel.ItemCd,
                ItemName = rsvkrtOrderInfModel.ItemName,
                Suryo = rsvkrtOrderInfModel.Suryo,
                UnitName = rsvkrtOrderInfModel.UnitName,
                UnitSbt = rsvkrtOrderInfModel.UnitSbt,
                TermVal = rsvkrtOrderInfModel.TermVal,
                KohatuKbn = rsvkrtOrderInfModel.KohatuKbn,
                SyohoKbn = rsvkrtOrderInfModel.SyohoKbn,
                SyohoLimitKbn = rsvkrtOrderInfModel.SyohoLimitKbn,
                DrugKbn = rsvkrtOrderInfModel.DrugKbn,
                YohoKbn = rsvkrtOrderInfModel.YohoKbn,
                Kokuji1 = rsvkrtOrderInfModel.Kokuji1,
                Kokuji2 = rsvkrtOrderInfModel.Kokuji2,
                IsNodspRece = rsvkrtOrderInfModel.IsNodspRece,
                IpnCd = rsvkrtOrderInfModel.IpnCd,
                IpnName = rsvkrtOrderInfModel.IpnName,
                Bunkatu = rsvkrtOrderInfModel.Bunkatu,
                CmtName = rsvkrtOrderInfModel.CmtName,
                CmtOpt = rsvkrtOrderInfModel.CmtOpt,
                FontColor = rsvkrtOrderInfModel.FontColor,
                CommentNewline = rsvkrtOrderInfModel.CommentNewline
            };
        }

        private long GetMaxRpNo(int hpId, long ptId)
        {
            var odrList = NoTrackingDataContext.RsvkrtOdrInfs
                .Where(odr => odr.HpId == hpId && odr.PtId == ptId);

            if (odrList.Any())
            {
                return odrList.Max(odr => odr.RpNo);
            }

            return 0;
        }

        public List<NextOrderFileInfModel> GetNextOrderFiles(int hpId, long ptId, long rsvkrtNo)
        {
            var lastSeqNo = GetLastNextOrderSeqNo(hpId, ptId, rsvkrtNo);
            var result = NoTrackingDataContext.RsvkrtKarteImgInfs.Where(item =>
                                                                                item.HpId == hpId
                                                                                && item.PtId == ptId
                                                                                && item.RsvkrtNo == rsvkrtNo
                                                                                && item.SeqNo == lastSeqNo
                                                                                && item.FileName != string.Empty
                                                                                )
                                                                    .OrderBy(item => item.Position)
                                                                    .Select(item => new NextOrderFileInfModel(
                                                                            item.KarteKbn > 0,
                                                                            item.FileName ?? string.Empty
                                                                    )).ToList();
            return result;
        }

        public long GetLastNextOrderSeqNo(int hpId, long ptId)
        {
            var lastItem = NoTrackingDataContext.RsvkrtKarteImgInfs.Where(item => item.HpId == hpId && item.PtId == ptId).ToList()?.MaxBy(item => item.SeqNo);
            return lastItem != null ? lastItem.SeqNo : 0;
        }

        private long GetLastNextOrderSeqNo(int hpId, long ptId, long rsvkrtNo)
        {
            var lastItem = NoTrackingDataContext.RsvkrtKarteImgInfs.Where(item => item.HpId == hpId && item.PtId == ptId && item.RsvkrtNo == rsvkrtNo).ToList()?.MaxBy(item => item.SeqNo);
            return lastItem != null ? lastItem.SeqNo : 0;
        }

        public bool SaveListFileNextOrder(int hpId, long ptId, long rsvkrtNo, string host, List<NextOrderFileInfModel> listFiles, bool saveTempFile)
        {
            if (saveTempFile)
            {
                var listFileInsert = ConvertListInsertTempNextOrderFile(hpId, ptId, host, listFiles);
                if (listFileInsert.Any())
                {
                    TrackingDataContext.RsvkrtKarteImgInfs.AddRange(listFileInsert);
                }
            }
            else
            {
                UpdateSeqNoNextOrderFile(hpId, ptId, rsvkrtNo, listFiles.Select(item => new NextOrderFileInfModel(item.IsSchema, item.LinkFile.Replace(host, string.Empty))).ToList());
            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        private void UpdateSeqNoNextOrderFile(int hpId, long ptId, long rsvkrtNo, List<NextOrderFileInfModel> fileInfModelList)
        {
            var listFileName = fileInfModelList.Select(item => item.LinkFile).Distinct().ToList();
            int position = 1;
            var lastSeqNo = GetLastNextOrderSeqNo(hpId, ptId);
            var listOldFile = TrackingDataContext.RsvkrtKarteImgInfs.Where(item =>
                                               item.HpId == hpId
                                               && item.PtId == ptId
                                               && item.SeqNo == lastSeqNo
                                               && item.SeqNo != 0
                                               && item.FileName != null
                                               && listFileName.Contains(item.FileName)
                                               ).OrderBy(item => item.Position)
                                               .ToList();

            var listUpdateFiles = TrackingDataContext.RsvkrtKarteImgInfs.Where(item =>
                                               item.HpId == hpId
                                               && item.PtId == ptId
                                               && item.RsvkrtNo == 0
                                               && item.SeqNo == 0
                                               && item.FileName != null
                                               && listFileName.Contains(item.FileName)
                                               ).ToList();

            foreach (var fileItem in fileInfModelList)
            {
                var oldItemConvert = listOldFile.FirstOrDefault(item => item.SeqNo == lastSeqNo
                                                                        && item.FileName != null
                                                                        && item.FileName == fileItem.LinkFile);

                if (oldItemConvert != null)
                {
                    RsvkrtKarteImgInf newFileConvert = oldItemConvert;
                    newFileConvert.Id = 0;
                    newFileConvert.RsvkrtNo = rsvkrtNo;
                    newFileConvert.SeqNo = lastSeqNo + 1;
                    newFileConvert.Position = position;
                    TrackingDataContext.RsvkrtKarteImgInfs.Add(newFileConvert);
                    position++;
                    continue;
                }

                var oldItemUpdateSeqNo = listUpdateFiles.FirstOrDefault(item => item.SeqNo == 0
                                                                                && item.FileName != null
                                                                                && item.FileName == fileItem.LinkFile);
                if (oldItemUpdateSeqNo != null)
                {
                    oldItemUpdateSeqNo.RsvkrtNo = rsvkrtNo;
                    oldItemUpdateSeqNo.SeqNo = lastSeqNo + 1;
                    oldItemUpdateSeqNo.RsvkrtNo = rsvkrtNo;
                    oldItemUpdateSeqNo.Position = position;
                    position++;
                    continue;
                }

                RsvkrtKarteImgInf newFile = new();
                newFile.FileName = fileItem.LinkFile;
                newFile.Id = 0;
                newFile.SeqNo = lastSeqNo + 1;
                newFile.Position = position;
                newFile.KarteKbn = fileItem.IsSchema ? 1 : 0;
                newFile.PtId = ptId;
                newFile.HpId = hpId;
                newFile.RsvkrtNo = rsvkrtNo;
                TrackingDataContext.RsvkrtKarteImgInfs.Add(newFile);
                position++;
            }

            if (listFileName.Any(item => item == string.Empty))
            {
                RsvkrtKarteImgInf newFile = new();
                newFile.FileName = string.Empty;
                newFile.Id = 0;
                newFile.SeqNo = lastSeqNo + 1;
                newFile.Position = 1;
                newFile.KarteKbn = 0;
                newFile.PtId = ptId;
                newFile.HpId = hpId;
                newFile.RsvkrtNo = rsvkrtNo;
                TrackingDataContext.RsvkrtKarteImgInfs.Add(newFile);
            }
        }

        private List<RsvkrtKarteImgInf> ConvertListInsertTempNextOrderFile(int hpId, long ptId, string host, List<NextOrderFileInfModel> listFiles)
        {
            List<RsvkrtKarteImgInf> result = new();
            int position = 1;

            // insert new entity
            foreach (var item in listFiles)
            {
                RsvkrtKarteImgInf entity = new();
                entity.HpId = hpId;
                entity.PtId = ptId;
                entity.RsvkrtNo = 0;
                entity.Position = position;
                entity.SeqNo = 0;
                if (item.IsSchema)
                {
                    entity.KarteKbn = 1;
                }
                entity.FileName = item.LinkFile.Replace(host, string.Empty);
                result.Add(entity);
                position += 1;
            }
            return result;
        }

        public List<RaiinKbnModel> InitDefaultByNextOrder(int hpId, long ptId, int sinDate, List<RaiinKbnModel> raiinKbns, List<(int grpId, int kbnCd, int kouiKbn1, int kouiKbn2)> raiinKouiKbns, List<RaiinKbnItemModel> raiinKbnItemCds)
        {
            var allNextOdr = GetNextOdrInfModels(hpId, ptId, sinDate);
            var nextOdrs = allNextOdr.FindAll(p => p.rsvDate == sinDate).ToList();
            if (nextOdrs.Count == 0)
            {
                nextOdrs = allNextOdr.FindAll(o => o.rsvDate == NextOrderConst.DefaultRsvDate);
            }

            foreach (var raiinKbn in raiinKbns)
            {
                if (raiinKbn.RaiinKbnInfModel.KbnCd > 0) continue;

                foreach (var kbnDetail in raiinKbn.RaiinKbnDetailModels)
                {
                    if (!kbnDetail.IsNextOrderChecked) continue;

                    //grid raiinKbnItem
                    var kouiKbns = raiinKouiKbns.FindAll(k => k.grpId == kbnDetail.GrpCd && k.kbnCd == kbnDetail.KbnCd);

                    //checkbox group raiinKouiKbn
                    var kbnItems = raiinKbnItemCds.FindAll(p => p.GrpCd == kbnDetail.GrpCd && p.KbnCd == kbnDetail.KbnCd && !string.IsNullOrEmpty(p.ItemCd));
                    var includeItems = kbnItems.FindAll(p => p.IsExclude != 1);
                    var excludeItems = kbnItems.FindAll(p => p.IsExclude == 1);

                    bool isSet = false;
                    foreach (var nextOdr in nextOdrs)
                    {
                        if (excludeItems.Exists(p => p.ItemCd == nextOdr.itemCd)) continue;

                        if (kouiKbns.Exists(p => p.kouiKbn1 == nextOdr.sinKouiKbn || p.kouiKbn2 == nextOdr.sinKouiKbn) ||
                            includeItems.Exists(p => p.ItemCd == nextOdr.itemCd))
                        {
                            raiinKbn.RaiinKbnInfModel.ChangeKbnCd(kbnDetail.KbnCd);
                            isSet = true;
                            break;
                        }
                    }
                    if (isSet) break;
                }
            }

            return raiinKbns;
        }

        private List<(int sinKouiKbn, int rsvDate, string itemCd)> GetNextOdrInfModels(int hpId, long ptId, int sinDate)
        {
            var result = new List<(int sinKouiKbn, int rsvDate, string itemCd)>();
            var nextOdrInfs = NoTrackingDataContext.RsvkrtOdrInfs.Where(p => p.HpId == hpId &&
                                                                                             p.PtId == ptId &&
                                                                                             p.IsDeleted == DeleteTypes.None &&
                                                                                             (p.RsvDate == sinDate || p.RsvDate == NextOrderConst.DefaultRsvDate));
            var nextOdrDetails = NoTrackingDataContext.RsvkrtOdrInfDetails.Where(p => p.HpId == hpId &&
                                                                                                      p.PtId == ptId &&
                                                                                                      (p.RsvDate == sinDate || p.RsvDate == NextOrderConst.DefaultRsvDate));
            var odrInfJoinDetailQuery = from nextOdrInf in nextOdrInfs.AsEnumerable()
                                        join nextOdrDetail in nextOdrDetails
                                        on new { nextOdrInf.HpId, nextOdrInf.PtId, nextOdrInf.RsvkrtNo, nextOdrInf.RpNo, nextOdrInf.RpEdaNo }
                                        equals new { nextOdrDetail.HpId, nextOdrDetail.PtId, nextOdrDetail.RsvkrtNo, nextOdrDetail.RpNo, nextOdrDetail.RpEdaNo } into TempNextOdrDetails
                                        select new
                                        {
                                            NextOdrDetails = TempNextOdrDetails
                                        };
            var enities = odrInfJoinDetailQuery.ToList();
            foreach (var entity in enities)
            {
                foreach (var entityDetail in entity.NextOdrDetails)
                {
                    result.Add(new(entityDetail.SinKouiKbn, entityDetail.RsvDate, entityDetail?.ItemCd ?? string.Empty));
                }
            }
            return result;
        }

        public bool ClearTempData(int hpId, long ptId, List<string> listFileNames)
        {
            var listDeletes = TrackingDataContext.RsvkrtKarteImgInfs.Where(item => item.HpId == hpId
                                                                                   && item.SeqNo == 0
                                                                                   && item.RsvkrtNo == 0
                                                                                   && item.FileName != null
                                                                                   && listFileNames.Contains(item.FileName)
                                                            ).ToList();
            TrackingDataContext.RsvkrtKarteImgInfs.RemoveRange(listDeletes);
            return TrackingDataContext.SaveChanges() > 0;
        }

        private long GetPtNum(int hpId, long ptId)
        {
            var ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(item => item.HpId == hpId && item.PtId == ptId);
            return ptInf != null ? ptInf.PtNum : 0;
        }

        private void SaveFileNextOrder(int hpId, long ptId, long ptNum, long rsvkrtNo, NextOrderModel nextOrderModel)
        {
            if (nextOrderModel.FileItem.IsUpdateFile)
            {
                if (rsvkrtNo > 0)
                {
                    var listFileItems = nextOrderModel.FileItem.ListFileItems;
                    if (!listFileItems.Any())
                    {
                        listFileItems = new List<string> { string.Empty };
                    }
                    SaveFileNextOrderAction(hpId, ptId, ptNum, rsvkrtNo, listFileItems, true);
                }
                else
                {
                    SaveFileNextOrderAction(hpId, ptId, ptNum, rsvkrtNo, nextOrderModel.FileItem.ListFileItems, false);
                }
            }
        }

        private void SaveFileNextOrderAction(int hpId, long ptId, long ptNum, long rsvkrtNo, List<string> listFileItems, bool saveSuccess)
        {
            List<string> listFolders = new();
            string path = string.Empty;
            listFolders.Add(CommonConstants.Store);
            listFolders.Add(CommonConstants.Karte);
            listFolders.Add(CommonConstants.NextPic);
            path = _amazonS3Service!.GetFolderUploadToPtNum(listFolders, ptNum);
            string host = _options!.BaseAccessUrl + "/" + path;
            var listUpdates = listFileItems.Select(item => item.Replace(host, string.Empty)).ToList();
            if (saveSuccess)
            {
                List<NextOrderFileInfModel> nextOrderFileInfModelList = new();
                var fileInfUpdateTemp = CopyFileFromKarteToNextOrder(ptNum, path, listFileItems);
                if (fileInfUpdateTemp.Any())
                {
                    foreach (var item in fileInfUpdateTemp)
                    {
                        if (item.Key == item.Value)
                        {
                            nextOrderFileInfModelList.Add(new NextOrderFileInfModel(false, item.Value));
                        }
                        else
                        {
                            nextOrderFileInfModelList.Add(new NextOrderFileInfModel(true, item.Value));
                        }
                    }
                    if (!nextOrderFileInfModelList.Any())
                    {
                        nextOrderFileInfModelList.Add(new NextOrderFileInfModel(true, string.Empty));
                    }
                }
                SaveListFileNextOrder(hpId, ptId, rsvkrtNo, host, nextOrderFileInfModelList, false);
            }
            else
            {
                ClearTempData(hpId, ptId, listUpdates.ToList());
                foreach (var item in listUpdates)
                {
                    _amazonS3Service.DeleteObjectAsync(path + item);
                }
            }
        }

        private Dictionary<string, string> CopyFileFromKarteToNextOrder(long ptNum, string pathSaveSet, List<string> listFileFromKarte)
        {
            Dictionary<string, string> fileInfUpdateTemp = new();

            var listFolderPath = new List<string>(){
                                            CommonConstants.Store,
                                            CommonConstants.Karte
                                        };
            string baseAccessUrl = _options!.BaseAccessUrl;
            string karteHost = baseAccessUrl + "/" + _amazonS3Service!.GetFolderUploadToPtNum(listFolderPath, ptNum);

            foreach (var oldFileLink in listFileFromKarte)
            {
                string oldFileName = Path.GetFileName(oldFileLink);
                if (oldFileLink.Contains(karteHost))
                {
                    string newFile = baseAccessUrl + $"/" + pathSaveSet + _amazonS3Service.GetUniqueFileNameKey(oldFileName.Trim());
                    var copySuccess = _amazonS3Service.CopyObjectAsync(oldFileLink.Replace(baseAccessUrl, string.Empty), newFile.Replace(baseAccessUrl, string.Empty)).Result;
                    if (copySuccess)
                    {
                        fileInfUpdateTemp.Add(oldFileName, newFile);
                    }
                }
                else
                {
                    fileInfUpdateTemp.Add(oldFileName, oldFileName);
                }
            }
            return fileInfUpdateTemp;
        }

        public bool CheckNextOrdHaveOdr(int hpId, long ptId, int sinDate)
        {
            var allRsvkrtMstQuery = NoTrackingDataContext.RsvkrtMsts
                .Where(mst => mst.HpId == hpId && mst.PtId == ptId && mst.IsDeleted == 0 && mst.RsvkrtKbn == 0 && (mst.RsvDate == sinDate || mst.RsvDate == 99999999));

            IQueryable<RsvkrtOdrInf> allRsvkrtOdrInfQuery;
            allRsvkrtOdrInfQuery = NoTrackingDataContext.RsvkrtOdrInfs
                .Where(o => o.HpId == hpId &&
                                        o.PtId == ptId &&
                                        (o.RsvDate == sinDate || o.RsvDate == 99999999) &&
                                        o.OdrKouiKbn >= 13 &&
                                        o.IsDeleted == DeleteStatus.None);

            var queryRsvkrtMsts = from mst in allRsvkrtMstQuery
                                  join odr in allRsvkrtOdrInfQuery on mst.RsvkrtNo equals odr.RsvkrtNo into odrs
                                  orderby mst.RsvDate descending
                                  select new
                                  {
                                      Mst = mst,
                                      Odrs = odrs,
                                  };
            return queryRsvkrtMsts.AsEnumerable().Any(x => x != null && x.Odrs.Any(o => o != null && o.IsDeleted == 0));
        }

        public bool CheckUpsertNextOrder(int hpId, long ptId, int rsvDate)
        {
            return NoTrackingDataContext.RsvkrtMsts.Any(x =>
                                                            x.HpId == hpId &&
                                                            x.PtId == ptId &&
                                                            x.RsvkrtKbn == 0 &&
                                                            x.RsvDate == rsvDate &&
                                                            x.IsDeleted == DeleteTypes.None);
        }
        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
