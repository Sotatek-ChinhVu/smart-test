using Domain.Models.Diseases;
using Domain.Models.SetMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Helper.Redis;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using StackExchange.Redis;
using System.Data;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Repositories;

public class SetMstRepository : RepositoryBase, ISetMstRepository
{
    private readonly string defaultSetName = "新規セット";
    private readonly string defaultGroupName = "新規グループ";
    private readonly int tryCountSave = 10;
    private readonly string key;
    private readonly IDatabase _cache;
    private readonly IConfiguration _configuration;

    public SetMstRepository(ITenantProvider tenantProvider, IConfiguration configuration) : base(tenantProvider)
    {
        key = GetCacheKey() + "SetMst";
        _configuration = configuration;
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

    public IEnumerable<SetMstModel> ReloadCache(int hpId, int generationId)
    {
        var finalKey = key + "_" + generationId;
        var setMstModelList =
                NoTrackingDataContext.SetMsts
                .Where(s => s.HpId == hpId
                            && s.IsDeleted == 0
                            && s.GenerationId == generationId)
                .Select(s => new SetMstModel(
                    s.HpId,
                    s.SetCd,
                    s.SetKbn,
                    s.SetKbnEdaNo,
                    s.GenerationId,
                    s.Level1,
                    s.Level2,
                    s.Level3,
                    s.SetName == null ? string.Empty : s.SetName,
                    s.WeightKbn,
                    s.Color,
                    s.IsDeleted,
                    s.IsGroup,
                    false
                    ))
                .ToList();
        var json = JsonSerializer.Serialize(setMstModelList);
        _cache.StringSet(finalKey, json);

        return setMstModelList;
    }

    private IEnumerable<SetMstModel> ReadCache(int generationId)
    {
        var finalKey = key + "_" + generationId;
        var results = _cache.StringGet(finalKey);
        var json = results.AsString();
        var datas = !string.IsNullOrEmpty(json) ? JsonSerializer.Deserialize<List<SetMstModel>>(json) : new();
        return datas ?? new();
    }

    public List<SetMstModel> GetList(int hpId, int setKbn, int setKbnEdaNo, int generationId, string textSearch)
    {
        var finalKey = key + "_" + generationId;
        IEnumerable<SetMstModel> setMstModelList;
        if (!_cache.KeyExists(finalKey))
        {
            setMstModelList = ReloadCache(hpId, generationId);
        }
        else
        {
            setMstModelList = ReadCache(generationId);
        }

        List<SetMstModel> result;
        if (string.IsNullOrEmpty(textSearch))
        {
            result = setMstModelList!
                    .Where(s => s.HpId == hpId
                                && s.GenerationId == generationId
                                && s.SetKbn == setKbn
                                && s.SetKbnEdaNo == setKbnEdaNo - 1
                                && s.IsDeleted == 0)
                    .ToList();
        }
        else
        {
            string kanaKeyword = textSearch;
            if (!WanaKana.IsKana(textSearch) && WanaKana.IsRomaji(textSearch))
            {
                var inputKeyword = textSearch;
                kanaKeyword = CIUtil.ToHalfsize(textSearch);
                if (WanaKana.IsRomaji(kanaKeyword)) //If after convert to kana. type still is IsRomaji, back to base input keyword
                    kanaKeyword = inputKeyword;
            }

            string sBigKeyword = kanaKeyword.ToUpper()
                                            .Replace("ｧ", "ｱ")
                                            .Replace("ｨ", "ｲ")
                                            .Replace("ｩ", "ｳ")
                                            .Replace("ｪ", "ｴ")
                                            .Replace("ｫ", "ｵ")
                                            .Replace("ｬ", "ﾔ")
                                            .Replace("ｭ", "ﾕ")
                                            .Replace("ｮ", "ﾖ")
                                            .Replace("ｯ", "ﾂ");

            // Search By SetName
            setMstModelList = setMstModelList!.Where(item => item.HpId == hpId
                                                             && item.GenerationId == generationId
                                                             && item.SetKbn == setKbn
                                                             && item.SetKbnEdaNo == setKbnEdaNo - 1
                                                             && item.IsDeleted == 0);

            var searchItemList = setMstModelList!
                                .Where(item => !string.IsNullOrEmpty(textSearch)
                                               && ((item.SetName != null && item.SetName.Contains(textSearch))
                                                    || (item.SetName != null && item.SetName.ToUpper()
                                                                                            .Replace("ｧ", "ｱ")
                                                                                            .Replace("ｨ", "ｲ")
                                                                                            .Replace("ｩ", "ｳ")
                                                                                            .Replace("ｪ", "ｴ")
                                                                                            .Replace("ｫ", "ｵ")
                                                                                            .Replace("ｬ", "ﾔ")
                                                                                            .Replace("ｭ", "ﾕ")
                                                                                            .Replace("ｮ", "ﾖ")
                                                                                            .Replace("ｯ", "ﾂ")
                                                                                            .Contains(sBigKeyword))
                                                                                            ))
                                .ToList();

            // SearchBy Order Inf Detail
            var setCdOrderDetailList = NoTrackingDataContext.SetOdrInfDetail
                                       .Where(item => item.HpId == hpId
                                                      && ((item.ItemName != null && item.ItemName.Contains(textSearch))
                                                           || (item.ItemName != null && item.ItemName.ToUpper()
                                                                                                    .Replace("ｧ", "ｱ")
                                                                                                    .Replace("ｨ", "ｲ")
                                                                                                    .Replace("ｩ", "ｳ")
                                                                                                    .Replace("ｪ", "ｴ")
                                                                                                    .Replace("ｫ", "ｵ")
                                                                                                    .Replace("ｬ", "ﾔ")
                                                                                                    .Replace("ｭ", "ﾕ")
                                                                                                    .Replace("ｮ", "ﾖ")
                                                                                                    .Replace("ｯ", "ﾂ")
                                                                                                    .Contains(sBigKeyword))
                                                                                                    ))
                                       .Select(item => new
                                       {
                                           item.SetCd,
                                           item.RpNo,
                                           item.RpEdaNo
                                       })
                                       .Distinct()
                                       .ToList();

            var setOrderInfList = NoTrackingDataContext.SetOdrInf.Where(item => item.HpId == hpId
                                                                                && item.IsDeleted == 0
                                                                                && setCdOrderDetailList.Select(item => item.SetCd).Contains(item.SetCd))
                                                                 .ToList();

            List<int> setCdOrderInfList = new();
            foreach (var order in setOrderInfList)
            {
                var setCds = setCdOrderDetailList.Where(item => item.RpNo == order.RpNo
                                                                && item.RpEdaNo == order.RpEdaNo)
                                                 .Select(item => item.SetCd)
                                                 .ToList();
                setCdOrderInfList.AddRange(setCds);
            }
            setCdOrderInfList = setCdOrderInfList.Distinct().ToList();

            var setItemOrderDetailList = setMstModelList!.Where(item => setCdOrderInfList.Contains(item.SetCd)).ToList();
            searchItemList.AddRange(setItemOrderDetailList);

            // SearchBy Karte
            var setCdKarte = NoTrackingDataContext.SetKarteInf.Where(item => item.HpId == hpId
                                                                             && item.IsDeleted == 0
                                                                             && ((item.Text != null && item.Text.Contains(textSearch))
                                                                                  || (item.Text != null && item.Text.ToUpper()
                                                                                                                    .Replace("ｧ", "ｱ")
                                                                                                                    .Replace("ｨ", "ｲ")
                                                                                                                    .Replace("ｩ", "ｳ")
                                                                                                                    .Replace("ｪ", "ｴ")
                                                                                                                    .Replace("ｫ", "ｵ")
                                                                                                                    .Replace("ｬ", "ﾔ")
                                                                                                                    .Replace("ｭ", "ﾕ")
                                                                                                                    .Replace("ｮ", "ﾖ")
                                                                                                                    .Replace("ｯ", "ﾂ")
                                                                                                                    .Contains(sBigKeyword))))
                                                              .Select(item => item.SetCd)
                                                              .Distinct()
                                                              .ToList();
            var setItemKarteList = setMstModelList!.Where(item => setCdKarte.Contains(item.SetCd)).ToList();
            searchItemList.AddRange(setItemKarteList);

            List<int> setCdList = new();
            // Action gen rootSet
            searchItemList = searchItemList.Distinct().ToList();
            foreach (var searchItem in searchItemList)
            {
                setCdList.Add(searchItem.SetCd);

                // if item is level 1
                if (searchItem.Level2 == 0)
                {
                    var setCdChildren = setMstModelList.Where(item => item.Level1 == searchItem.Level1)
                                                       .Select(item => item.SetCd)
                                                       .ToList();
                    setCdList.AddRange(setCdChildren);
                }

                // if item is level 2
                if (searchItem.Level2 > 0 && searchItem.Level3 == 0)
                {
                    var setCdRootLevel1 = setMstModelList.First(item => item.Level1 == searchItem.Level1
                                                                        && item.Level2 == 0).SetCd;
                    setCdList.Add(setCdRootLevel1);
                    var setCdChildrenLevel3 = setMstModelList.Where(item => item.Level1 == searchItem.Level1
                                                                            && item.Level2 == searchItem.Level2)
                                                             .Select(item => item.SetCd)
                                                             .ToList();
                    setCdList.AddRange(setCdChildrenLevel3);
                }

                // if item is level 3
                if (searchItem.Level3 > 0)
                {
                    var setCdRootLevel1 = setMstModelList.First(item => item.Level1 == searchItem.Level1
                                                                        && item.Level2 == 0).SetCd;
                    setCdList.Add(setCdRootLevel1);

                    var setCdRootLevel2 = setMstModelList.First(item => item.Level1 == searchItem.Level1
                                                                        && item.Level2 == searchItem.Level2
                                                                        && item.Level3 == 0).SetCd;
                    setCdList.Add(setCdRootLevel2);
                }
            }
            setCdList = setCdList.Distinct().ToList();
            result = setMstModelList.Where(item => setCdList.Contains(item.SetCd)).ToList();
        }

        return result.OrderBy(s => s.Level1)
         .ThenBy(s => s.Level2)
         .ThenBy(s => s.Level3)
         .ToList();
    }

    public SetMstTooltipModel GetToolTip(int hpId, int setCd)
    {
        List<string> byomeiNameList = new();
        var setByomeiList = NoTrackingDataContext.SetByomei.Where(item => item.SetCd == setCd && item.HpId == hpId && item.IsDeleted != 1 && item.Byomei != string.Empty).ToList();
        foreach (var byomei in setByomeiList)
        {
            var syusyokuCdList = SyusyokuCdToList(byomei);
            var prefixList = syusyokuCdList.Where(item => !item.Code.StartsWith("8")).Select(item => item.Name).ToList();
            var suffixList = syusyokuCdList.Where(item => item.Code.StartsWith("8")).Select(item => item.Name).ToList();
            StringBuilder fullByomei = new();
            foreach (var item in prefixList)
            {
                fullByomei.Append(item);
            }
            fullByomei.Append(byomei.Byomei);
            foreach (var item in suffixList)
            {
                fullByomei.Append(item);
            }
            byomeiNameList.Add(fullByomei.ToString());
        }

        var listKarteInfs = NoTrackingDataContext.SetKarteInf.Where(item => item.SetCd == setCd && item.HpId == hpId && item.IsDeleted != 1 && item.KarteKbn == 1).ToList();
        var listKarteNames = listKarteInfs.Where(item => !string.IsNullOrEmpty(item.Text)).Select(item => item.Text ?? string.Empty).ToList();
        var keys = NoTrackingDataContext.SetOdrInf.Where(s => s.SetCd == setCd && s.HpId == hpId && s.IsDeleted != 1).Select(s => new { s.RpNo, s.RpEdaNo }).ToList();
        var allOrderDetails = NoTrackingDataContext.SetOdrInfDetail.Where(item => item.SetCd == setCd && item.HpId == hpId).ToList();
        Dictionary<long, List<OrderTooltipModel>> dicOrders = new();
        foreach (var key in keys)
        {
            var orderDetailPerRpNo = allOrderDetails.Where(item => item.SetCd == setCd && item.HpId == hpId && key.RpNo == item.RpNo && key.RpEdaNo == item.RpEdaNo).Select(item => new OrderTooltipModel(item.ItemName ?? string.Empty, item.Suryo, item.UnitName ?? string.Empty)).ToList();
            dicOrders.Add(key.RpNo, orderDetailPerRpNo);
        }

        return new SetMstTooltipModel(listKarteNames, dicOrders, byomeiNameList);
    }

    [Obsolete]
    public List<SetMstModel> SaveSetMstModel(int userId, int sinDate, SetMstModel setMstModel)
    {
        SetMst setMst = new();
        try
        {
            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
            executionStrategy.Execute(
                () =>
                {
                    using var transaction = TrackingDataContext.Database.BeginTransaction();
                    try
                    {
                        setMst = SaveSetMstAction(userId, sinDate, setMstModel);
                        TrackingDataContext.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                });

            List<SetMst> setMstList = new();
            setMstList.AddRange(NoTrackingDataContext.SetMsts
                      .Where(item => item.SetKbn == setMst.SetKbn
                                     && item.SetKbnEdaNo == setMst.SetKbnEdaNo
                                     && item.GenerationId == setMst.GenerationId
                                     && item.Level1 == setMst.Level1
                                     && item.IsDeleted != 1
                      ).ToList());

            // if is level 1
            if (setMst.Level1 > 0 && setMst.Level2 == 0 && setMstModel.IsDeleted == 1)
            {
                setMstList.AddRange(NoTrackingDataContext.SetMsts
                          .Where(item => item.SetKbn == setMst.SetKbn
                                         && item.SetKbnEdaNo == setMst.SetKbnEdaNo
                                         && item.GenerationId == setMst.GenerationId
                                         && item.Level1 > setMst.Level1
                                         && item.IsDeleted != 1
                          ).ToList());
                setMstList.Add(setMst);
            }

            setMstList = setMstList.Distinct().ToList();

            var setMstListResult = setMstList.Select(item => new SetMstModel(
                                              item.HpId,
                                              item.SetCd,
                                              item.SetKbn,
                                              item.SetKbnEdaNo,
                                              item.GenerationId,
                                              item.Level1,
                                              item.Level2,
                                              item.Level3,
                                              item.SetName ?? string.Empty,
                                              item.WeightKbn,
                                              item.Color,
                                              item.IsDeleted,
                                              item.IsGroup
                                          )).ToList();
            return setMstListResult;
        }
        catch (Exception ex)
        {
            var innerException = ex.InnerException?.ToString() ?? string.Empty;
            bool flag = false;
            if (HandleException(ex) == "23505" && innerException.Contains("23505") && innerException.Contains("unique constraint"))
            {
                int count = 0;
                while (count <= tryCountSave)
                {
                    try
                    {
                        flag = true;
                        RetrySaveSetMst(setMst);
                        break;
                    }
                    catch (Exception tryEx)
                    {
                        flag = false;
                        innerException = tryEx.InnerException?.ToString() ?? string.Empty;
                        if (HandleException(tryEx) == "23505" && innerException.Contains("23505") && innerException.Contains("unique constraint"))
                            if (HandleException(ex) == "23505" && innerException.Contains("23505") && innerException.Contains("unique constraint"))
                            {
                                count++;
                                //RetrySaveSetMst(setMst);
                                continue;
                            }
                        break;
                    }
                }
            }
            //Console.WriteLine(ex.Message);
            if (!flag)
            {
                RetrySaveSetMst(setMst);
            }
            return flag ? new(){new SetMstModel(
                    setMst.HpId,
                    setMst.SetCd,
                    setMst.SetKbn,
                    setMst.SetKbnEdaNo,
                    setMst.GenerationId,
                    setMst.Level1,
                    setMst.Level2,
                    setMst.Level3,
                    setMst.SetName ?? string.Empty,
                    setMst.WeightKbn,
                    setMst.Color,
                    setMst.IsDeleted,
                    setMst.IsGroup
                ) } : new();
        }
        finally
        {
            ReloadCache(1, setMst.GenerationId);
        }
    }

    private SetMst SaveSetMstAction(int userId, int sinDate, SetMstModel setMstModel)
    {
        SetMst setMst = new();

        // Check SetMstModel is delete?
        bool isDelete = setMstModel.IsDeleted == 1;
        var setKbnEdaNo = (setMstModel.SetKbnEdaNo - 1) > 0 ? setMstModel.SetKbnEdaNo - 1 : 0;

        // Create SetMst to save
        var oldSetMst = TrackingDataContext.SetMsts.FirstOrDefault(item => item.SetCd == setMstModel.SetCd
                                                                           && item.IsDeleted != 1);

        if (oldSetMst == null && !setMstModel.IsAddNew)
        {
            return new();
        }

        // if is add new with root set level is level 3 => stop progress
        else if (oldSetMst != null && oldSetMst.Level1 > 0 && oldSetMst.Level2 > 0 && oldSetMst.Level3 > 0 && setMstModel.IsAddNew)
        {
            return new();
        }
        oldSetMst = oldSetMst != null ? oldSetMst : new SetMst();

        setMst = ConvertSetMstModelToSetMst(oldSetMst, setMstModel, userId);
        setMst.GenerationId = GetGenerationId(setMst.HpId, sinDate);

        if (!isDelete)
        {
            // set status for IsDelete
            setMst.IsDeleted = 0;
            ChangeRpName(userId, setMst.SetCd, setMst.SetName ?? string.Empty);

            // If SetMst is add new
            if (setMstModel.IsAddNew)
            {
                var level = GetLevelSetMst(setMst.SetCd, setMst.SetKbn, setMst.SetKbnEdaNo, setMst.GenerationId);
                setMst.Level1 = level.level1;
                setMst.Level2 = level.level2;
                setMst.Level3 = level.level3;
                setMst.SetCd = 0;

                setMst.IsGroup = setMstModel.IsGroup;
                if (setMst.SetName == null || setMst.SetName.Length == 0)
                {
                    setMst.SetName = setMst.IsGroup == 1 ? defaultGroupName : defaultSetName;
                }
                setMst.CreateDate = CIUtil.GetJapanDateTimeNow();
                setMst.CreateId = userId;

                // Save SetMst 
                TrackingDataContext.SetMsts.Add(setMst);
            }
        }
        // Delete SetMst
        else
        {
            setMst.IsDeleted = 1;

            List<SetMst> listSetMstDelete = new();
            if (setMst.Level1 > 0 && setMst.Level2 == 0)
            {
                listSetMstDelete = TrackingDataContext.SetMsts
                                   .Where(item => item.SetKbn == setMst.SetKbn
                                                  && item.SetKbnEdaNo == setKbnEdaNo
                                                  && item.GenerationId == setMst.GenerationId
                                                  && item.Level1 == setMst.Level1
                                                  && item.IsDeleted != 1
                                   ).ToList();
            }
            else if (setMst.Level1 > 0 && setMst.Level2 > 0 && setMst.Level3 == 0)
            {
                listSetMstDelete = TrackingDataContext.SetMsts
                                   .Where(item => item.SetKbn == setMst.SetKbn
                                                  && item.SetKbnEdaNo == setKbnEdaNo
                                                  && item.GenerationId == setMst.GenerationId
                                                  && item.Level1 == setMst.Level1
                                                  && item.Level2 == setMst.Level2
                                                  && item.IsDeleted != 1
                                   ).ToList();
            }
            else if (setMst.Level1 > 0 && setMst.Level2 > 0 && setMst.Level3 > 0)
            {
                listSetMstDelete = TrackingDataContext.SetMsts
                                   .Where(item => item.SetKbn == setMst.SetKbn
                                                  && item.SetKbnEdaNo == setKbnEdaNo
                                                  && item.GenerationId == setMst.GenerationId
                                                  && item.Level1 == setMst.Level1
                                                  && item.Level2 == setMst.Level2
                                                  && item.Level3 == setMst.Level3
                                                  && item.IsDeleted != 1
                                   ).ToList();
            }
            foreach (var item in listSetMstDelete)
            {
                item.IsDeleted = 1;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }

            // update other item
            TrackingDataContext.SaveChanges();

            // is level 1
            List<SetMst> listUpdateLevel;
            if (setMst.Level1 > 0 && setMst.Level2 == 0)
            {
                listUpdateLevel = TrackingDataContext.SetMsts
                                  .Where(item => item.SetKbn == setMst.SetKbn
                                                 && item.SetKbnEdaNo == setKbnEdaNo
                                                 && item.GenerationId == setMst.GenerationId
                                                 && item.Level1 > setMst.Level1
                                                 && item.IsDeleted != 1
                                  ).ToList();
                foreach (var item in listUpdateLevel)
                {
                    item.Level1 = item.Level1 - 1;
                }
            }

            // is level 2
            if (setMst.Level1 > 0 && setMst.Level2 > 0 && setMst.Level3 == 0)
            {
                listUpdateLevel = TrackingDataContext.SetMsts
                                  .Where(item => item.SetKbn == setMst.SetKbn
                                                 && item.SetKbnEdaNo == setKbnEdaNo
                                                 && item.GenerationId == setMst.GenerationId
                                                 && item.Level1 == setMst.Level1
                                                 && item.Level2 > setMst.Level2
                                                 && item.IsDeleted != 1
                                  ).ToList();
                foreach (var item in listUpdateLevel)
                {
                    item.Level2 = item.Level2 - 1;
                }
            }

            // is level 3
            if (setMst.Level1 > 0 && setMst.Level2 > 0 && setMst.Level3 > 0)
            {
                listUpdateLevel = TrackingDataContext.SetMsts
                                  .Where(item => item.SetKbn == setMst.SetKbn
                                                 && item.SetKbnEdaNo == setKbnEdaNo
                                                 && item.GenerationId == setMst.GenerationId
                                                 && item.Level1 == setMst.Level1
                                                 && item.Level2 == setMst.Level2
                                                 && item.Level3 > setMst.Level3
                                                 && item.IsDeleted != 1
                                  ).ToList();
                foreach (var item in listUpdateLevel)
                {
                    item.Level3 = item.Level3 - 1;
                }
            }
        }
        return setMst;
    }

    [Obsolete]
    public (bool status, List<SetMstModel> setMstModels) ReorderSetMst(int userId, int hpId, int setCdDragItem, int setCdDropItem)
    {
        SetMst? dragItem = null;
        SetMst? dropItem = null;
        int? originDragLevel1 = null;
        int? originDropLevel1 = null;
        int generationId = 0;
        List<SetMstModel> setMstModels = new();
        bool status = false;
        try
        {
            var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
            status = executionStrategy.Execute(
                () =>
                {
                    using var transaction = TrackingDataContext.Database.BeginTransaction();
                    try
                    {
                        bool status = false;
                        dragItem = TrackingDataContext.SetMsts.FirstOrDefault(mst => mst.SetCd == setCdDragItem && mst.HpId == hpId);
                        dropItem = TrackingDataContext.SetMsts.FirstOrDefault(mst => mst.SetCd == setCdDropItem && mst.HpId == hpId);

                        // if dragItem is not exist
                        if (dragItem == null)
                        {
                            transaction.Rollback();
                            return false;
                        }

                        // if dropItem input is not exist
                        else if (dropItem == null && setCdDropItem != 0)
                        {
                            return false;
                        }

                        originDragLevel1 = dragItem.Level1;
                        originDropLevel1 = dropItem?.Level1;

                        // Get all SetMst with dragItem SetKbn and dragItem SetKbnEdaNo
                        var listSetMsts = TrackingDataContext.SetMsts.Where(mst => mst.SetKbn == dragItem.SetKbn && mst.SetKbnEdaNo == dragItem.SetKbnEdaNo && mst.HpId == dragItem.HpId && mst.Level1 > 0 && mst.IsDeleted != 1 && mst.GenerationId == dragItem.GenerationId).ToList();

                        if (dropItem != null)
                        {
                            // if dragItem SetKbnEdaNo diffirent dropItem SetKbnEdaNo or dragItem SetKbn different dropItem SetKbn
                            if (dragItem.SetKbnEdaNo != dropItem.SetKbnEdaNo || dragItem.SetKbn != dropItem.SetKbn)
                            {
                                return false;
                            }

                            // if dragItem is level1
                            if (dragItem.Level2 == 0 && dragItem.Level3 == 0)
                            {
                                status = DragItemIsLevel1(dragItem, dropItem, userId, listSetMsts);
                            }

                            // if dragItem is level2
                            else if (dragItem.Level2 > 0 && dragItem.Level3 == 0)
                            {
                                status = DragItemIsLevel2(dragItem, dropItem, userId, listSetMsts);
                            }

                            // if dragItem is level 3
                            else if (dragItem.Level3 > 0)
                            {
                                status = DragItemIsLevel3(dragItem, dropItem, userId, listSetMsts);
                            }
                        }
                        else if (setCdDropItem == 0)
                        {
                            status = DragItemWithDropItemIsLevel0(dragItem, userId, listSetMsts);
                        }

                        TrackingDataContext.SaveChanges();
                        transaction.Commit();
                        if (status)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                });
        }
        finally
        {
            generationId = dragItem?.GenerationId ?? 0;
            if (generationId > 0)
            {
                var setMsts = ReloadCache(1, generationId);
                setMstModels = GetDataAfterDragDrop(setMsts, dragItem ?? new(), dropItem ?? new(), originDragLevel1 ?? 0, originDropLevel1 ?? 0);
            }
        }
        return (status, setMstModels);
    }

    private List<SetMstModel> GetDataAfterDragDrop(IEnumerable<SetMstModel> setMsts, SetMst dragItem, SetMst dropItem, int originDragLevel1, int originDropLevel1)
    {
        List<SetMstModel> result = new();
        var setKbn = dragItem.SetKbn;
        var setKbnEdaNo = dragItem.SetKbnEdaNo;
        var generationId = dragItem.GenerationId;

        setMsts = setMsts.Where(item => item.SetKbn == setKbn && item.SetKbnEdaNo == setKbnEdaNo && item.GenerationId == generationId);

        if (originDropLevel1 != 0)
        {
            if (originDragLevel1 != originDropLevel1)
            {
                if (dragItem.Level2 == 0 && dropItem.Level2 == 0)
                {
                    if (originDragLevel1 > originDropLevel1)
                    {
                        result = setMsts.Where(item => originDropLevel1 <= item.Level1 && item.Level1 <= originDragLevel1).ToList();
                    }
                    if (originDragLevel1 < originDropLevel1)
                    {
                        result = setMsts.Where(item => originDragLevel1 <= item.Level1 && item.Level1 <= originDropLevel1).ToList();
                    }
                }
                else
                {
                    if (originDragLevel1 > originDropLevel1)
                    {
                        result = setMsts.Where(item => originDropLevel1 <= item.Level1).ToList();
                    }
                    if (originDragLevel1 < originDropLevel1)
                    {
                        result = setMsts.Where(item => originDragLevel1 <= item.Level1).ToList();
                    }
                    result.Add(new SetMstModel(
                                   dragItem.HpId,
                                   dragItem.SetCd,
                                   dragItem.SetKbn,
                                   dragItem.SetKbnEdaNo,
                                   dragItem.GenerationId,
                                   0,
                                   0,
                                   0,
                                   dragItem.SetName ?? string.Empty,
                                   dragItem.WeightKbn,
                                   dragItem.Color,
                                   1,
                                   dragItem.IsGroup
                           ));
                }
            }
            else
            {
                result = setMsts.Where(item => originDragLevel1 == item.Level1 || originDropLevel1 == item.Level1).ToList();
            }
        }
        else
        {
            result = setMsts.ToList();
        }

        return result;
    }

    [Obsolete]
    public List<SetMstModel> PasteSetMst(int hpId, int userId, int generationId, int setCdCopyItem, int setCdPasteItem, bool pasteToOtherGroup, int copySetKbnEdaNo, int copySetKbn, int pasteSetKbnEdaNo, int pasteSetKbn)
    {
        var setCd = 0;
        List<SetMstModel> setMstModels;
        if (pasteSetKbnEdaNo <= 0 && pasteSetKbn <= 0)
        {
            return new();
        }

        try
        {
            if (pasteToOtherGroup && setCdCopyItem == 0 && setCdPasteItem == 0)
            {

                setCd = CopyPasteGroupSetMst(hpId, userId, generationId, copySetKbnEdaNo, copySetKbn, pasteSetKbnEdaNo, pasteSetKbn);
            }
            else if (setCdCopyItem > 0)
            {
                setCd = CopyPasteItemSetMst(hpId, userId, setCdCopyItem, setCdPasteItem, pasteToOtherGroup, generationId, pasteSetKbnEdaNo, pasteSetKbn);
            }
        }
        finally
        {
            var setMsts = ReloadCache(1, generationId);
            var rootSet = setMsts.FirstOrDefault(s => s.SetCd == setCd);
            setMstModels = setMsts.Where(s => s.HpId == rootSet?.HpId && s.SetKbn == rootSet.SetKbn && s.SetKbnEdaNo == rootSet.SetKbnEdaNo && s.GenerationId == rootSet.GenerationId && (rootSet.Level1 == 0 || (rootSet.Level1 > 0 && s.Level1 == rootSet.Level1))).ToList();
        }

        return setMstModels;
    }

    public bool CheckExistSetMstBySetCd(int setCd)
    {
        return NoTrackingDataContext.SetMsts.Any(item => item.SetCd == setCd);
    }

    public bool CheckExistSetMstBySetCd(int hpId, List<int> setCdList)
    {
        setCdList = setCdList.Distinct().ToList();
        return NoTrackingDataContext.SetMsts.Count(item => item.HpId == hpId && setCdList.Contains(item.SetCd)) == setCdList.Count;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    #region private method

    // GetGenerationId by hpId and sindate
    private int GetGenerationId(int hpId, int sinDate)
    {
        int generationId = 0;
        var generation = NoTrackingDataContext.SetGenerationMsts.Where(x => x.HpId == hpId && x.StartDate <= sinDate && x.IsDeleted == 0)
                                                                .OrderByDescending(x => x.StartDate)
                                                                .FirstOrDefault();
        if (generation != null)
        {
            generationId = generation.GenerationId;
        }
        return generationId;
    }

    private SetMst ConvertSetMstModelToSetMst(SetMst setMst, SetMstModel setMstModel, int userId)
    {
        setMst.HpId = setMstModel.HpId;
        setMst.SetCd = setMstModel.SetCd;
        setMst.SetKbn = setMstModel.SetKbn;
        setMst.SetKbnEdaNo = (setMstModel.SetKbnEdaNo - 1) > 0 ? setMstModel.SetKbnEdaNo - 1 : 0;
        if (setMstModel.IsAddNew)
        {
            setMst.GenerationId = setMstModel.GenerationId;
            setMst.Level1 = setMstModel.Level1;
            setMst.Level2 = setMstModel.Level2;
            setMst.Level3 = setMstModel.Level3;
        }
        setMst.SetName = setMstModel.SetName;
        setMst.Color = setMstModel.Color;
        setMst.WeightKbn = setMstModel.WeightKbn;
        setMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
        setMst.UpdateId = userId;
        return setMst;
    }

    [Obsolete]
    private int CopyPasteItemSetMst(int hpId, int userId, int setCdCopyItem, int setCdPasteItem, bool pasteToOtherGroup, int generationId, int pasteSetKbnEdaNo, int pasteSetKbn)
    {
        int setCd = -1;
        pasteSetKbnEdaNo = pasteSetKbnEdaNo - 1;
        var copyItem = NoTrackingDataContext.SetMsts.FirstOrDefault(mst => mst.SetCd == setCdCopyItem && mst.HpId == hpId && mst.IsDeleted != 1);
        // If copy item is null => return false
        if (copyItem == null)
        {
            return setCd;
        }

        var pasteItem = NoTrackingDataContext.SetMsts.FirstOrDefault(mst => mst.SetCd == setCdPasteItem && mst.HpId == hpId && mst.IsDeleted != 1);
        // if paste item is null then paste item cd is lager than 0 or pasteSetKbnEdaNo equal 0 or pasteSetKbn equal 0 => return false
        if (pasteItem == null && setCdPasteItem != 0)
        {
            return setCd;
        }
        // if SetKbnEdaNo of pasteItem not equal pasteSetKbnEdaNo or SetKbn of pasteItem is not equal pasteSetKbn => return false
        else if (pasteItem != null && (pasteItem.SetKbnEdaNo != pasteSetKbnEdaNo || pasteItem.SetKbn != pasteSetKbn))
        {
            return setCd;
        }

        // if group of copy item is same group of paste item
        var listSetMsts = NoTrackingDataContext.SetMsts.Where(mst => mst.GenerationId == generationId && mst.SetKbn == copyItem.SetKbn && mst.SetKbnEdaNo == copyItem.SetKbnEdaNo && mst.HpId == copyItem.HpId && mst.Level1 > 0 && mst.IsDeleted != 1).ToList();
        // if is paste to other group and paste item is not null
        if (pasteToOtherGroup && pasteItem != null)
        {
            listSetMsts.AddRange(NoTrackingDataContext.SetMsts.Where(mst => mst.GenerationId == generationId && mst.SetKbn == pasteItem.SetKbn && mst.SetKbnEdaNo == pasteItem.SetKbnEdaNo && mst.HpId == pasteItem.HpId && mst.Level1 > 0 && mst.IsDeleted != 1).ToList());
        }
        // if is paste to other group and paste item is not null
        else if (pasteToOtherGroup && pasteItem == null)
        {
            listSetMsts.AddRange(NoTrackingDataContext.SetMsts.Where(mst => mst.GenerationId == generationId && mst.SetKbn == pasteSetKbn && mst.SetKbnEdaNo == pasteSetKbnEdaNo && mst.HpId == hpId && mst.Level1 > 0 && mst.IsDeleted != 1).ToList());
        }
        if (pasteItem != null)
        {
            if (CountLevelItem(copyItem, listSetMsts) + GetLevelItem(pasteItem) > 3)
            {
                return setCd;
            }
            if ((copyItem.SetKbn != pasteItem.SetKbn || copyItem.SetKbnEdaNo != pasteItem.SetKbnEdaNo) && !pasteToOtherGroup)
            {
                return setCd;
            }
            if (GetLevelItem(pasteItem) == 1)
            {
                // get index for paste
                var lastItemLevel2 = listSetMsts.Where(item => item.Level1 == pasteItem.Level1 && item.Level2 > 0 && item.Level3 == 0 && item.SetKbn == pasteItem.SetKbn && item.SetKbnEdaNo == pasteItem.SetKbnEdaNo).OrderByDescending(item => item.Level2).FirstOrDefault();
                int indexPaste = (lastItemLevel2 != null ? lastItemLevel2.Level2 : 0) + 1;
                setCd = PasteItemAction(indexPaste, pasteSetKbnEdaNo, pasteSetKbn, userId, copyItem, pasteItem, listSetMsts);
            }
            else if (GetLevelItem(pasteItem) == 2)
            {
                // get index for paste
                var lastItemLevel3 = listSetMsts.Where(item => item.Level1 == pasteItem.Level1 && item.Level2 == pasteItem.Level2 && item.Level3 > 0 && item.SetKbn == pasteItem.SetKbn && item.SetKbnEdaNo == pasteItem.SetKbnEdaNo).OrderByDescending(item => item.Level3).FirstOrDefault();
                int indexPaste = (lastItemLevel3 != null ? lastItemLevel3.Level3 : 0) + 1;
                setCd = PasteItemAction(indexPaste, pasteSetKbnEdaNo, pasteSetKbn, userId, copyItem, pasteItem, listSetMsts);
            }
        }
        else
        {
            // get index for paste
            var lastItemLevel1 = listSetMsts.Where(item => item.Level2 == 0 && item.Level3 == 0 && item.SetKbn == pasteSetKbn && item.SetKbnEdaNo == pasteSetKbnEdaNo).OrderByDescending(item => item.Level1).FirstOrDefault();
            int indexPaste = (lastItemLevel1 != null ? lastItemLevel1.Level1 : 0) + 1;
            setCd = PasteItemAction(indexPaste, pasteSetKbnEdaNo, pasteSetKbn, userId, copyItem, null, listSetMsts);
        }

        return setCd;
    }

    private int CopyPasteGroupSetMst(int hpId, int userId, int generationId, int copySetKbnEdaNo, int copySetKbn, int pasteSetKbnEdaNo, int pasteSetKbn)
    {
        int setCd = -1;
        copySetKbnEdaNo = copySetKbnEdaNo - 1;
        pasteSetKbnEdaNo = pasteSetKbnEdaNo - 1;
        var listCopySetMsts = NoTrackingDataContext.SetMsts.Where(mst => mst.GenerationId == generationId && mst.SetKbn == copySetKbn && mst.SetKbnEdaNo == copySetKbnEdaNo && mst.IsDeleted != 1 && mst.HpId == hpId).ToList();
        if (!listCopySetMsts.Any())
        {
            return setCd;
        }
        var listPasteSetMsts = NoTrackingDataContext.SetMsts.Where(mst => mst.GenerationId == generationId && mst.SetKbn == pasteSetKbn && mst.SetKbnEdaNo == pasteSetKbnEdaNo && mst.IsDeleted != 1 && mst.HpId == hpId);
        var lastItemLevel1 = listPasteSetMsts.Where(item => item.Level2 == 0 && item.Level3 == 0 && item.SetKbn == pasteSetKbn && item.SetKbnEdaNo == pasteSetKbnEdaNo).OrderByDescending(item => item.Level1).FirstOrDefault();
        int indexPaste = (lastItemLevel1 != null ? lastItemLevel1.Level1 : 0) + 1;
        return PasteGroupAction(userId, indexPaste, pasteSetKbnEdaNo, pasteSetKbn, listCopySetMsts);
    }

    [Obsolete]
    private int PasteItemAction(int indexPaste, int pasteSetKbnEdaNo, int pasteSetKbn, int userId, SetMst copyItem, SetMst? pasteItem, List<SetMst> listSetMsts)
    {
        int setCd = -1;
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using (var transaction = TrackingDataContext.Database.BeginTransaction())
                {
                    try
                    {
                        // paste SetMst to list super set
                        List<SetMst> listCopyItems = new();
                        List<SetMst> listPasteItems = new();
                        switch (GetLevelItem(copyItem))
                        {
                            case 1:
                                listCopyItems = listSetMsts.Where(item => item.GenerationId == copyItem.GenerationId && item.Level1 == copyItem.Level1 && item.SetKbnEdaNo == copyItem.SetKbnEdaNo && item.SetKbn == copyItem.SetKbn).ToList();
                                break;
                            case 2:
                                listCopyItems = listSetMsts.Where(item => item.GenerationId == copyItem.GenerationId && item.Level1 == copyItem.Level1 && item.Level2 == copyItem.Level2 && item.SetKbnEdaNo == copyItem.SetKbnEdaNo && item.SetKbn == copyItem.SetKbn).ToList();
                                break;
                            case 3:
                                listCopyItems = listSetMsts.Where(item => item.GenerationId == copyItem.GenerationId && item.Level1 == copyItem.Level1 && item.Level2 == copyItem.Level2 && item.Level3 == copyItem.Level3 && item.SetKbnEdaNo == copyItem.SetKbnEdaNo && item.SetKbn == copyItem.SetKbn).ToList();
                                break;
                        }

                        var rootSet = listCopyItems.FirstOrDefault(item => item.SetCd == copyItem.SetCd);
                        var rootSetCd = 0;
                        if (rootSet != null)
                        {
                            rootSetCd = rootSet.SetCd;
                            listCopyItems.Remove(rootSet);

                            rootSet.SetCd = 0;
                            rootSet.SetKbn = pasteSetKbn;
                            rootSet.SetKbnEdaNo = pasteSetKbnEdaNo;
                            rootSet.CreateDate = CIUtil.GetJapanDateTimeNow();
                            rootSet.CreateId = userId;
                            rootSet.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            rootSet.UpdateId = userId;
                            rootSet.IsDeleted = DeleteTypes.Deleted;
                            TrackingDataContext.SetMsts.Add(rootSet);
                            TrackingDataContext.SaveChanges();
                            setCd = rootSet.SetCd;
                            // Convert SetMst copy to SetMst paste
                            foreach (var item in listCopyItems)
                            {
                                SetMst setMst = item.DeepClone();
                                setMst.SetCd = 0;
                                setMst.SetKbn = pasteSetKbn;
                                setMst.SetKbnEdaNo = pasteSetKbnEdaNo;
                                setMst.CreateDate = CIUtil.GetJapanDateTimeNow();
                                setMst.CreateId = userId;
                                setMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                                setMst.UpdateId = userId;
                                setMst.IsDeleted = DeleteTypes.Deleted;
                                listPasteItems.Add(setMst);
                            }

                            TrackingDataContext.SetMsts.AddRange(listPasteItems);
                            TrackingDataContext.SaveChanges();
                            listPasteItems.Add(rootSet);
                        }

                        // get paste content item
                        Dictionary<int, SetMst> dictionarySetMstMap = new();
                        foreach (var copy in listCopyItems)
                        {
                            var pasteItemToMap = listPasteItems.FirstOrDefault(paste => paste.Level1 == copy.Level1 && paste.Level2 == copy.Level2 && paste.Level3 == copy.Level3);
                            if (pasteItemToMap != null && !dictionarySetMstMap.ContainsKey(copy.SetCd))
                            {
                                dictionarySetMstMap.Add(copy.SetCd, pasteItemToMap);
                            }
                        }

                        var listCopySetCds = listCopyItems.Select(item => item.SetCd).ToList();
                        if (rootSet != null && !dictionarySetMstMap.ContainsKey(rootSet.SetCd))
                        {
                            listCopySetCds.Add(rootSetCd);
                            var pasteItemToMap = listPasteItems.FirstOrDefault(paste => paste.Level1 == rootSet.Level1 && paste.Level2 == rootSet.Level2 && paste.Level3 == rootSet.Level3);
                            if (pasteItemToMap != null)
                            {
                                dictionarySetMstMap.Add(rootSetCd, pasteItemToMap);
                            }
                        }
                        AddNewItemToSave(userId, listCopySetCds, dictionarySetMstMap);
                        TrackingDataContext.SaveChanges();
                        // Set level for item
                        try
                        {
                            if (pasteItem != null)
                            {
                                pasteItem.IsDeleted = DeleteTypes.None;
                            }
                            foreach (var item in listPasteItems)
                            {
                                item.IsDeleted = DeleteTypes.None;
                            }
                            ReSetLevelForItem(indexPaste, copyItem, pasteItem, listPasteItems);

                            TrackingDataContext.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            bool flag = false;
                            var innerException = ex.InnerException?.ToString() ?? string.Empty;
                            if (HandleException(ex) == "23505" && innerException.Contains("23505") && innerException.Contains("unique constraint"))
                            {
                                var count = 0;
                                while (count <= tryCountSave)
                                {
                                    try
                                    {
                                        flag = true;
                                        RetryCopyPasteSetMst(copyItem, pasteItem, listPasteItems);

                                        TrackingDataContext.SaveChanges();
                                        transaction.Commit();
                                        break;
                                    }
                                    catch (Exception tryEx)
                                    {
                                        flag = false;
                                        transaction.Rollback();
                                        innerException = tryEx.InnerException?.ToString() ?? string.Empty;
                                        if (HandleException(tryEx) == "23505" && innerException.Contains("23505") && innerException.Contains("unique constraint"))
                                        {
                                            //RetryCopyPasteSetMst(copyItem, pasteItem, listPasteItems);

                                            //TrackingDataContext.SaveChanges();
                                            //transaction.Commit();
                                            count++;
                                            continue;
                                        }
                                        break;
                                        throw;
                                    }
                                }
                            }
                            if (!flag)
                            {
                                RetryCopyPasteSetMst(copyItem, pasteItem, listPasteItems);

                                TrackingDataContext.SaveChanges();
                                transaction.Commit();
                            }
                            Console.WriteLine(ex.Message);
                        }

                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            );
        return setCd;
    }

    private int PasteGroupAction(int userId, int pasteIndex, int pasteSetKbnEdaNo, int pasteSetKbn, List<SetMst> listCopySetMsts)
    {
        int setCd = -1;
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using (var transaction = TrackingDataContext.Database.BeginTransaction())
                {
                    try
                    {
                        var listLevel1 = listCopySetMsts.Select(item => item.Level1).OrderBy(item => item).Distinct().ToList();
                        // Create dic to update level1
                        Dictionary<int, int> dicLevel1Updates = new();
                        int indexUpdate = pasteIndex;
                        foreach (var item in listLevel1)
                        {
                            dicLevel1Updates.Add(item, indexUpdate);
                            indexUpdate++;
                        }

                        // Convert SetMst copy to SetMst paste
                        List<SetMst> listPasteItems = new();
                        foreach (var item in listCopySetMsts)
                        {
                            SetMst setMst = item.DeepClone();
                            setMst.SetCd = 0;
                            setMst.Level1 = dicLevel1Updates[item.Level1];
                            setMst.SetKbn = pasteSetKbn;
                            setMst.SetKbnEdaNo = pasteSetKbnEdaNo;
                            setMst.CreateDate = CIUtil.GetJapanDateTimeNow();
                            setMst.CreateId = userId;
                            setMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                            setMst.UpdateId = userId;
                            listPasteItems.Add(setMst);
                        }
                        TrackingDataContext.SetMsts.AddRange(listPasteItems);
                        TrackingDataContext.SaveChanges();

                        // get paste content item
                        Dictionary<int, SetMst> dictionarySetMstMap = new();
                        foreach (var copy in listCopySetMsts)
                        {
                            var pasteItemToMap = listPasteItems.FirstOrDefault(paste => paste.Level1 == dicLevel1Updates[copy.Level1] && paste.Level2 == copy.Level2 && paste.Level3 == copy.Level3);
                            if (pasteItemToMap != null)
                            {
                                dictionarySetMstMap.Add(copy.SetCd, pasteItemToMap);
                            }
                        }

                        var listCopySetCds = listCopySetMsts.Select(item => item.SetCd).ToList();
                        AddNewItemToSave(userId, listCopySetCds, dictionarySetMstMap);

                        TrackingDataContext.SaveChanges();
                        transaction.Commit();
                        var firstSetMstResult = listPasteItems.FirstOrDefault(item => item.Level1 == pasteIndex && item.Level2 == 0 && item.Level3 == 0);
                        setCd = firstSetMstResult != null ? firstSetMstResult.SetCd : -1;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            );
        return setCd;
    }

    private void ReSetLevelForItem(int indexPaste, SetMst copyItem, SetMst? pasteItem, List<SetMst> listPasteItems)
    {
        if (pasteItem != null)
        {
            switch (GetLevelItem(pasteItem))
            {
                // if paste item is level 1
                case 1:
                    // if copy item is level 1
                    if (GetLevelItem(copyItem) == 1)
                    {
                        // level 2 => level 3
                        var listUpdateLevel2 = listPasteItems.Where(x => x.Level2 > 0 && x.Level3 == 0).ToList();
                        foreach (var item in listUpdateLevel2)
                        {
                            item.Level3 = item.Level2;
                            item.Level2 = indexPaste;
                            item.Level1 = pasteItem.Level1;
                        }

                        // level 1 => level 2
                        var listUpdateLevel1 = listPasteItems.Where(x => x.Level2 == 0 && x.Level3 == 0).ToList();
                        foreach (var item in listUpdateLevel1)
                        {
                            item.Level1 = pasteItem.Level1;
                            item.Level2 = indexPaste;
                            item.Level3 = 0;
                        }
                    }
                    // if copy item is level 2
                    else if (GetLevelItem(copyItem) == 2)
                    {
                        foreach (var item in listPasteItems)
                        {
                            item.Level1 = pasteItem.Level1;
                            item.Level2 = indexPaste;
                        }
                    }
                    // if copy item is level 3
                    else if (GetLevelItem(copyItem) == 3)
                    {
                        foreach (var item in listPasteItems)
                        {
                            item.Level1 = pasteItem.Level1;
                            item.Level2 = indexPaste;
                            item.Level3 = 0;
                        }
                    }
                    break;
                // if paste item is level 2
                case 2:
                    foreach (var item in listPasteItems)
                    {
                        item.Level1 = pasteItem.Level1;
                        item.Level2 = pasteItem.Level2;
                        item.Level3 = indexPaste;
                    }
                    break;
            }
        }
        else
        {
            switch (GetLevelItem(copyItem))
            {
                case 1:
                    foreach (var item in listPasteItems)
                    {
                        item.Level1 = indexPaste;
                    }
                    break;
                case 2:
                    // level 2 => level 1
                    var listUpdateLevel2 = listPasteItems.Where(item => item.Level2 > 0 && item.Level3 == 0).ToList();
                    foreach (var item in listUpdateLevel2)
                    {
                        item.Level1 = indexPaste;
                        item.Level2 = 0;
                        item.Level3 = 0;
                    }

                    // level 3 => level 2
                    var listUpdateLevel3 = listPasteItems.Where(item => item.Level2 > 0 && item.Level3 > 0).ToList();
                    foreach (var item in listUpdateLevel3)
                    {
                        item.Level1 = indexPaste;
                        item.Level2 = item.Level3;
                        item.Level3 = 0;
                    }
                    break;
                case 3:
                    foreach (var item in listPasteItems)
                    {
                        item.Level1 = indexPaste;
                        item.Level2 = 0;
                        item.Level3 = 0;
                    }
                    break;
            }
        }
    }

    private int GetLevelItem(SetMst setMst)
    {
        int level = 0;
        if (setMst.Level2 == 0 && setMst.Level3 == 0)
        {
            level = 1;
        }
        else if (setMst.Level2 > 0 && setMst.Level3 == 0)
        {
            level = 2;
        }
        else if (setMst.Level3 > 0)
        {
            level = 3;
        }
        return level;
    }

    private int CountLevelItem(SetMst setMst, List<SetMst> setMsts)
    {
        int count = 1;
        List<SetMst> listSamelevel;

        switch (GetLevelItem(setMst))
        {
            case 1:
                listSamelevel = setMsts.Where(item => item.Level1 == setMst.Level1 && item.SetKbnEdaNo == setMst.SetKbnEdaNo && item.SetKbn == setMst.SetKbn).ToList();
                if (listSamelevel.Any(item => item.Level2 > 0 && item.Level3 == 0))
                {
                    count = 2;
                }
                if (listSamelevel.Any(item => item.Level3 > 0))
                {
                    count = 3;
                }
                break;
            case 2:
                listSamelevel = setMsts.Where(item => item.Level1 == setMst.Level1 && item.Level2 == setMst.Level2).ToList();
                if (listSamelevel.Any(item => item.Level3 > 0))
                {
                    count = 2;
                }
                break;
        }
        return count;
    }

    private void AddNewItemToSave(int userId, List<int> listCopySetCds, Dictionary<int, SetMst> dictionarySetMstMap)
    {
        listCopySetCds = listCopySetCds.Distinct().ToList();
        // Order inf
        var listCopySetOrderInfs = NoTrackingDataContext.SetOdrInf.Where(item => listCopySetCds.Contains(item.SetCd) && item.IsDeleted != 1).ToList();
        var listPasteSetOrderInfs = new List<SetOdrInf>();
        foreach (var item in listCopySetOrderInfs)
        {
            SetOdrInf order = item.DeepClone();
            order.Id = 0;
            order.SetCd = dictionarySetMstMap[order.SetCd].SetCd;
            order.CreateDate = CIUtil.GetJapanDateTimeNow();
            order.CreateId = userId;
            order.UpdateDate = CIUtil.GetJapanDateTimeNow();
            order.UpdateId = userId;
            listPasteSetOrderInfs.Add(order);
        }
        TrackingDataContext.SetOdrInf.AddRange(listPasteSetOrderInfs);

        // Order inf detail
        var listCopySetOrderInfDetails = NoTrackingDataContext.SetOdrInfDetail.Where(item => listCopySetCds.Contains(item.SetCd)).ToList();
        var listPasteSetOrderInfDetails = new List<SetOdrInfDetail>();
        foreach (var item in listCopySetOrderInfDetails)
        {
            SetOdrInfDetail detail = item.DeepClone();
            detail.SetCd = dictionarySetMstMap[detail.SetCd].SetCd;
            listPasteSetOrderInfDetails.Add(detail);
        }
        TrackingDataContext.SetOdrInfDetail.AddRange(listPasteSetOrderInfDetails);

        // Karte inf
        var listCopySetKarteInfs = NoTrackingDataContext.SetKarteInf.Where(item => listCopySetCds.Contains(item.SetCd) && item.IsDeleted != 1).ToList();
        var listPasteSetKarteInfs = new List<SetKarteInf>();
        foreach (var item in listCopySetKarteInfs)
        {
            SetKarteInf karte = item.DeepClone();
            karte.SetCd = dictionarySetMstMap[karte.SetCd].SetCd;
            karte.CreateDate = CIUtil.GetJapanDateTimeNow();
            karte.CreateId = userId;
            karte.UpdateDate = CIUtil.GetJapanDateTimeNow();
            karte.UpdateId = userId;
            listPasteSetKarteInfs.Add(karte);
        }
        TrackingDataContext.SetKarteInf.AddRange(listPasteSetKarteInfs);

        // Karte Image inf
        var listCopySetKarteImageInfs = NoTrackingDataContext.SetKarteImgInf.Where(item => listCopySetCds.Contains(item.SetCd)).ToList();
        var listPasteSetKarteImageInfs = new List<SetKarteImgInf>();
        foreach (var item in listCopySetKarteImageInfs)
        {
            SetKarteImgInf karteImage = item.DeepClone();
            karteImage.SetCd = dictionarySetMstMap[karteImage.SetCd].SetCd;
            karteImage.Id = 0;
            listPasteSetKarteImageInfs.Add(karteImage);
        }
        TrackingDataContext.SetKarteInf.AddRange(listPasteSetKarteInfs);

        // Set byomei
        var listCopySetByomeies = NoTrackingDataContext.SetByomei.Where(item => listCopySetCds.Contains(item.SetCd) && item.IsDeleted != 1).ToList();
        var listPasteSetByomeies = new List<SetByomei>();
        foreach (var item in listCopySetByomeies)
        {
            SetByomei karte = item.DeepClone();
            karte.SetCd = dictionarySetMstMap[karte.SetCd].SetCd;
            karte.CreateDate = CIUtil.GetJapanDateTimeNow();
            karte.CreateId = userId;
            karte.UpdateDate = CIUtil.GetJapanDateTimeNow();
            karte.UpdateId = userId;
            karte.Id = 0;
            listPasteSetByomeies.Add(karte);
        }
        TrackingDataContext.SetByomei.AddRange(listPasteSetByomeies);
    }

    [Obsolete]
    private bool DragItemIsLevel1(SetMst dragItem, SetMst dropItem, int userId, List<SetMst> listSetMsts)
    {
        var listDragItem = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1).ToList();
        // if drop item is level 1
        if (dropItem.Level2 == 0 && dropItem.Level3 == 0)
        {
            if (dragItem.Level1 > dropItem.Level1)
            {
                var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > dropItem.Level1 && mst.Level1 < dragItem.Level1).ToList();
                foreach (var item in listDragItem)
                {
                    item.Level1 = dropItem.Level1 + 1;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    item.IsDeleted = DeleteTypes.Deleted;
                }
                foreach (var item in listUpdateLevel1)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                }
                //LevelDown(1, userId, listUpdateLevel1);
                LevelDown(1, userId, listUpdateLevel1);
                TrackingDataContext.SaveChanges();

                foreach (var item in listUpdateLevel1)
                {
                    item.IsDeleted = DeleteTypes.None;
                }
                foreach (var item in listDragItem)
                {
                    item.IsDeleted = DeleteTypes.None;
                }

                TrackingDataContext.SaveChanges();
            }
            else if (dragItem.Level1 < dropItem.Level1)
            {
                var levelDrop = dropItem.Level1;
                var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > dragItem.Level1 && mst.Level1 <= dropItem.Level1).ToList();

                foreach (var item in listDragItem)
                {
                    item.Level1 = levelDrop;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    item.IsDeleted = DeleteTypes.Deleted;
                }

                LevelUp(1, userId, listUpdateLevel1);
                foreach (var item in listUpdateLevel1)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                }
                TrackingDataContext.SaveChanges();
                foreach (var item in listDragItem)
                {
                    item.IsDeleted = DeleteTypes.None;
                }
                foreach (var item in listUpdateLevel1)
                {
                    item.IsDeleted = DeleteTypes.None;
                }
                TrackingDataContext.SaveChanges();
            }
            else
            {
                return false;
            }

        }
        // if drop item is level 2
        else if (dropItem.Level2 > 0 && dropItem.Level3 == 0)
        {
            // if same level1 => return false
            if (dragItem.Level1 == dropItem.Level1)
            {
                return false;
            }
            // if level1 has children => return false
            if (listDragItem?.Count(item => item.Level2 > 0) > 0)
            {
                return false;
            }
            var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > 0).OrderByDescending(mst => mst.Level3).ToList();
            var listUpdateLevel3SkipLast = listUpdateLevel3.Skip(1).ToList();
            var lastLevel3 = listUpdateLevel3.FirstOrDefault();

            var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > dragItem.Level1).ToList();
            LevelUp(1, userId, listUpdateLevel1);
            foreach (var item in listUpdateLevel1)
            {
                item.IsDeleted = DeleteTypes.Deleted;
            }

            dragItem.Level1 = dropItem.Level1;
            dragItem.Level2 = dropItem.Level2;
            dragItem.Level3 = 1;
            dragItem.IsDeleted = DeleteTypes.Deleted;
            //LevelDown(3, userId, listUpdateLevel3);
            foreach (var item in listUpdateLevel3SkipLast)
            {
                item.IsDeleted = DeleteTypes.Deleted;
            }
            LevelDown(3, userId, listUpdateLevel3SkipLast);

            foreach (var item in listUpdateLevel1)
            {
                item.IsDeleted = DeleteTypes.Deleted;
            }
            if (lastLevel3 != null)
            {
                SaveLevelDown(3, userId, new List<SetMst> { lastLevel3 });
            }
            else
            {
                TrackingDataContext.SaveChanges();
            }

            dragItem.IsDeleted = DeleteTypes.None;
            foreach (var item in listUpdateLevel1)
            {
                item.IsDeleted = DeleteTypes.None;

            }
            foreach (var item in listUpdateLevel3SkipLast)
            {
                item.IsDeleted = DeleteTypes.None;
            }
            TrackingDataContext.SaveChanges();
        }
        // if drop item is level 3 return false
        else if (dropItem.Level3 > 0)
        {
            return false;
        }
        return true;
    }

    [Obsolete]
    private bool DragItemIsLevel2(SetMst dragItem, SetMst dropItem, int userId, List<SetMst> listSetMsts)
    {
        // if dropItem is level1
        if (dropItem.Level2 == 0)
        {
            if (dragItem.Level1 == dropItem.Level1)
            {
                var listDropUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > 0 && mst.Level2 < dragItem.Level2).ToList();
                var listDragItem = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2).ToList();
                foreach (var item in listDragItem)
                {
                    item.Level2 = 1;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    item.IsDeleted = DeleteTypes.Deleted;
                }
                LevelDown(2, userId, listDropUpdateLevel2);
                foreach (var item in listDropUpdateLevel2)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                }
                TrackingDataContext.SaveChanges();
                foreach (var item in listDragItem)
                {
                    item.IsDeleted = DeleteTypes.None;
                }
                foreach (var item in listDropUpdateLevel2)
                {
                    item.IsDeleted = DeleteTypes.None;
                }
                TrackingDataContext.SaveChanges();
            }
            else
            {
                var listDrag = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2).ToList();

                var listDropUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 > 0).ToList() ?? new();
                var maxLevel2 = listDropUpdateLevel2.Count == 0 ? 0 : listDropUpdateLevel2.Max(l => l.Level2);
                var maxDropUpdateLevel2 = listDropUpdateLevel2.Where(m => m.Level2 == maxLevel2).ToList();
                var rootMaxDropUpdateLevel2 = maxDropUpdateLevel2.FirstOrDefault(m => m.Level3 == 0);
                var listDropUpdateLevel2ExceptMaxLevel = listDropUpdateLevel2.Where(l => !maxDropUpdateLevel2.Contains(l)).ToList();
                int dropItemLevel1 = dropItem.Level1;
                //LevelDown(2, userId, listDropUpdateLevel2);
                var listDragUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > dragItem.Level2).ToList() ?? new();
                foreach (var item in listDrag)
                {
                    item.Level1 = dropItemLevel1;
                    item.Level2 = 1;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    item.IsDeleted = DeleteTypes.Deleted;
                }
                LevelDown(2, userId, listDropUpdateLevel2ExceptMaxLevel);

                foreach (var item in listDropUpdateLevel2ExceptMaxLevel)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                }

                LevelUp(2, userId, listDragUpdateLevel2);
                foreach (var item in listDragUpdateLevel2)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                }
                if (rootMaxDropUpdateLevel2 != null)
                {
                    SaveLevelDown(2, userId, new List<SetMst> { rootMaxDropUpdateLevel2 });
                    foreach (var item in maxDropUpdateLevel2.Where(m => m != rootMaxDropUpdateLevel2).ToList())
                    {
                        item.Level2 = rootMaxDropUpdateLevel2.Level2;
                        item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        item.UpdateId = userId;
                    }
                }
                else
                {
                    TrackingDataContext.SaveChanges();
                }

                foreach (var item in listDrag)
                {
                    item.IsDeleted = DeleteTypes.None;
                }

                foreach (var item in listDragUpdateLevel2)
                {
                    item.IsDeleted = DeleteTypes.None;
                }

                foreach (var item in listDropUpdateLevel2ExceptMaxLevel)
                {
                    item.IsDeleted = DeleteTypes.None;
                }

                TrackingDataContext.SaveChanges();
            }
        }
        // if dropItem is level2
        else if (dropItem.Level2 > 0 && dropItem.Level3 == 0)
        {
            if (dragItem.Level1 == dropItem.Level1)
            {
                var listDragUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2).ToList();
                if (dragItem.Level2 > dropItem.Level2)
                {
                    var listUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 > dropItem.Level2 && mst.Level2 < dragItem.Level2).ToList();

                    foreach (var item in listDragUpdateLevel2)
                    {
                        item.Level2 = dropItem.Level2 + 1;
                        item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        item.UpdateId = userId;
                        item.IsDeleted = DeleteTypes.Deleted;
                    }
                    //LevelDown(2, userId, listUpdateLevel2);
                    LevelDown(2, userId, listUpdateLevel2);
                    foreach (var item in listUpdateLevel2)
                    {
                        item.IsDeleted = DeleteTypes.Deleted;
                    }
                    TrackingDataContext.SaveChanges();
                    foreach (var item in listDragUpdateLevel2)
                    {
                        item.IsDeleted = DeleteTypes.None;
                    }
                    foreach (var item in listUpdateLevel2)
                    {
                        item.IsDeleted = DeleteTypes.None;
                    }
                    TrackingDataContext.SaveChanges();
                }
                else if (dragItem.Level2 < dropItem.Level2)
                {
                    var listUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 > dragItem.Level2 && mst.Level2 <= dropItem.Level2).ToList();
                    var dropLevel = dropItem.Level2;
                    LevelUp(2, userId, listUpdateLevel2);
                    foreach (var item in listUpdateLevel2)
                    {
                        item.IsDeleted = DeleteTypes.Deleted;
                    }
                    foreach (var item in listDragUpdateLevel2)
                    {
                        item.Level2 = dropLevel;
                        item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        item.UpdateId = userId;
                        item.IsDeleted = DeleteTypes.Deleted;
                    }
                    TrackingDataContext.SaveChanges();

                    foreach (var item in listUpdateLevel2)
                    {
                        item.IsDeleted = DeleteTypes.None;
                    }
                    foreach (var item in listDragUpdateLevel2)
                    {
                        item.IsDeleted = DeleteTypes.None;
                    }

                    TrackingDataContext.SaveChanges();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (listSetMsts?.Count(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > 0) > 0)
                {
                    return false;
                }
                var listUpdateLevel3 = listSetMsts?.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > 0).ToList() ?? new();
                var maxUpdateLevel3 = listUpdateLevel3.OrderByDescending(mst => mst.Level3).FirstOrDefault();
                var listUpdateLevel3ExceptMaxLevel = listUpdateLevel3.Where(mst => mst != maxUpdateLevel3).ToList();
                //LevelDown(3, userId, listUpdateLevel3);
                LevelDown(3, userId, listUpdateLevel3ExceptMaxLevel);
                foreach (var item in listUpdateLevel3ExceptMaxLevel)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                }
                var listDragUpdateLevel2 = listSetMsts?.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > dragItem.Level2).ToList() ?? new();
                LevelUp(2, userId, listDragUpdateLevel2);
                foreach (var item in listDragUpdateLevel2)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                }
                dragItem.Level1 = dropItem.Level1;
                dragItem.Level2 = dropItem.Level2;
                dragItem.Level3 = 1;
                dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                dragItem.UpdateId = userId;
                dragItem.IsDeleted = DeleteTypes.Deleted;
                if (maxUpdateLevel3 != null)
                {
                    SaveLevelDown(3, userId, new List<SetMst> { maxUpdateLevel3 });
                }
                else
                {
                    TrackingDataContext.SaveChanges();
                }

                foreach (var item in listUpdateLevel3ExceptMaxLevel)
                {
                    item.IsDeleted = DeleteTypes.None;
                }

                foreach (var item in listDragUpdateLevel2)
                {
                    item.IsDeleted = DeleteTypes.None;
                }
                dragItem.IsDeleted = DeleteTypes.None;
                TrackingDataContext.SaveChanges();
            }
        }
        // if dropItem is level3 => return false
        else if (dropItem.Level3 > 0)
        {
            return false;
        }
        return true;
    }

    [Obsolete]
    private bool DragItemIsLevel3(SetMst dragItem, SetMst dropItem, int userId, List<SetMst> listSetMsts)
    {
        // if dropItem is level1 
        if (dropItem.Level2 == 0)
        {
            var dragLevel1 = dragItem.Level1;
            var dragLevel2 = dragItem.Level2;
            var dragLevel3 = dragItem.Level3;
            var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > dragItem.Level3).ToList();
            LevelUp(3, userId, listUpdateLevel3);
            foreach (var item in listUpdateLevel3)
            {
                item.IsDeleted = DeleteTypes.Deleted;
            }
            var listUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 > 0 && (dragItem.Level1 != dropItem.Level1 || !(mst.Level1 == dragLevel1 && mst.Level2 == dragLevel2 && mst.Level3 == dragLevel3))).ToList();
            var maxLevel2 = listUpdateLevel2.Count == 0 ? 0 : listUpdateLevel2.Max(l => l.Level2);
            var maxDropUpdateLevel2 = listUpdateLevel2.Where(m => m.Level2 == maxLevel2).ToList();
            var rootMaxDropUpdateLevel2 = maxDropUpdateLevel2.FirstOrDefault(m => m.Level3 == 0);
            var listDropUpdateLevel2ExceptMaxLevel = listUpdateLevel2.Where(l => !maxDropUpdateLevel2.Contains(l)).ToList();
            //LevelDown(2, userId, listUpdateLevel2);
            LevelDown(2, userId, listDropUpdateLevel2ExceptMaxLevel);
            foreach (var item in listDropUpdateLevel2ExceptMaxLevel)
            {
                item.IsDeleted = DeleteTypes.Deleted;
            }
            dragItem.Level1 = dropItem.Level1;
            dragItem.Level2 = 1;
            dragItem.Level3 = 0;
            dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
            dragItem.UpdateId = userId;
            dragItem.IsDeleted = DeleteTypes.Deleted;
            if (rootMaxDropUpdateLevel2 != null)
            {
                rootMaxDropUpdateLevel2.IsDeleted = DeleteTypes.Deleted;
                SaveLevelDown(2, userId, new List<SetMst> { rootMaxDropUpdateLevel2 });
                foreach (var item in maxDropUpdateLevel2.Where(m => m != rootMaxDropUpdateLevel2).ToList())
                {
                    item.Level2 = rootMaxDropUpdateLevel2.Level2;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                    item.IsDeleted = DeleteTypes.Deleted;
                }
                TrackingDataContext.SaveChanges();
            }
            else
            {
                TrackingDataContext.SaveChanges();
            }

            foreach (var item in listUpdateLevel3)
            {
                item.IsDeleted = DeleteTypes.None;
            }

            foreach (var item in listDropUpdateLevel2ExceptMaxLevel)
            {
                item.IsDeleted = DeleteTypes.None;
            }

            foreach (var item in maxDropUpdateLevel2)
            {
                item.IsDeleted = DeleteTypes.None;
            }

            dragItem.IsDeleted = DeleteTypes.None;

            TrackingDataContext.SaveChanges();
        }
        else if (dropItem.Level2 > 0 && dropItem.Level3 == 0)
        {
            if (dragItem.Level1 == dropItem.Level1 && dragItem.Level2 == dropItem.Level2)
            {
                var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > 0).OrderByDescending(mst => mst.Level3).ToList();
                //LevelDown(3, userId, listUpdateLevel3);
                LevelDown(3, userId, listUpdateLevel3);
                foreach (var item in listUpdateLevel3)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                }
                dragItem.Level3 = 1;
                dragItem.UpdateId = userId;
                dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                dragItem.IsDeleted = DeleteTypes.Deleted;
                TrackingDataContext.SaveChanges();
                foreach (var item in listUpdateLevel3)
                {
                    item.IsDeleted = DeleteTypes.None;
                }
                dragItem.IsDeleted = DeleteTypes.None;
                TrackingDataContext.SaveChanges();
            }
            else
            {
                var listDragUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > dragItem.Level3).ToList();

                LevelUp(3, userId, listDragUpdateLevel3);
                foreach (var item in listDragUpdateLevel3)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                }

                var listDropUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > 0).OrderByDescending(mst => mst.Level3).ToList();
                var listUpdateLevel3SkipLast = listDropUpdateLevel3.Skip(1).ToList();
                var lastLevel3 = listDropUpdateLevel3.FirstOrDefault();
                LevelDown(3, userId, listUpdateLevel3SkipLast);
                foreach (var item in listUpdateLevel3SkipLast)
                {
                    item.IsDeleted = DeleteTypes.Deleted;
                }
                //LevelDown(3, userId, listDropUpdateLevel3);

                dragItem.Level1 = dropItem.Level1;
                dragItem.Level2 = dropItem.Level2;
                dragItem.Level3 = 1;
                dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                dragItem.UpdateId = userId;
                dragItem.IsDeleted = DeleteTypes.Deleted;

                if (lastLevel3 != null)
                {
                    SaveLevelDown(3, userId, new List<SetMst> { lastLevel3 });
                }
                else
                {
                    TrackingDataContext.SaveChanges();
                }

                foreach (var item in listDragUpdateLevel3)
                {
                    item.IsDeleted = DeleteTypes.None;
                }

                foreach (var item in listUpdateLevel3SkipLast)
                {
                    item.IsDeleted = DeleteTypes.None;
                }

                dragItem.IsDeleted = DeleteTypes.None;

                TrackingDataContext.SaveChanges();
            }
        }
        else if (dropItem.Level3 > 0)
        {
            if (dragItem.Level1 == dropItem.Level1 && dragItem.Level2 == dropItem.Level2)
            {
                if (dragItem.Level3 > dropItem.Level3)
                {
                    var listDropUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > dropItem.Level3 && mst.Level3 < dragItem.Level3).ToList();
                    //LevelDown(3, userId, listDropUpdateLevel3);
                    LevelDown(3, userId, listDropUpdateLevel3);
                    foreach (var item in listDropUpdateLevel3)
                    {
                        item.IsDeleted = DeleteTypes.Deleted;
                    }
                    dragItem.Level3 = dropItem.Level3 + 1;
                    dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    dragItem.UpdateId = userId;
                    dragItem.IsDeleted = DeleteTypes.Deleted;
                    TrackingDataContext.SaveChanges();
                    foreach (var item in listDropUpdateLevel3)
                    {
                        item.IsDeleted = DeleteTypes.None;
                    }
                    dragItem.IsDeleted = DeleteTypes.None;
                    TrackingDataContext.SaveChanges();
                }
                else if (dragItem.Level3 < dropItem.Level3)
                {
                    var listDropUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > dragItem.Level3 && mst.Level3 <= dropItem.Level3).ToList();
                    var level3Drop = dropItem.Level3;
                    LevelUp(3, userId, listDropUpdateLevel3);
                    foreach (var item in listDropUpdateLevel3)
                    {
                        item.IsDeleted = DeleteTypes.Deleted;
                    }
                    dragItem.Level3 = level3Drop;
                    dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    dragItem.UpdateId = userId;
                    dragItem.IsDeleted = DeleteTypes.Deleted;
                    TrackingDataContext.SaveChanges();
                    foreach (var item in listDropUpdateLevel3)
                    {
                        item.IsDeleted = DeleteTypes.None;
                    }
                    dragItem.IsDeleted = DeleteTypes.None;
                    TrackingDataContext.SaveChanges();
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    [Obsolete]
    private bool DragItemWithDropItemIsLevel0(SetMst dragItem, int userId, List<SetMst> listSetMsts)
    {
        if (dragItem.Level2 == 0)
        {
            var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > 0 && mst.Level1 < dragItem.Level1).ToList();
            var listDragUpdate = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1).ToList();
            //LevelDown(1, userId, listUpdateLevel1);
            LevelDown(1, userId, listUpdateLevel1);
            foreach (var item in listUpdateLevel1)
            {
                item.IsDeleted = DeleteTypes.Deleted;
            }
            foreach (var item in listDragUpdate)
            {
                item.Level1 = 1;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
                item.IsDeleted = DeleteTypes.Deleted;
            }
            TrackingDataContext.SaveChanges();
            foreach (var item in listUpdateLevel1)
            {
                item.IsDeleted = DeleteTypes.None; ;
            }
            foreach (var item in listDragUpdate)
            {
                item.IsDeleted = DeleteTypes.None; ;
            }
            TrackingDataContext.SaveChanges();
        }
        else if (dragItem.Level2 > 0 && dragItem.Level3 == 0)
        {
            var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > 0).ToList();
            var maxLevel1 = listUpdateLevel1.Count == 0 ? 0 : listUpdateLevel1.Max(l => l.Level1);
            var maxDropUpdateLevel1 = listUpdateLevel1.Where(m => m.Level1 == maxLevel1).ToList();
            var rootMaxDropUpdateLevel1 = maxDropUpdateLevel1.FirstOrDefault(m => m.Level2 == 0 && m.Level3 == 0);
            var listDropUpdateLevel1ExceptMaxLevel = listUpdateLevel1.Where(l => !maxDropUpdateLevel1.Contains(l)).ToList();
            //LevelDown(1, userId, listUpdateLevel1);
            LevelDown(1, userId, listDropUpdateLevel1ExceptMaxLevel);
            foreach (var item in listDropUpdateLevel1ExceptMaxLevel)
            {
                item.IsDeleted = DeleteTypes.Deleted;
            }

            var listUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > dragItem.Level2).ToList();
            var listDragUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > 0).ToList();

            LevelUp(2, userId, listUpdateLevel2);
            foreach (var item in listUpdateLevel2)
            {
                item.IsDeleted = DeleteTypes.Deleted;
            }

            // level3 => level2
            foreach (var levelNew in listDragUpdateLevel3)
            {
                levelNew.Level1 = 1;
                levelNew.Level2 = levelNew.Level3;
                levelNew.Level3 = 0;
                levelNew.UpdateDate = CIUtil.GetJapanDateTimeNow();
                levelNew.UpdateId = userId;
                levelNew.IsDeleted = DeleteTypes.Deleted;
            }

            dragItem.IsDeleted = DeleteTypes.Deleted;

            if (rootMaxDropUpdateLevel1 != null)
            {
                SaveLevelDown(1, userId, new List<SetMst> { rootMaxDropUpdateLevel1 });
                foreach (var item in maxDropUpdateLevel1.Where(m => m != rootMaxDropUpdateLevel1).ToList())
                {
                    item.Level1 = rootMaxDropUpdateLevel1.Level1;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                }
            }
            else
            {
                TrackingDataContext.SaveChanges();
            }

            foreach (var item in listDropUpdateLevel1ExceptMaxLevel)
            {
                item.IsDeleted = DeleteTypes.None;
            }
            foreach (var item in listUpdateLevel2)
            {
                item.IsDeleted = DeleteTypes.None;
            }
            foreach (var levelNew in listDragUpdateLevel3)
            {
                levelNew.IsDeleted = DeleteTypes.None;
            }

            // level2 => level1
            dragItem.Level1 = 1;
            dragItem.Level2 = 0;
            dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
            dragItem.UpdateId = userId;
            dragItem.IsDeleted = DeleteTypes.None;
            TrackingDataContext.SaveChanges();
        }
        else if (dragItem.Level2 > 0 && dragItem.Level3 > 0)
        {
            var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > 0).ToList();
            var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > dragItem.Level3).ToList();
            var maxLevel1 = listUpdateLevel1.Count == 0 ? 0 : listUpdateLevel1.Max(l => l.Level1);
            var maxDropUpdateLevel1 = listUpdateLevel1.Where(m => m.Level1 == maxLevel1).ToList();
            var rootMaxDropUpdateLevel1 = maxDropUpdateLevel1.FirstOrDefault(m => m.Level2 == 0 && m.Level3 == 0);
            var listDropUpdateLevel1ExceptMaxLevel = listUpdateLevel1.Where(l => !maxDropUpdateLevel1.Contains(l)).ToList();
            //LevelDown(1, userId, listUpdateLevel1);
            LevelDown(1, userId, listDropUpdateLevel1ExceptMaxLevel);
            foreach (var item in listDropUpdateLevel1ExceptMaxLevel)
            {
                item.IsDeleted = DeleteTypes.Deleted;
            }
            LevelUp(3, userId, listUpdateLevel3);
            foreach (var item in listUpdateLevel3)
            {
                item.IsDeleted = DeleteTypes.Deleted;
            }
            dragItem.IsDeleted = DeleteTypes.Deleted;
            if (rootMaxDropUpdateLevel1 != null)
            {
                SaveLevelDown(1, userId, new List<SetMst> { rootMaxDropUpdateLevel1 });
                foreach (var item in maxDropUpdateLevel1.Where(m => m != rootMaxDropUpdateLevel1).ToList())
                {
                    item.Level1 = rootMaxDropUpdateLevel1.Level1;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                }
            }
            else
            {
                TrackingDataContext.SaveChanges();
            }
            foreach (var item in listDropUpdateLevel1ExceptMaxLevel)
            {
                item.IsDeleted = DeleteTypes.None;
            }
            foreach (var item in listUpdateLevel3)
            {
                item.IsDeleted = DeleteTypes.None;
            }
            dragItem.Level1 = 1;
            dragItem.Level2 = 0;
            dragItem.Level3 = 0;
            dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
            dragItem.UpdateId = userId;
            dragItem.IsDeleted = DeleteTypes.None;
            TrackingDataContext.SaveChanges();
        }
        return true;
    }

    private void LevelDown(int level, int userId, List<SetMst> listUpdate)
    {
        foreach (var item in listUpdate)
        {
            switch (level)
            {
                case 1:
                    item.Level1 = item.Level1 + 1;
                    break;
                case 2:
                    item.Level2 = item.Level2 + 1;
                    break;
                case 3:
                    item.Level3 = item.Level3 + 1;
                    break;
            }
            item.UpdateDate = CIUtil.GetJapanDateTimeNow();
            item.UpdateId = userId;
        }
    }

    [Obsolete]
    private void SaveLevelDown(int level, int userId, List<SetMst> listUpdate)
    {
        try
        {
            LevelDown(level, userId, listUpdate);
            TrackingDataContext.SaveChanges();
        }
        catch (Exception ex)
        {
            bool flag = false;
            var innerException = ex.InnerException?.ToString() ?? string.Empty;
            if (HandleException(ex) == "23505" && innerException.Contains("23505") && innerException.Contains("unique constraint"))
            {
                var count = 0;
                while (count <= tryCountSave)
                {
                    try
                    {
                        flag = true;
                        LevelDown(level, userId, listUpdate);
                        TrackingDataContext.SaveChanges();
                        break;
                    }
                    catch (Exception tryEx)
                    {
                        flag = false;
                        innerException = tryEx.InnerException?.ToString() ?? string.Empty;
                        if (HandleException(tryEx) == "23505" && innerException.Contains("23505") && innerException.Contains("unique constraint"))
                        {
                            count++;
                            continue;
                            //LevelDown(3, userId, listUpdate);
                            //TrackingDataContext.SaveChanges();
                        }
                        break;
                    }
                }
            }
            if (!flag)
            {
                LevelDown(level, userId, listUpdate);
                TrackingDataContext.SaveChanges();
            }
            //Console.WriteLine(ex.Message);
        }
    }

    private void LevelUp(int level, int userId, List<SetMst> listUpdate)
    {
        foreach (var item in listUpdate)
        {
            switch (level)
            {
                case 1:
                    item.Level1 = item.Level1 - 1;
                    break;
                case 2:
                    item.Level2 = item.Level2 - 1;
                    break;
                case 3:
                    item.Level3 = item.Level3 - 1;
                    break;
            }
            item.UpdateDate = CIUtil.GetJapanDateTimeNow();
            item.UpdateId = userId;
        }
    }

    private List<PrefixSuffixModel> SyusyokuCdToList(SetByomei ptByomei)
    {
        List<string> codeList = new()
            {
                ptByomei.SyusyokuCd1 ?? string.Empty,
                ptByomei.SyusyokuCd2 ?? string.Empty,
                ptByomei.SyusyokuCd3 ?? string.Empty,
                ptByomei.SyusyokuCd4 ?? string.Empty,
                ptByomei.SyusyokuCd5 ?? string.Empty,
                ptByomei.SyusyokuCd6 ?? string.Empty,
                ptByomei.SyusyokuCd7 ?? string.Empty,
                ptByomei.SyusyokuCd8 ?? string.Empty,
                ptByomei.SyusyokuCd9 ?? string.Empty,
                ptByomei.SyusyokuCd10 ?? string.Empty,
                ptByomei.SyusyokuCd11 ?? string.Empty,
                ptByomei.SyusyokuCd12 ?? string.Empty,
                ptByomei.SyusyokuCd13 ?? string.Empty,
                ptByomei.SyusyokuCd14 ?? string.Empty,
                ptByomei.SyusyokuCd15 ?? string.Empty,
                ptByomei.SyusyokuCd16 ?? string.Empty,
                ptByomei.SyusyokuCd17 ?? string.Empty,
                ptByomei.SyusyokuCd18 ?? string.Empty,
                ptByomei.SyusyokuCd19 ?? string.Empty,
                ptByomei.SyusyokuCd20 ?? string.Empty,
                ptByomei.SyusyokuCd21 ?? string.Empty
            };
        codeList.Add(ptByomei.ByomeiCd ?? string.Empty);
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

    private void ChangeRpName(int userId, int setCd, string setName)
    {
        if (setCd != 0 && setName != string.Empty)
        {
            var setOrderInfListBySetCd = TrackingDataContext.SetOdrInf.Where(item => item.SetCd == setCd).ToList();
            foreach (var item in setOrderInfListBySetCd)
            {
                item.RpName = setName;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }
        }
    }

    private int GetMaxLevel(int hpId, int setKbn, int setKbnEdaNo, int generationId, int level1, int level2, int level3, bool isCheckLevel0 = false)
    {
        var setMsts = NoTrackingDataContext.SetMsts.Where(s => s.HpId == hpId && s.SetKbn == setKbn && s.SetKbnEdaNo == setKbnEdaNo && s.GenerationId == generationId).ToList();
        int max = 0;

        if ((isCheckLevel0 || level1 > 0) && level2 == 0 && level3 == 0)
        {
            max = setMsts.Count == 0 ? 0 : setMsts.Max(s => s.Level1);
        }
        else if (level1 > 0 && level2 > 0 && level3 == 0)
        {
            max = setMsts.Count == 0 ? 0 : setMsts.Where(s => s.Level1 == level1).Max(s => s.Level2);
        }
        else if (level1 > 0 && level2 > 0 && level3 > 0)
        {
            max = setMsts.Count == 0 ? 0 : setMsts.Where(s => s.Level1 == level1 && s.Level2 == level2).Max(s => s.Level3);
        }

        return max;
    }

    private void RetrySaveSetMst(SetMst setMst)
    {
        var levelMax = GetMaxLevel(setMst.HpId, setMst.SetKbn, setMst.SetKbnEdaNo, setMst.GenerationId, setMst.Level1, setMst.Level2, setMst.Level3);
        if (setMst.Level2 == 0 && setMst.Level3 == 0)
        {
            setMst.Level1 = ++levelMax;
        }
        else if (setMst.Level2 > 0 && setMst.Level3 == 0)
        {
            setMst.Level2 = ++levelMax;
        }
        else
        {
            setMst.Level3 = ++levelMax;
        }
        TrackingDataContext.Add(setMst);
        TrackingDataContext.SaveChanges();
    }

    private void RetryCopyPasteSetMst(SetMst copyItem, SetMst? pasteItem, List<SetMst> listPasteItems)
    {
        var levelPaste = 0;
        if (pasteItem == null)
        {
            levelPaste = 1;
        }
        else if (pasteItem?.Level1 > 0 && pasteItem?.Level2 == 0 && pasteItem?.Level3 == 0)
        {
            levelPaste = 2;
        }
        else if (pasteItem?.Level1 > 0 && pasteItem?.Level2 > 0 && pasteItem?.Level3 == 0)
        {
            levelPaste = 3;
        }
        else if (pasteItem?.Level1 > 0 && pasteItem?.Level2 > 0 && pasteItem?.Level3 > 0)
        {
            levelPaste = 4;
        }

        var levelMax = GetMaxLevel(copyItem.HpId, copyItem.SetKbn, copyItem.SetKbnEdaNo, copyItem.GenerationId, levelPaste >= 1 ? pasteItem?.Level1 ?? 0 : 0, levelPaste >= 2 ? pasteItem?.Level2 ?? 0 : 0, levelPaste >= 3 ? pasteItem?.Level3 ?? 0 : 0, pasteItem == null);

        ReSetLevelForItem(levelMax, copyItem, pasteItem, listPasteItems);
    }

    private (int level1, int level2, int level3) GetLevelSetMst(int setCd, int setKbn, int setKbnEdaNo, int generationId)
    {
        IQueryable<SetMst> setMstQuery = NoTrackingDataContext.SetMsts.Where(item => item.IsDeleted == 0
                                                                                     && item.SetKbn == setKbn
                                                                                     && item.SetKbnEdaNo == setKbnEdaNo
                                                                                     && item.GenerationId == generationId);
        int level1 = 0;
        int level2 = 0;
        int level3 = 0;

        // if item is level1
        if (setCd == 0)
        {
            var level1List = setMstQuery.Where(item => item.Level1 > 0
                                                       && item.Level2 == 0
                                                       && item.Level3 == 0)
                                        .Select(item => item.Level1)
                                        .Distinct()
                                        .ToList();

            var maxLevel1 = 0;
            if (level1List.Any())
            {
                maxLevel1 = level1List.Max();
            }
            level1 = maxLevel1 + 1;
        }

        if (setCd > 0)
        {
            var setItem = setMstQuery.First(item => item.SetCd == setCd);
            level1 = setItem.Level1;

            // if item is level1 => children is level 2
            if (setItem.Level1 > 0 && setItem.Level2 == 0 && setItem.Level3 == 0)
            {
                var level2List = setMstQuery.Where(item => item.Level1 == setItem.Level1
                                                       && item.Level2 > 0
                                                       && item.Level3 == 0)
                                        .Select(item => item.Level2)
                                        .Distinct()
                                        .ToList();

                int maxLevel2 = 0;
                if (level2List.Any())
                {
                    maxLevel2 = level2List.Max();
                }
                level2 = maxLevel2 + 1;
            }

            // if item is level2 => children is level 3
            else if (setItem.Level1 > 0 && setItem.Level2 > 0 && setItem.Level3 == 0)
            {
                level2 = setItem.Level2;
                var level3List = setMstQuery.Where(item => item.Level1 == setItem.Level1
                                                           && item.Level2 == setItem.Level2
                                                           && item.Level3 > 0)
                                            .Select(item => item.Level3)
                                            .Distinct()
                                            .ToList();

                int maxLevel3 = 0;
                if (level3List.Any())
                {
                    maxLevel3 = level3List.Max();
                }
                level3 = maxLevel3 + 1;
            }
        }
        return (level1, level2, level3);
    }

    #endregion

    #region Catch Exception
    [Obsolete]
    private static string HandleException(Exception exception)
    {
        if (exception is DbUpdateConcurrencyException concurrencyEx)
        {
            return "0";
        }
        else if (exception is DbUpdateException dbUpdateEx)
        {
            if (dbUpdateEx.InnerException != null)
            {
                if (dbUpdateEx.InnerException is PostgresException postgreException)
                {
                    return postgreException.Code ?? string.Empty;
                }
            }
        }

        return "0";
    }
    #endregion
}
