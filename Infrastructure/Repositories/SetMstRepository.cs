using Domain.Models.SetMst;
using Entity.Tenant;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class SetMstRepository : ISetMstRepository
{
    private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
    private readonly TenantDataContext _tenantDataContext;
    private readonly string DefaultSetName = "新規セット";
    private readonly string DefaultGroupName = "新規グループ";
    public SetMstRepository(ITenantProvider tenantProvider)
    {
        _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public IEnumerable<SetMstModel> GetList(int hpId, int setKbn, int setKbnEdaNo, string textSearch)
    {
        var setEntities = _tenantNoTrackingDataContext.SetMsts.Where(s => s.HpId == hpId && s.SetKbn == setKbn && s.SetKbnEdaNo == setKbnEdaNo - 1 && s.IsDeleted == 0 && (string.IsNullOrEmpty(textSearch) || (s.SetName != null && s.SetName.Contains(textSearch))))
          .OrderBy(s => s.Level1)
          .ThenBy(s => s.Level2)
          .ThenBy(s => s.Level3)
          .ToList();

        if (setEntities == null)
        {
            return new List<SetMstModel>();
        }

        var listSetCd = setEntities.Select(item => item.SetCd).ToList();
        var listByomeis = _tenantNoTrackingDataContext.SetByomei.Where(item => listSetCd.Contains(item.SetCd) && item.IsDeleted != 1 && item.Byomei != String.Empty).ToList();
        var listKarteNames = _tenantNoTrackingDataContext.SetKarteInf.Where(item => listSetCd.Contains(item.SetCd) && item.IsDeleted != 1 && item.Text != String.Empty).ToList();
        var listOrders = _tenantNoTrackingDataContext.SetOdrInfDetail.Where(item => listSetCd.Contains(item.SetCd)).ToList();

        return setEntities.Select(s =>
                new SetMstModel(
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
                    s.IsGroup,
                    new SetMstTooltipModel(
                            listByomeis.Where(item => item.SetCd == s.SetCd).Select(item => item.Byomei ?? String.Empty).ToList(),
                            listOrders.Where(item => item.SetCd == s.SetCd).Select(item => new OrderTooltipModel(item.ItemName ?? String.Empty, item.Suryo, item.UnitName ?? String.Empty)).ToList(),
                            listKarteNames.Where(item => item.SetCd == s.SetCd).Select(item => item.Text ?? String.Empty).ToList()
                        )
                    )
                ).ToList();
    }

    public SetMstModel? SaveSetMstModel(int userId, int sinDate, SetMstModel setMstModel)
    {
        try
        {
            // Check SetMstModel is delete?
            bool isDelete = setMstModel.IsDeleted == 1;
            var setKbnEdaNo = (setMstModel.SetKbnEdaNo - 1) > 0 ? setMstModel.SetKbnEdaNo - 1 : 0;

            // Create SetMst to save
            var oldSetMst = _tenantDataContext.SetMsts.FirstOrDefault(item => item.SetCd == setMstModel.SetCd
                                                                            && item.SetKbn == setMstModel.SetKbn
                                                                            && item.SetKbnEdaNo == setKbnEdaNo
                                                                            && item.GenerationId == setMstModel.GenerationId
                                                                            && item.IsDeleted != 1);

            if (oldSetMst == null && setMstModel.SetCd != 0)
            {
                return null;
            }
            else
            {
                oldSetMst = oldSetMst != null ? oldSetMst : new SetMst();
            }
            var setMst = ConvertSetMstModelToSetMst(oldSetMst, setMstModel, userId);

            if (!isDelete)
            {
                // set status for IsDelete
                setMst.IsDeleted = 0;

                // If SetMst is add new
                if (setMstModel.SetCd == 0 || _tenantNoTrackingDataContext.SetMsts.FirstOrDefault(item => item.SetCd == setMstModel.SetCd) == null)
                {
                    setMst.IsGroup = setMstModel.IsGroup;
                    if (setMst.SetName == null || setMst.SetName.Length == 0)
                    {
                        setMst.SetName = setMst.IsGroup == 1 ? DefaultGroupName : DefaultSetName;
                    }
                    setMst.GenerationId = GetGenerationId(setMst.HpId, sinDate);
                    setMst.CreateDate = DateTime.UtcNow;
                    setMst.CreateId = userId;

                    // Save SetMst 
                    _tenantDataContext.SetMsts.Add(setMst);
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
                    var listSetMstLevel3 = _tenantDataContext.SetMsts
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
                        item.UpdateDate = DateTime.UtcNow;
                        item.UpdateId = userId;
                    }
                }

                // if SetMst is level 1 and have children element
                if (setMst.Level2 == 0 && setMst.Level3 == 0)
                {
                    // get list SetMst level 2
                    var listSetMstLevel2 = _tenantDataContext.SetMsts
                                            .Where(item => item.SetKbn == setMst.SetKbn
                                                          && item.SetKbnEdaNo == setKbnEdaNo
                                                          && item.GenerationId == setMst.GenerationId
                                                          && item.Level1 == setMst.Level1
                                                          && item.Level2 > 0
                                                          && item.Level3 == 0
                                                          && item.IsDeleted != 1
                                            ).ToList();

                    // get list SetMst level 3
                    var listSetMstLevel3 = _tenantDataContext.SetMsts
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
                        level2.UpdateDate = DateTime.UtcNow;
                        level2.UpdateId = userId;
                    }

                    foreach (var level3 in listSetMstLevel3)
                    {
                        level3.IsDeleted = 1;
                        level3.UpdateDate = DateTime.UtcNow;
                        level3.UpdateId = userId;
                    }
                }
            }
            _tenantDataContext.SaveChanges();
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
                    setMst.IsGroup,
                    new SetMstTooltipModel(
                            new(),
                            new List<OrderTooltipModel>(),
                            new()
                        )
                );
        }
        catch
        {
            return null;
        }
    }

    // GetGenerationId by hpId and sindate
    private int GetGenerationId(int hpId, int sinDate)
    {
        int generationId = 0;
        var generation = _tenantNoTrackingDataContext.SetGenerationMsts.Where(x => x.HpId == hpId && x.StartDate <= sinDate && x.IsDeleted == 0)
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
        setMst.UpdateDate = DateTime.UtcNow;
        setMst.UpdateId = userId;
        return setMst;
    }

    public bool ReorderSetMst(int userId, int hpId, int setCdDragItem, int setCdDropItem)
    {
        bool status = false;
        try
        {
            var dragItem = _tenantDataContext.SetMsts.FirstOrDefault(mst => mst.SetCd == setCdDragItem && mst.HpId == hpId);
            var dropItem = _tenantDataContext.SetMsts.FirstOrDefault(mst => mst.SetCd == setCdDropItem && mst.HpId == hpId);

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
            var listSetMsts = _tenantDataContext.SetMsts.Where(mst => mst.SetKbn == dragItem.SetKbn && mst.SetKbnEdaNo == dragItem.SetKbnEdaNo && mst.HpId == dragItem.HpId && mst.Level1 > 0 && mst.IsDeleted != 1 && mst.GenerationId == dragItem.GenerationId).ToList();

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

            _tenantDataContext.SaveChanges();
        }
        catch
        {
            return status;
        }
        return status;
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
                LevelDown(1, userId, listUpdateLevel1);

                foreach (var item in listDragItem)
                {
                    item.Level1 = dropItem.Level1 + 1;
                    item.UpdateDate = DateTime.UtcNow;
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
                    item.UpdateDate = DateTime.UtcNow;
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
            LevelDown(3, userId, listUpdateLevel3);

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
                LevelDown(2, userId, listDropUpdateLevel2);

                var listDragItem = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2).ToList();
                foreach (var item in listDragItem)
                {
                    item.Level2 = 1;
                    item.UpdateDate = DateTime.UtcNow;
                    item.UpdateId = userId;
                }
            }
            else
            {
                var listDropUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 > 0).ToList() ?? new();
                LevelDown(2, userId, listDropUpdateLevel2);

                var listDragUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 > dragItem.Level2).ToList() ?? new();
                LevelUp(2, userId, listDragUpdateLevel2);

                var listDrag = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2).ToList();
                foreach (var item in listDrag)
                {
                    item.Level1 = dropItem.Level1;
                    item.Level2 = 1;
                    item.UpdateDate = DateTime.UtcNow;
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
                    LevelDown(2, userId, listUpdateLevel2);

                    foreach (var item in listDragUpdateLevel2)
                    {
                        item.Level2 = dropItem.Level2 + 1;
                        item.UpdateDate = DateTime.UtcNow;
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
                        item.UpdateDate = DateTime.UtcNow;
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
                LevelDown(3, userId, listUpdateLevel3);

                var listDragUpdateLevel2 = listSetMsts?.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > dragItem.Level2).ToList() ?? new();
                LevelUp(2, userId, listDragUpdateLevel2);

                dragItem.Level1 = dropItem.Level1;
                dragItem.Level2 = dropItem.Level2;
                dragItem.Level3 = 1;
                dragItem.UpdateDate = DateTime.UtcNow;
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
            LevelDown(2, userId, listUpdateLevel2);

            dragItem.Level1 = dropItem.Level1;
            dragItem.Level2 = 1;
            dragItem.Level3 = 0;
            dragItem.UpdateDate = DateTime.UtcNow;
            dragItem.UpdateId = userId;
        }
        else if (dropItem.Level2 > 0 && dropItem.Level3 == 0)
        {
            if (dragItem.Level1 == dropItem.Level1 && dragItem.Level2 == dropItem.Level2)
            {
                var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > 0).ToList();
                LevelDown(3, userId, listUpdateLevel3);
                dragItem.Level3 = 1;
                dragItem.UpdateId = userId;
                dragItem.UpdateDate = DateTime.UtcNow;
            }
            else
            {
                var listDragUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > dragItem.Level3).ToList();
                LevelUp(3, userId, listDragUpdateLevel3);

                var listDropUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > 0).ToList();
                LevelDown(3, userId, listDropUpdateLevel3);

                dragItem.Level1 = dropItem.Level1;
                dragItem.Level2 = dropItem.Level2;
                dragItem.Level3 = 1;
                dragItem.UpdateDate = DateTime.UtcNow;
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
                    LevelDown(3, userId, listDropUpdateLevel3);

                    dragItem.Level3 = dropItem.Level3 + 1;
                    dragItem.UpdateDate = DateTime.UtcNow;
                    dragItem.UpdateId = userId;
                }
                else if (dragItem.Level3 < dropItem.Level3)
                {
                    var listDropUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > dragItem.Level3 && mst.Level3 <= dropItem.Level3).ToList();
                    LevelUp(3, userId, listDropUpdateLevel3);

                    dragItem.Level3 = dropItem.Level3 + 1;
                    dragItem.UpdateDate = DateTime.UtcNow;
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
            LevelDown(1, userId, listUpdateLevel1);
            foreach (var item in listDragUpdate)
            {
                item.Level1 = 1;
                item.UpdateDate = DateTime.UtcNow;
                item.UpdateId = userId;
            }
        }
        else if (dragItem.Level2 > 0 && dragItem.Level3 == 0)
        {
            var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > 0).ToList();
            LevelDown(1, userId, listUpdateLevel1);

            var listUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > dragItem.Level2).ToList();
            var listDragUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > 0).ToList();

            LevelUp(2, userId, listUpdateLevel2);

            // level3 => level2
            foreach (var levelNew in listDragUpdateLevel3)
            {
                levelNew.Level1 = 1;
                levelNew.Level2 = levelNew.Level3;
                levelNew.Level3 = 0;
                levelNew.UpdateDate = DateTime.UtcNow;
                levelNew.UpdateId = userId;
            }

            // level2 => level1
            dragItem.Level1 = 1;
            dragItem.Level2 = 0;
            dragItem.UpdateDate = DateTime.UtcNow;
            dragItem.UpdateId = userId;
        }
        else if (dragItem.Level2 > 0 && dragItem.Level3 > 0)
        {
            var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > 0).ToList();
            var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > dragItem.Level3).ToList();

            LevelDown(1, userId, listUpdateLevel1);

            LevelUp(3, userId, listUpdateLevel3);

            dragItem.Level1 = 1;
            dragItem.Level2 = 0;
            dragItem.Level3 = 0;
            dragItem.UpdateDate = DateTime.UtcNow;
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
            item.UpdateDate = DateTime.UtcNow;
            item.UpdateId = userId;
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
            item.UpdateDate = DateTime.UtcNow;
            item.UpdateId = userId;
        }
    }

    public bool PasteSetMst(int userId, int hpId, int setCdCopyItem, int setCdPasteItem)
    {
        bool status = false;
        try
        {
            var copyItem = _tenantNoTrackingDataContext.SetMsts.FirstOrDefault(mst => mst.SetCd == setCdCopyItem && mst.HpId == hpId);
            var pasteItem = _tenantNoTrackingDataContext.SetMsts.FirstOrDefault(mst => mst.SetCd == setCdPasteItem && mst.HpId == hpId);

            if (copyItem == null)
            {
                return status;
            }
            else if (pasteItem == null && setCdPasteItem != 0)
            {
                return status;
            }

            // Get all SetMst with dragItem SetKbn and dragItem SetKbnEdaNo
            var listSetMsts = _tenantNoTrackingDataContext.SetMsts.Where(mst => mst.SetKbn == copyItem.SetKbn && mst.SetKbnEdaNo == copyItem.SetKbnEdaNo && mst.HpId == copyItem.HpId && mst.Level1 > 0 && mst.IsDeleted != 1).ToList();
            if (pasteItem != null)
            {
                if (CountLevelItem(copyItem, listSetMsts) + GetLevelItem(pasteItem) > 3)
                {
                    return status;
                }
                if (copyItem.SetKbn != pasteItem.SetKbn || copyItem.SetKbnEdaNo != pasteItem.SetKbnEdaNo)
                {
                    return false;
                }
                if (GetLevelItem(pasteItem) == 1)
                {
                    // get index for paste
                    var lastItemLevel2 = listSetMsts.Where(item => item.Level1 == pasteItem.Level1 && item.Level2 > 0 && item.Level3 == 0).OrderByDescending(item => item.Level2).FirstOrDefault();
                    int indexPaste = (lastItemLevel2 != null ? lastItemLevel2.Level2 : 0) + 1;
                    status = PasteAction(indexPaste, userId, copyItem, pasteItem, listSetMsts);
                }
                else if (GetLevelItem(pasteItem) == 2)
                {
                    // get index for paste
                    var lastItemLevel3 = listSetMsts.Where(item => item.Level1 == pasteItem.Level1 && item.Level2 == pasteItem.Level2 && item.Level3 > 0).OrderByDescending(item => item.Level3).FirstOrDefault();
                    int indexPaste = (lastItemLevel3 != null ? lastItemLevel3.Level3 : 0) + 1;
                    status = PasteAction(indexPaste, userId, copyItem, pasteItem, listSetMsts);
                }
            }
            else
            {
                // get index for paste
                var lastItemLevel1 = listSetMsts.Where(item => item.Level2 == 0 && item.Level3 == 0).OrderByDescending(item => item.Level1).FirstOrDefault();
                int indexPaste = (lastItemLevel1 != null ? lastItemLevel1.Level1 : 0) + 1;
                status = PasteAction(indexPaste, userId, copyItem, null, listSetMsts);
            }

            return status;
        }
        catch (Exception)
        {
            return status;
        }
    }

    private bool PasteAction(int indexPaste, int userId, SetMst copyItem, SetMst? pasteItem, List<SetMst> listSetMsts)
    {
        bool status = false;
        var executionStrategy = _tenantDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using (var transaction = _tenantDataContext.Database.BeginTransaction())
                {
                    try
                    {
                        // paste SetMst to list super set
                        List<SetMst> listCopyItems = new();
                        List<SetMst> listPasteItems = new();
                        switch (GetLevelItem(copyItem))
                        {
                            case 1:
                                listCopyItems = listSetMsts.Where(item => item.Level1 == copyItem.Level1).ToList();
                                break;
                            case 2:
                                listCopyItems = listSetMsts.Where(item => item.Level1 == copyItem.Level1 && item.Level2 == copyItem.Level2).ToList();
                                break;
                            case 3:
                                listCopyItems = listSetMsts.Where(item => item.Level1 == copyItem.Level1 && item.Level2 == copyItem.Level2 && item.Level3 == copyItem.Level3).ToList();
                                break;
                        }

                        // Convert SetMst copy to SetMst paste
                        foreach (var item in listCopyItems)
                        {
                            SetMst setMst = item.DeepClone();
                            setMst.SetCd = 0;
                            setMst.CreateDate = DateTime.UtcNow;
                            setMst.CreateId = userId;
                            setMst.UpdateDate = DateTime.UtcNow;
                            setMst.UpdateId = userId;
                            listPasteItems.Add(setMst);
                        }

                        _tenantDataContext.SetMsts.AddRange(listPasteItems);
                        _tenantDataContext.SaveChanges();

                        // get paste content item
                        Dictionary<int, SetMst> dictionarySetMstMap = new();
                        foreach (var copy in listCopyItems)
                        {
                            var pasteItemToMap = listPasteItems.FirstOrDefault(paste => paste.Level1 == copy.Level1 && paste.Level2 == copy.Level2 && paste.Level3 == copy.Level3);
                            dictionarySetMstMap.Add(copy.SetCd, pasteItemToMap ?? new SetMst());
                        }

                        var listCopySetCds = listCopyItems.Select(item => item.SetCd).ToList();
                        AddNewItemToSave(userId, listCopySetCds, dictionarySetMstMap);

                        // Set level for item 
                        ReSetLevelForItem(indexPaste, copyItem, pasteItem, listPasteItems);

                        _tenantDataContext.SaveChanges();
                        status = true;
                        transaction.Commit();
                    }
                    catch
                    {
                        status = false;
                        transaction.Rollback();
                    }
                }
            }
            );
        return status;
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
                    else
                    {
                        foreach (var item in listPasteItems)
                        {
                            item.Level1 = pasteItem.Level1;
                            item.Level2 = indexPaste;
                        }
                    }
                    break;
                // if paste item is level 1
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
                listSamelevel = setMsts.Where(item => item.Level1 == setMst.Level1).ToList();
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
        // Order inf
        var listCopySetOrderInfs = _tenantNoTrackingDataContext.SetOdrInf.Where(item => listCopySetCds.Contains(item.SetCd) && item.IsDeleted != 1).ToList();
        var listPasteSetOrderInfs = new List<SetOdrInf>();
        foreach (var item in listCopySetOrderInfs)
        {
            SetOdrInf order = item.DeepClone();
            order.Id = 0;
            order.SetCd = dictionarySetMstMap[order.SetCd].SetCd;
            order.CreateDate = DateTime.UtcNow;
            order.CreateId = userId;
            order.UpdateDate = DateTime.UtcNow;
            order.UpdateId = userId;
            listPasteSetOrderInfs.Add(order);
        }
        _tenantDataContext.SetOdrInf.AddRange(listPasteSetOrderInfs);

        // Order inf detail
        var listCopySetOrderInfDetails = _tenantNoTrackingDataContext.SetOdrInfDetail.Where(item => listCopySetCds.Contains(item.SetCd)).ToList();
        var listPasteSetOrderInfDetails = new List<SetOdrInfDetail>();
        foreach (var item in listCopySetOrderInfDetails)
        {
            SetOdrInfDetail detail = item.DeepClone();
            detail.SetCd = dictionarySetMstMap[detail.SetCd].SetCd;
            listPasteSetOrderInfDetails.Add(detail);
        }
        _tenantDataContext.SetOdrInfDetail.AddRange(listPasteSetOrderInfDetails);

        // Karte inf
        var listCopySetKarteInfs = _tenantNoTrackingDataContext.SetKarteInf.Where(item => listCopySetCds.Contains(item.SetCd) && item.IsDeleted != 1).ToList();
        var listPasteSetKarteInfs = new List<SetKarteInf>();
        foreach (var item in listCopySetKarteInfs)
        {
            SetKarteInf karte = item.DeepClone();
            karte.SetCd = dictionarySetMstMap[karte.SetCd].SetCd;
            karte.CreateDate = DateTime.UtcNow;
            karte.CreateId = userId;
            karte.UpdateDate = DateTime.UtcNow;
            karte.UpdateId = userId;
            listPasteSetKarteInfs.Add(karte);
        }
        _tenantDataContext.SetKarteInf.AddRange(listPasteSetKarteInfs);

        // Set byomei
        var listCopySetByomeies = _tenantNoTrackingDataContext.SetByomei.Where(item => listCopySetCds.Contains(item.SetCd) && item.IsDeleted != 1).ToList();
        var listPasteSetByomeies = new List<SetByomei>();
        foreach (var item in listCopySetByomeies)
        {
            SetByomei karte = item.DeepClone();
            karte.SetCd = dictionarySetMstMap[karte.SetCd].SetCd;
            karte.CreateDate = DateTime.UtcNow;
            karte.CreateId = userId;
            karte.UpdateDate = DateTime.UtcNow;
            karte.UpdateId = userId;
            listPasteSetByomeies.Add(karte);
        }
        _tenantDataContext.SetByomei.AddRange(listPasteSetByomeies);
    }
}
