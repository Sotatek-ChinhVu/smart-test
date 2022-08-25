using Domain.Models.SetMst;
using Entity.Tenant;
using Infrastructure.Interfaces;
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
                    s.IsGroup
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
                    setMst.IsGroup
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

    public bool ReorderSetMst(int userId, SetMstModel setMstModelDragItem, SetMstModel setMstModelDropItem)
    {
        bool status = false;
        try
        {
            var listSetMsts = _tenantDataContext.SetMsts.Where(mst => mst.SetKbn == setMstModelDragItem.SetKbn && mst.SetKbnEdaNo == setMstModelDragItem.SetKbnEdaNo && mst.HpId == setMstModelDragItem.HpId && mst.Level1 > 0 && mst.IsDeleted != 1).ToList();
            var dragItem = listSetMsts.FirstOrDefault(mst => mst.SetCd == setMstModelDragItem.SetCd && mst.GenerationId == setMstModelDragItem.GenerationId);
            var dropItem = listSetMsts.FirstOrDefault(mst => mst.SetCd == setMstModelDropItem.SetCd && mst.GenerationId == setMstModelDropItem.GenerationId);

            if (dragItem == null)
            {
                return status;
            }
            if (dropItem == null && setMstModelDropItem.Level1 > 0)
            {
                return status;
            }
            if (dropItem != null)
            {
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
            else if (setMstModelDropItem.Level1 == 0)
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
                var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > dropItem.Level1 && mst.Level1 <= dragItem.Level1).ToList();
                LevelUp(1, userId, listUpdateLevel1);

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
                LevelDown(1, userId, listUpdateLevel1);

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
            if (listDragItem.Count(item => item.Level2 > 0) > 0)
            {
                return false;
            }
            var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > 0).ToList();
            LevelUp(3, userId, listUpdateLevel3);

            var listUpdateLevel1 = listSetMsts.Where(mst => mst.Level1 > dragItem.Level1).ToList();
            LevelDown(1, userId, listUpdateLevel1);

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
                LevelUp(2, userId, listDropUpdateLevel2);

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
                List<SetMst> listDropUpdateLevel2 = new();
                if (dragItem.Level1 != dropItem.Level1)
                {
                    listDropUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 > 0).ToList();
                }
                else
                {
                    listDropUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > 0 && mst.Level2 < dragItem.Level2).ToList();
                }
                LevelUp(2, userId, listDropUpdateLevel2);

                var listDragUpdate = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2).ToList();
                foreach (var item in listDragUpdate)
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
                    LevelUp(2, userId, listUpdateLevel2);

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
                    LevelDown(2, userId, listUpdateLevel2);

                    foreach (var item in listDragUpdateLevel2)
                    {
                        item.Level2 = dropItem.Level2;
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
                if (listSetMsts.Count(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > 0) > 0)
                {
                    return false;
                }
                var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > 0).ToList();
                LevelUp(3, userId, listUpdateLevel3);

                var listDragUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > dragItem.Level2).ToList();
                LevelDown(2, userId, listDragUpdateLevel2);

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
            LevelDown(3, userId, listUpdateLevel3);

            var listUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 > 0).ToList();
            LevelUp(2, userId, listUpdateLevel2);

            dragItem.Level1 = 1;
            dragItem.Level2 = 0;
            dragItem.Level3 = 0;
            dragItem.UpdateDate = DateTime.UtcNow;
            dragItem.UpdateId = userId;
        }
        else if (dropItem.Level2 > 0 && dropItem.Level3 == 0)
        {
            if (dragItem.Level1 == dropItem.Level1 && dragItem.Level2 == dropItem.Level2)
            {
                var listUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dropItem.Level1 && mst.Level2 == dropItem.Level2 && mst.Level3 > 0).ToList();
                LevelUp(3, userId, listUpdateLevel3);
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
                    LevelUp(3, userId, listDropUpdateLevel3);

                    dragItem.Level3 = dropItem.Level3 + 1;
                    dragItem.UpdateDate = DateTime.UtcNow;
                    dragItem.UpdateId = userId;
                }
                else if (dragItem.Level3 < dropItem.Level3)
                {
                    var listDropUpdateLevel3 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2 && mst.Level3 > dragItem.Level3 && mst.Level3 <= dropItem.Level3).ToList();
                    LevelDown(3, userId, listDropUpdateLevel3);

                    dragItem.Level3 = dropItem.Level3;
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
            LevelUp(1, userId, listUpdateLevel1);
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
            LevelUp(1, userId, listUpdateLevel1);

            var listUpdateLevel2 = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 > dragItem.Level2).ToList();
            LevelUp(2, userId, listUpdateLevel2);

            var listDragUpdate = listSetMsts.Where(mst => mst.Level1 == dragItem.Level1 && mst.Level2 == dragItem.Level2).ToList();
            // level3 => level2
            var listLevel2New = listDragUpdate.Where(mst => mst.Level3 > 0).ToList();
            foreach (var levelNew in listLevel2New)
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

            LevelUp(1, userId, listUpdateLevel1);

            LevelDown(3, userId, listUpdateLevel3);

            dragItem.Level1 = 1;
            dragItem.Level2 = 0;
            dragItem.Level3 = 0;
            dragItem.UpdateDate = DateTime.UtcNow;
            dragItem.UpdateId = userId;
        }
        return true;
    }
    private void LevelUp(int level, int userId, List<SetMst> listUpdate)
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

    private void LevelDown(int level, int userId, List<SetMst> listUpdate)
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
}
