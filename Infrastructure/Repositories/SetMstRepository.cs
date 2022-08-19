﻿using Domain.Models.SetMst;
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

    public bool SaveSetMstModel(SetMstModel setMstModel, int userId, int sinDate)
    {
        bool status = false;
        if (setMstModel.Level1 == 0)
        {
            return status;
        }
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
            oldSetMst = oldSetMst != null ? oldSetMst : new SetMst();
            var setMst = ConvertSetMstModelToSetMst(oldSetMst, setMstModel, userId);

            if (!isDelete)
            {
                // set status for IsDelete
                setMst.IsDeleted = 0;

                // If SetMst is add new
                if (setMstModel.SetCd == 0)
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

                        var listSetMstLevel3Deletes = listSetMstLevel3.Where(item => item.Level2 == level2.Level2).ToList();
                        foreach (var level3 in listSetMstLevel3Deletes)
                        {
                            level3.IsDeleted = 1;
                            level3.UpdateDate = DateTime.UtcNow;
                            level3.UpdateId = userId;
                        }
                    }
                }
            }
            _tenantDataContext.SaveChanges();
            status = true;
        }
        catch (Exception)
        {
            return status;
        }
        return status;
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
}
