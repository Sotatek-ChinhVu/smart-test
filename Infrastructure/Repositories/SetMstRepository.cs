﻿using Domain.Models.Diseases;
using Domain.Models.SetMst;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Npgsql;
using System.Data;
using System.Text;

namespace Infrastructure.Repositories;

public class SetMstRepository : RepositoryBase, ISetMstRepository
{
    private readonly string defaultSetName = "新規セット";
    private readonly string defaultGroupName = "新規グループ";
    private readonly IMemoryCache _memoryCache;
    private readonly int tryCountSave = 10;
    public SetMstRepository(ITenantProvider tenantProvider, IMemoryCache memoryCache) : base(tenantProvider)
    {
        _memoryCache = memoryCache;
    }

    private IEnumerable<SetMstModel> ReloadCache(int hpId)
    {
        var setMstModelList =
                NoTrackingDataContext.SetMsts
                .Where(s => s.HpId == hpId)
                .Select(s => new SetMstModel(
                    s.HpId,
                    s.SetCd,
                    s.SetKbn,
                    s.SetKbnEdaNo,
                    s.GenerationId,
                    s.Level1,
                    s.Level2,
                    s.Level3,
                    s.SetName == null ? String.Empty : s.SetName,
                    s.WeightKbn,
                    s.Color,
                    s.IsDeleted,
                    s.IsGroup
                    ))
                .ToList();
        var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetPriority(CacheItemPriority.Normal);
        _memoryCache.Set(GetCacheKey(), setMstModelList, cacheEntryOptions);

        return setMstModelList;
    }

    public IEnumerable<SetMstModel> GetList(int hpId, int setKbn, int setKbnEdaNo, int generationId, string textSearch)
    {
        if (!_memoryCache.TryGetValue(GetCacheKey(), out IEnumerable<SetMstModel>? setMstModelList))
        {
            setMstModelList = ReloadCache(hpId);
        }

        var result = new List<SetMstModel>();
        if (string.IsNullOrEmpty(textSearch))
        {
            result = setMstModelList!
         .Where(s => s.HpId == hpId && s.GenerationId == generationId && s.SetKbn == setKbn && s.SetKbnEdaNo == setKbnEdaNo - 1 && s.IsDeleted == 0).ToList();
        }
        else
        {
            var searchItems = setMstModelList!
          .Where(s => s.HpId == hpId && s.GenerationId == generationId && s.SetKbn == setKbn && s.SetKbnEdaNo == setKbnEdaNo - 1 && s.IsDeleted == 0 && (string.IsNullOrEmpty(textSearch) || (s.SetName != null && s.SetName.Contains(textSearch)))).ToList();
            var filters = searchItems.Where(s => s.Level3 == 0).ToList();
            foreach (var filter in filters)
            {
                if (filter.Level2 > 0)
                    searchItems.RemoveAll(s => s.Level1 == filter.Level1 && s.Level2 == filter.Level2 && s.Level3 > 0);
                else
                    searchItems.RemoveAll(s => s.Level1 == filter.Level1 && s.Level2 > 0);
            }

            foreach (var searchItem in searchItems)
            {
                if (searchItem.Level2 == 0 && searchItem.Level3 == 0)
                {
                    var resultItem = setMstModelList!.Where(s => s.HpId == hpId && s.GenerationId == generationId && s.SetKbn == searchItem.SetKbn && s.SetKbnEdaNo == searchItem.SetKbnEdaNo && s.IsDeleted == 0 && s.Level1 == searchItem.Level1);
                    result.AddRange(resultItem);
                }
                else if (searchItem.Level3 == 0)
                {
                    var resultItem = setMstModelList!.Where(s => s.HpId == hpId && s.GenerationId == generationId && s.SetKbn == searchItem.SetKbn && s.SetKbnEdaNo == searchItem.SetKbnEdaNo && s.IsDeleted == 0 && s.Level1 == searchItem.Level1 && s.Level2 == searchItem.Level2);
                    var rootItem = setMstModelList!.FirstOrDefault(s => s.HpId == hpId && s.GenerationId == generationId && s.SetKbn == searchItem.SetKbn && s.SetKbnEdaNo == searchItem.SetKbnEdaNo && s.IsDeleted == 0 && s.Level1 == searchItem.Level1 && s.Level2 == 0 && s.Level3 == 0);
                    if (rootItem != null)
                    {
                        result.Add(rootItem);
                    }
                    result.AddRange(resultItem);
                }
                else
                {
                    var level2RootItem = setMstModelList!.FirstOrDefault(s => s.HpId == hpId && s.GenerationId == generationId && s.SetKbn == searchItem.SetKbn && s.SetKbnEdaNo == searchItem.SetKbnEdaNo && s.IsDeleted == 0 && s.Level1 == searchItem.Level1 && s.Level2 == searchItem.Level2 && s.Level3 == 0);
                    var rootItem = setMstModelList!.FirstOrDefault(s => s.HpId == hpId && s.GenerationId == generationId && s.SetKbn == searchItem.SetKbn && s.SetKbnEdaNo == searchItem.SetKbnEdaNo && s.IsDeleted == 0 && s.Level1 == searchItem.Level1 && s.Level2 == 0 && s.Level3 == 0);
                    if (rootItem != null)
                    {
                        result.Add(rootItem);
                    }
                    if (level2RootItem != null)
                    {
                        result.Add(level2RootItem);
                    }
                    result.Add(searchItem);
                }
            }
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
        var listOrders = new List<OrderTooltipModel>();
        foreach (var key in keys)
        {
            listOrders.AddRange(allOrderDetails.Where(item => item.SetCd == setCd && item.HpId == hpId && key.RpNo == item.RpNo && key.RpEdaNo == item.RpEdaNo).Select(item => new OrderTooltipModel(item.ItemName ?? String.Empty, item.Suryo, item.UnitName ?? String.Empty)));
        }

        return new SetMstTooltipModel(listKarteNames, listOrders, byomeiNameList);
    }

    public SetMstModel SaveSetMstModel(int userId, int sinDate, SetMstModel setMstModel)
    {
        SetMst setMst = new();
        try
        {
            // Check SetMstModel is delete?
            bool isDelete = setMstModel.IsDeleted == 1;
            var setKbnEdaNo = (setMstModel.SetKbnEdaNo - 1) > 0 ? setMstModel.SetKbnEdaNo - 1 : 0;

            // Create SetMst to save
            var oldSetMst = TrackingDataContext.SetMsts.FirstOrDefault(item => item.SetCd == setMstModel.SetCd
                                                                            && item.SetKbn == setMstModel.SetKbn
                                                                            && item.SetKbnEdaNo == setKbnEdaNo
                                                                            && item.GenerationId == setMstModel.GenerationId
                                                                            && item.IsDeleted != 1);

            if (oldSetMst == null && setMstModel.SetCd != 0)
            {
                return new SetMstModel();
            }
            else
            {
                oldSetMst = oldSetMst != null ? oldSetMst : new SetMst();
            }
            setMst = ConvertSetMstModelToSetMst(oldSetMst, setMstModel, userId);

            if (!isDelete)
            {
                // set status for IsDelete
                setMst.IsDeleted = 0;
                ChangeRpName(userId, setMst.SetCd, setMst.SetName ?? string.Empty);

                // If SetMst is add new
                if (setMstModel.SetCd == 0 || TrackingDataContext.SetMsts.FirstOrDefault(item => item.SetCd == setMstModel.SetCd) == null)
                {
                    setMst.IsGroup = setMstModel.IsGroup;
                    if (setMst.SetName == null || setMst.SetName.Length == 0)
                    {
                        setMst.SetName = setMst.IsGroup == 1 ? defaultGroupName : defaultSetName;
                    }
                    setMst.GenerationId = GetGenerationId(setMst.HpId, sinDate);
                    setMst.CreateDate = CIUtil.GetJapanDateTimeNow();
                    setMst.CreateId = userId;
                    setMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    setMst.UpdateId = userId;

                    // Save SetMst 
                    TrackingDataContext.SetMsts.Add(setMst);
                }
            }
            // Delete SetMst
            else
            {
                // set status for IsDelete
                setMst.IsDeleted = 1;

                // if SetMst have children element
                // if SetMst is level 2 and have children element
                if (setMst.Level2 > 0 && setMst.Level3 == 0)
                {
                    var listSetMstLevel3 = TrackingDataContext.SetMsts
                                            .Where(item => item.SetKbn == setMst.SetKbn
                                                          && item.SetKbnEdaNo == setKbnEdaNo
                                                          && item.GenerationId == setMst.GenerationId
                                                          && item.Level1 == setMst.Level1
                                                          && item.Level2 == setMst.Level2
                                                          && item.Level3 > 0
                                                          && item.IsDeleted != 1
                                            ).ToList();
                    foreach (var item in listSetMstLevel3)
                    {
                        item.IsDeleted = 1;
                        item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        item.UpdateId = userId;
                    }
                }

                // if SetMst is level 1 and have children element
                if (setMst.Level2 == 0 && setMst.Level3 == 0)
                {
                    // get list SetMst level 2
                    var listSetMstLevel2 = TrackingDataContext.SetMsts
                                            .Where(item => item.SetKbn == setMst.SetKbn
                                                          && item.SetKbnEdaNo == setKbnEdaNo
                                                          && item.GenerationId == setMst.GenerationId
                                                          && item.Level1 == setMst.Level1
                                                          && item.Level2 > 0
                                                          && item.Level3 == 0
                                                          && item.IsDeleted != 1
                                            ).ToList();

                    // get list SetMst level 3
                    var listSetMstLevel3 = TrackingDataContext.SetMsts
                                            .Where(item => item.SetKbn == setMst.SetKbn
                                                          && item.SetKbnEdaNo == setKbnEdaNo
                                                          && item.GenerationId == setMst.GenerationId
                                                          && item.Level1 == setMst.Level1
                                                          && item.Level3 > 0
                                                          && item.IsDeleted != 1
                                            ).ToList();

                    // Update isDelete for SetMst level 2 and level 3
                    foreach (var level2 in listSetMstLevel2)
                    {
                        level2.IsDeleted = 1;
                        level2.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        level2.UpdateId = userId;
                    }

                    foreach (var level3 in listSetMstLevel3)
                    {
                        level3.IsDeleted = 1;
                        level3.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        level3.UpdateId = userId;
                    }
                }
            }
            TrackingDataContext.SaveChanges();
            return new SetMstModel(
                    setMst.HpId,
                    setMst.SetCd,
                    setMst.SetKbn,
                    setMst.SetKbnEdaNo,
                    setMst.GenerationId,
                    setMst.Level1,
                    setMst.Level2,
                    setMst.Level3,
                    setMst.SetName ?? String.Empty,
                    setMst.WeightKbn,
                    setMst.Color,
                    setMst.IsDeleted,
                    setMst.IsGroup
                );
        }
        catch (Exception ex)
        {
            if (HandleException(ex) == 23000)
            {
                int count = 0;
                while (count <= tryCountSave)
                {
                    try
                    {
                        RetrySaveSetMst(setMst);
                    }
                    catch (Exception tryEx)
                    {

                        if (HandleException(tryEx) == 23000)
                        {
                            RetrySaveSetMst(setMst);
                        }
                        Console.WriteLine(tryEx.Message);
                    }
                    count++;
                }
            }
            Console.WriteLine(ex.Message);
            return new SetMstModel();
        }
        finally
        {
            ReloadCache(1);
        }
    }

    public bool ReorderSetMst(int userId, int hpId, int setCdDragItem, int setCdDropItem)
    {
        bool status = false;
        try
        {
            var dragItem = TrackingDataContext.SetMsts.FirstOrDefault(mst => mst.SetCd == setCdDragItem && mst.HpId == hpId);
            var dropItem = TrackingDataContext.SetMsts.FirstOrDefault(mst => mst.SetCd == setCdDropItem && mst.HpId == hpId);

            // if dragItem is not exist
            if (dragItem == null)
            {
                return status;
            }
            // if dropItem input is not exist
            else if (dropItem == null && setCdDropItem != 0)
            {
                return status;
            }

            // Get all SetMst with dragItem SetKbn and dragItem SetKbnEdaNo
            var listSetMsts = TrackingDataContext.SetMsts.Where(mst => mst.SetKbn == dragItem.SetKbn && mst.SetKbnEdaNo == dragItem.SetKbnEdaNo && mst.HpId == dragItem.HpId && mst.Level1 > 0 && mst.IsDeleted != 1 && mst.GenerationId == dragItem.GenerationId).ToList();

            if (dropItem != null)
            {
                // if dragItem SetKbnEdaNo diffirent dropItem SetKbnEdaNo or dragItem SetKbn different dropItem SetKbn
                if (dragItem.SetKbnEdaNo != dropItem.SetKbnEdaNo || dragItem.SetKbn != dropItem.SetKbn)
                {
                    return status;
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
        }
        catch
        {
            return status;
        }
        finally
        {
            ReloadCache(1);
        }
        return status;
    }

    public int PasteSetMst(int hpId, int userId, int generationId, int setCdCopyItem, int setCdPasteItem, bool pasteToOtherGroup, int copySetKbnEdaNo, int copySetKbn, int pasteSetKbnEdaNo, int pasteSetKbn)
    {
        if (pasteSetKbnEdaNo <= 0 && pasteSetKbn <= 0)
        {
            return -1;
        }

        try
        {
            if (pasteToOtherGroup && setCdCopyItem == 0 && setCdPasteItem == 0)
            {
                return CopyPasteGroupSetMst(hpId, userId, generationId, copySetKbnEdaNo, copySetKbn, pasteSetKbnEdaNo, pasteSetKbn);
            }
            else if (setCdCopyItem > 0)
            {
                return CopyPasteItemSetMst(hpId, userId, setCdCopyItem, setCdPasteItem, pasteToOtherGroup, generationId, pasteSetKbnEdaNo, pasteSetKbn);
            }
        }
        finally
        {
            ReloadCache(1);
        }
        return -1;
    }

    public bool CheckExistSetMstBySetCd(int setCd)
    {
        return NoTrackingDataContext.SetMsts.Any(item => item.SetCd == setCd);
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
        setMst.GenerationId = setMstModel.GenerationId;
        setMst.Level1 = setMstModel.Level1;
        setMst.Level2 = setMstModel.Level2;
        setMst.Level3 = setMstModel.Level3;
        setMst.SetName = setMstModel.SetName;
        setMst.Color = setMstModel.Color;
        setMst.WeightKbn = setMstModel.WeightKbn;
        setMst.UpdateDate = CIUtil.GetJapanDateTimeNow();
        setMst.UpdateId = userId;
        return setMst;
    }

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
                            TrackingDataContext.SetMsts.Add(rootSet);
                            TrackingDataContext.SaveChanges();
                            setCd = rootSet.SetCd;
                            //Get max level1
                            var levelMax = GetMaxLevel(rootSet.HpId, rootSet.SetKbn, rootSet.SetKbnEdaNo, rootSet.GenerationId, rootSet.Level1, 0, 0);
                            var setLevel = Int32.MaxValue - listCopyItems.Count;
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
                                setMst.Level1 = setLevel++;
                                setMst.Level2 = setLevel++;
                                setMst.Level3 = setLevel++;
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

                        // Set level for item
                        try
                        {
                            ReSetLevelForItem(indexPaste, copyItem, pasteItem, listPasteItems);

                            TrackingDataContext.SaveChanges();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            if (HandleException(ex) == 23000)
                            {
                                var count = 0;
                                while (count <= tryCountSave)
                                {
                                    try
                                    {
                                        RetryCopyPasteSetMst(copyItem, pasteItem, listPasteItems);

                                        TrackingDataContext.SaveChanges();
                                        transaction.Commit();
                                    }
                                    catch (Exception tryEx)
                                    {
                                        if (HandleException(tryEx) == 23000)
                                        {
                                            RetryCopyPasteSetMst(copyItem, pasteItem, listPasteItems);

                                            TrackingDataContext.SaveChanges();
                                            transaction.Commit();
                                        }
                                        Console.WriteLine(tryEx.Message);
                                    }
                                }
                            }
                            Console.WriteLine(ex.Message);
                        }

                    }
                    catch
                    {
                        transaction.Rollback();
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
                    catch
                    {
                        transaction.Rollback();
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

    private bool DragItemIsLevel1(SetMst dragItem, SetMst dropItem, int userId, List<SetMst> listSetMsts)
    {
        var listDragItem = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1).ToList();
        // if drop item is level 1
        if (dropItem.Level2 == 0 && dropItem.Level3 == 0)
        {
            if (dragItem.Level1 > dropItem.Level1)
            {
                var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > dropItem.Level1 && mst.Level1 < dragItem.Level1).ToList();
                //LevelDown(1, userId, listUpdateLevel1);
                SaveLevelDown(1, userId, listUpdateLevel1);

                foreach (var item in listDragItem)
                {
                    item.Level1 = dropItem.Level1 + 1;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                }
            }
            else if (dragItem.Level1 < dropItem.Level1)
            {
                var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > dragItem.Level1 && mst.Level1 <= dropItem.Level1).ToList();
                LevelUp(1, userId, listUpdateLevel1);

                foreach (var item in listDragItem)
                {
                    item.Level1 = dropItem.Level1 + 1;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                }
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
            var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > 0).ToList();
            //LevelDown(3, userId, listUpdateLevel3);
            SaveLevelDown(3, userId, listUpdateLevel3);

            var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > dragItem.Level1).ToList();
            LevelUp(1, userId, listUpdateLevel1);

            dragItem.Level1 = dropItem.Level1;
            dragItem.Level2 = dropItem.Level2;
            dragItem.Level3 = 1;
        }
        // if drop item is level 3 return false
        else if (dropItem.Level3 > 0)
        {
            return false;
        }
        return true;
    }

    private bool DragItemIsLevel2(SetMst dragItem, SetMst dropItem, int userId, List<SetMst> listSetMsts)
    {
        // if dropItem is level1
        if (dropItem.Level2 == 0)
        {
            if (dragItem.Level1 == dropItem.Level1)
            {
                var listDropUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > 0 && mst.Level2 < dragItem.Level2).ToList();
                //LevelDown(2, userId, listDropUpdateLevel2);
                SaveLevelDown(2, userId, listDropUpdateLevel2);

                var listDragItem = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2).ToList();
                foreach (var item in listDragItem)
                {
                    item.Level2 = 1;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                }
            }
            else
            {
                var listDrag = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2).ToList();

                var listDropUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 > 0).ToList() ?? new();
                //LevelDown(2, userId, listDropUpdateLevel2);
                SaveLevelDown(2, userId, listDropUpdateLevel2);

                var listDragUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > dragItem.Level2).ToList() ?? new();
                LevelUp(2, userId, listDragUpdateLevel2);

                foreach (var item in listDrag)
                {
                    item.Level1 = dropItem.Level1;
                    item.Level2 = 1;
                    item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    item.UpdateId = userId;
                }
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
                    //LevelDown(2, userId, listUpdateLevel2);
                    SaveLevelDown(3, userId, listUpdateLevel2);

                    foreach (var item in listDragUpdateLevel2)
                    {
                        item.Level2 = dropItem.Level2 + 1;
                        item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        item.UpdateId = userId;
                    }
                }
                else if (dragItem.Level2 < dropItem.Level2)
                {
                    var listUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 > dragItem.Level2 && mst.Level2 <= dropItem.Level2).ToList();
                    LevelUp(2, userId, listUpdateLevel2);

                    foreach (var item in listDragUpdateLevel2)
                    {
                        item.Level2 = dropItem.Level2 + 1;
                        item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                        item.UpdateId = userId;
                    }
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
                //LevelDown(3, userId, listUpdateLevel3);
                SaveLevelDown(3, userId, listUpdateLevel3);

                var listDragUpdateLevel2 = listSetMsts?.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > dragItem.Level2).ToList() ?? new();
                LevelUp(2, userId, listDragUpdateLevel2);

                dragItem.Level1 = dropItem.Level1;
                dragItem.Level2 = dropItem.Level2;
                dragItem.Level3 = 1;
                dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                dragItem.UpdateId = userId;
            }
        }
        // if dropItem is level3 => return false
        else if (dropItem.Level3 > 0)
        {
            return false;
        }
        return true;
    }

    private bool DragItemIsLevel3(SetMst dragItem, SetMst dropItem, int userId, List<SetMst> listSetMsts)
    {
        // if dropItem is level1 
        if (dropItem.Level2 == 0)
        {
            var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > dragItem.Level3).ToList();
            LevelUp(3, userId, listUpdateLevel3);

            var listUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 > 0).ToList();
            //LevelDown(2, userId, listUpdateLevel2);
            SaveLevelDown(2, userId, listUpdateLevel2);

            dragItem.Level1 = dropItem.Level1;
            dragItem.Level2 = 1;
            dragItem.Level3 = 0;
            dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
            dragItem.UpdateId = userId;
        }
        else if (dropItem.Level2 > 0 && dropItem.Level3 == 0)
        {
            if (dragItem.Level1 == dropItem.Level1 && dragItem.Level2 == dropItem.Level2)
            {
                var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > 0).ToList();
                //LevelDown(3, userId, listUpdateLevel3);
                SaveLevelDown(3, userId, listUpdateLevel3);
                dragItem.Level3 = 1;
                dragItem.UpdateId = userId;
                dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
            }
            else
            {
                var listDragUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > dragItem.Level3).ToList();
                LevelUp(3, userId, listDragUpdateLevel3);

                var listDropUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > 0).ToList();
                SaveLevelDown(3, userId, listDropUpdateLevel3);
                //LevelDown(3, userId, listDropUpdateLevel3);

                dragItem.Level1 = dropItem.Level1;
                dragItem.Level2 = dropItem.Level2;
                dragItem.Level3 = 1;
                dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                dragItem.UpdateId = userId;
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
                    SaveLevelDown(3, userId, listDropUpdateLevel3);

                    dragItem.Level3 = dropItem.Level3 + 1;
                    dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    dragItem.UpdateId = userId;
                }
                else if (dragItem.Level3 < dropItem.Level3)
                {
                    var listDropUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > dragItem.Level3 && mst.Level3 <= dropItem.Level3).ToList();
                    LevelUp(3, userId, listDropUpdateLevel3);

                    dragItem.Level3 = dropItem.Level3 + 1;
                    dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
                    dragItem.UpdateId = userId;
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

    private bool DragItemWithDropItemIsLevel0(SetMst dragItem, int userId, List<SetMst> listSetMsts)
    {
        if (dragItem.Level2 == 0)
        {
            var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > 0 && mst.Level1 < dragItem.Level1).ToList();
            var listDragUpdate = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1).ToList();
            //LevelDown(1, userId, listUpdateLevel1);
            SaveLevelDown(1, userId, listUpdateLevel1);

            foreach (var item in listDragUpdate)
            {
                item.Level1 = 1;
                item.UpdateDate = CIUtil.GetJapanDateTimeNow();
                item.UpdateId = userId;
            }
        }
        else if (dragItem.Level2 > 0 && dragItem.Level3 == 0)
        {
            var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > 0).ToList();
            //LevelDown(1, userId, listUpdateLevel1);
            SaveLevelDown(1, userId, listUpdateLevel1);

            var listUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > dragItem.Level2).ToList();
            var listDragUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > 0).ToList();

            LevelUp(2, userId, listUpdateLevel2);

            // level3 => level2
            foreach (var levelNew in listDragUpdateLevel3)
            {
                levelNew.Level1 = 1;
                levelNew.Level2 = levelNew.Level3;
                levelNew.Level3 = 0;
                levelNew.UpdateDate = CIUtil.GetJapanDateTimeNow();
                levelNew.UpdateId = userId;
            }

            // level2 => level1
            dragItem.Level1 = 1;
            dragItem.Level2 = 0;
            dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
            dragItem.UpdateId = userId;
        }
        else if (dragItem.Level2 > 0 && dragItem.Level3 > 0)
        {
            var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > 0).ToList();
            var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > dragItem.Level3).ToList();

            //LevelDown(1, userId, listUpdateLevel1);
            SaveLevelDown(1, userId, listUpdateLevel1);

            LevelUp(3, userId, listUpdateLevel3);

            dragItem.Level1 = 1;
            dragItem.Level2 = 0;
            dragItem.Level3 = 0;
            dragItem.UpdateDate = CIUtil.GetJapanDateTimeNow();
            dragItem.UpdateId = userId;
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

    private void SaveLevelDown(int level, int userId, List<SetMst> listUpdate)
    {
        try
        {
            LevelDown(level, userId, listUpdate);
            TrackingDataContext.SaveChanges();
        }
        catch (Exception ex)
        {
            if (HandleException(ex) == 23000)
            {
                var count = 0;
                while (count <= tryCountSave)
                {
                    try
                    {
                        LevelDown(3, userId, listUpdate);
                        TrackingDataContext.SaveChanges();
                    }
                    catch (Exception tryEx)
                    {
                        if (HandleException(tryEx) == 23000)
                        {
                            LevelDown(3, userId, listUpdate);
                            TrackingDataContext.SaveChanges();
                        }
                        Console.WriteLine(tryEx.Message);
                    }
                }
            }
            Console.WriteLine(ex.Message);
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
        codeList = codeList.Where(c => c != string.Empty).ToList();

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
            setMst.Level1 = levelMax++;
        }
        else if (setMst.Level2 > 0 && setMst.Level3 == 0)
        {
            setMst.Level2 = levelMax++;
        }
        else
        {
            setMst.Level3 = levelMax++;
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
    #endregion

    #region Catch Exception
    private int HandleException(Exception exception)
    {
        if (exception is DbUpdateConcurrencyException concurrencyEx)
        {
            return 0;
        }
        else if (exception is DbUpdateException dbUpdateEx)
        {
            if (dbUpdateEx.InnerException != null
                    && dbUpdateEx.InnerException.InnerException != null)
            {
                if (dbUpdateEx.InnerException.InnerException is PostgresException postgreException)
                {
                    return postgreException.ErrorCode;
                }
            }
        }

        return 0;
    }
    #endregion
}
