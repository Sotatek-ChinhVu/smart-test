using Domain.Models.Family;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class FamilyRepository : RepositoryBase, IFamilyRepository
{
    public FamilyRepository(ITenantProvider tenantProvider) : base(tenantProvider) { }

    public List<FamilyModel> GetFamilyList(int hpId, long ptId, int sinDate)
    {
        var ptFamilys = NoTrackingDataContext.PtFamilys.Where(item => item.HpId == hpId
                                                                        && item.PtId == ptId
                                                                        && item.IsDeleted != 1)
                                                        .ToList();

        var familyPtIdLists = ptFamilys.Select(item => item.FamilyPtId).ToList();
        var familyIdLists = ptFamilys.Select(item => item.FamilyId).ToList();

        var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                && familyPtIdLists.Contains(item.PtId)
                                                                && item.IsDelete != 1)
                                                 .ToList();

        var ptFamilyRekis = NoTrackingDataContext.PtFamilyRekis.Where(u => u.HpId == hpId
                                                                            && familyIdLists.Contains(u.FamilyId)
                                                                            && !string.IsNullOrEmpty(u.Byomei)
                                                                            && u.IsDeleted != 1)
                                                               .OrderBy(u => u.SortNo)
                                                               .ToList();

        return ptFamilys.Select(item => ConvertToFamilyModel(sinDate, item, ptInfs, ptFamilyRekis))
                        .OrderBy(item => item.SortNo)
                        .ToList();
    }

    public List<FamilyModel> GetFamilyReverserList(int hpId, long familyPtId, List<long> ptIdInputList)
    {
        var ptIdExistList = NoTrackingDataContext.PtFamilys.Where(item => item.HpId == hpId
                                                                    && ptIdInputList.Contains(item.PtId)
                                                                    && item.FamilyPtId == familyPtId
                                                                    && item.IsDeleted != 1)
                                                        .Select(item => item.PtId)
                                                        .ToList();

        var ptInfList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                && ptIdInputList.Contains(item.PtId)
                                                                && !ptIdExistList.Contains(item.PtId)
                                                                && item.IsDelete != 1)
                                                 .ToList();

        return ptInfList.Select(item => new FamilyModel(
                                                            item.PtId,
                                                            item.PtNum,
                                                            item.Name ?? string.Empty,
                                                            item.KanaName ?? string.Empty,
                                                            item.Sex,
                                                            item.Birthday,
                                                            item.IsDead
                                                        )).ToList();
    }

    public bool SaveFamilyList(int hpId, int userId, List<FamilyModel> familyList)
    {
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        return executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    if (SaveFamilyListAction(hpId, userId, familyList))
                    {
                        transaction.Commit();
                        return true;
                    }
                    transaction.Rollback();
                    return false;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            });
    }

    public List<FamilyModel> GetListByPtId(int hpId, long ptId)
    {
        return NoTrackingDataContext.PtFamilys.Where(x => x.HpId == hpId
                                                          && x.PtId == ptId
                                                          && x.IsDeleted != 1)
                                              .Select(item => new FamilyModel(
                                                item.FamilyId,
                                                item.PtId,
                                                item.ZokugaraCd ?? string.Empty,
                                                item.FamilyPtId
                                               ))
                                              .ToList();
    }

    public bool CheckExistFamilyRekiList(int hpId, List<long> familyRekiIdList)
    {
        var countFromDB = NoTrackingDataContext.PtFamilyRekis.Count(x => x.HpId == hpId && familyRekiIdList.Contains(x.Id) && x.IsDeleted != 1);
        return familyRekiIdList.Count == countFromDB;
    }

    #region private function
    private FamilyModel ConvertToFamilyModel(int sinDate, PtFamily ptFamily, List<PtInf> ptInfs, List<PtFamilyReki> ptFamilyRekis)
    {
        var ptInf = ptInfs.FirstOrDefault(item => ptFamily.FamilyPtId > 0 && item.PtId == ptFamily.FamilyPtId);
        var ptFamilyRekiFilter = ptFamilyRekis.Where(item => item.FamilyId == ptFamily.FamilyId)
                                              .Select(item => ConvertToPtFamilyRekiModel(item))
                                              .ToList();

        long familyPtNum = ptInf != null ? ptInf.PtNum : 0;
        string name = ptInf != null ? ptInf.Name ?? string.Empty : ptFamily.Name ?? string.Empty;
        int sex = ptInf != null ? ptInf.Sex : ptFamily.Sex;
        int birthday = ptInf != null ? ptInf.Birthday : ptFamily.Birthday;
        int isDead = ptInf != null ? ptInf.IsDead : ptFamily.IsDead;
        return new FamilyModel(
                                    ptFamily.FamilyId,
                                    ptFamily.SeqNo,
                                    ptFamily.ZokugaraCd ?? string.Empty,
                                    ptFamily.FamilyPtId,
                                    familyPtNum,
                                    name,
                                    ptFamily.KanaName ?? string.Empty,
                                    sex,
                                    birthday,
                                    CIUtil.SDateToAge(birthday, sinDate),
                                    isDead,
                                    ptFamily.IsSeparated,
                                    ptFamily.Biko ?? string.Empty,
                                    ptFamily.SortNo,
                                    ptFamilyRekiFilter
                               );
    }

    private PtFamilyRekiModel ConvertToPtFamilyRekiModel(PtFamilyReki ptFamilyReki)
    {
        return new PtFamilyRekiModel(
                                        ptFamilyReki.Id,
                                        ptFamilyReki.ByomeiCd ?? string.Empty,
                                        ptFamilyReki.Byomei ?? string.Empty,
                                        ptFamilyReki.Cmt ?? string.Empty,
                                        ptFamilyReki.SortNo
                                    );
    }

    private bool SaveFamilyListAction(int hpId, int userId, List<FamilyModel> listFamily)
    {
        var listFamilyPtId = listFamily.Where(item => item.FamilyPtId > 0 && !item.IsDeleted).Select(item => item.FamilyPtId).ToList();
        var listFamilyId = listFamily.Select(item => item.FamilyId).ToList();
        var listFamilyDB = TrackingDataContext.PtFamilys.Where(item => listFamilyId.Contains(item.FamilyId) && item.IsDeleted != 1);
        var listFamilyRekiDB = TrackingDataContext.PtFamilyRekis.Where(item => listFamilyId.Contains(item.FamilyId) && item.IsDeleted != 1).ToList();
        var listPtInf = TrackingDataContext.PtInfs.Where(item => item.IsDelete != 1 && listFamilyPtId.Contains(item.PtId)).ToList();

        foreach (var familyModel in listFamily)
        {
            if (familyModel.FamilyId <= 0 && !familyModel.IsDeleted)
            {
                var ptFamilyEntity = ConvertToNewPtFamily(hpId, userId, familyModel);
                TrackingDataContext.PtFamilys.Add(ptFamilyEntity);
                TrackingDataContext.SaveChanges();
                UpdatePtInf(listPtInf, familyModel.FamilyPtId, familyModel.IsDead);
                if (!SaveFamilyRekiList(hpId, userId, ptFamilyEntity.FamilyPtId, ptFamilyEntity.FamilyId, listFamilyRekiDB, familyModel.ListPtFamilyRekis))
                {
                    return false;
                }
            }
            else
            {
                var ptFamilyEntity = listFamilyDB.FirstOrDefault(item => item.FamilyId == familyModel.FamilyId);
                if (ptFamilyEntity == null)
                {
                    return false;
                }
                ptFamilyEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                ptFamilyEntity.UpdateId = userId;
                if (familyModel.IsDeleted)
                {
                    ptFamilyEntity.IsDeleted = 1;
                    continue;
                }
                ptFamilyEntity.ZokugaraCd = familyModel.ZokugaraCd;
                ptFamilyEntity.SortNo = familyModel.SortNo;
                ptFamilyEntity.FamilyPtId = familyModel.FamilyPtId;
                ptFamilyEntity.KanaName = familyModel.KanaName;
                ptFamilyEntity.Name = familyModel.Name;
                ptFamilyEntity.Sex = familyModel.Sex;
                ptFamilyEntity.Birthday = familyModel.Birthday;
                ptFamilyEntity.IsDead = familyModel.IsDead;
                ptFamilyEntity.IsSeparated = familyModel.IsSeparated;
                ptFamilyEntity.Biko = familyModel.Biko;
                UpdatePtInf(listPtInf, familyModel.FamilyPtId, familyModel.IsDead);
                if (!SaveFamilyRekiList(hpId, userId, ptFamilyEntity.FamilyPtId, ptFamilyEntity.FamilyId, listFamilyRekiDB, familyModel.ListPtFamilyRekis))
                {
                    return false;
                }
            }
        }
        return TrackingDataContext.SaveChanges() > 0;
    }

    private PtFamily ConvertToNewPtFamily(int hpId, int userId, FamilyModel model)
    {
        PtFamily ptFamily = new();
        ptFamily.FamilyId = 0;
        ptFamily.HpId = hpId;
        ptFamily.PtId = model.PtId;
        ptFamily.ZokugaraCd = model.ZokugaraCd;
        ptFamily.SortNo = model.SortNo;
        ptFamily.FamilyPtId = model.FamilyPtId;
        ptFamily.KanaName = model.KanaName;
        ptFamily.Name = model.Name;
        ptFamily.Sex = model.Sex;
        ptFamily.Birthday = model.Birthday;
        ptFamily.IsDead = model.IsDead;
        ptFamily.IsSeparated = model.IsSeparated;
        ptFamily.Biko = model.Biko;
        ptFamily.CreateDate = CIUtil.GetJapanDateTimeNow();
        ptFamily.CreateId = userId;
        ptFamily.UpdateDate = CIUtil.GetJapanDateTimeNow();
        ptFamily.UpdateId = userId;
        ptFamily.IsDeleted = 0;
        return ptFamily;
    }

    private bool SaveFamilyRekiList(int hpId, int userId, long familyPtId, long familyId, List<PtFamilyReki> listPtFamilyRekiEntity, List<PtFamilyRekiModel> listPtFamilyRekiModel)
    {
        foreach (var familyRekiModel in listPtFamilyRekiModel)
        {
            if (familyRekiModel.Id <= 0 && !familyRekiModel.IsDeleted)
            {
                var familyReki = ConvertToNewPtFamilyReki(hpId, userId, familyPtId, familyId, familyRekiModel);
                TrackingDataContext.PtFamilyRekis.Add(familyReki);
            }
            else
            {
                var ptFamilyRekiEntity = listPtFamilyRekiEntity.FirstOrDefault(item => item.Id == familyRekiModel.Id);
                if (ptFamilyRekiEntity == null)
                {
                    return false;
                }
                ptFamilyRekiEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
                ptFamilyRekiEntity.UpdateId = userId;
                if (familyRekiModel.IsDeleted)
                {
                    ptFamilyRekiEntity.IsDeleted = 1;
                    continue;
                }
                ptFamilyRekiEntity.SortNo = familyRekiModel.SortNo;
                ptFamilyRekiEntity.ByomeiCd = familyRekiModel.ByomeiCd;
                ptFamilyRekiEntity.Byomei = familyRekiModel.Byomei;
                ptFamilyRekiEntity.Cmt = familyRekiModel.Cmt;
            }
        }
        return true;
    }

    private PtFamilyReki ConvertToNewPtFamilyReki(int hpId, int userId, long familyPtId, long familyId, PtFamilyRekiModel model)
    {
        PtFamilyReki ptFamilyRekiEntity = new();
        ptFamilyRekiEntity.Id = 0;
        ptFamilyRekiEntity.HpId = hpId;
        ptFamilyRekiEntity.PtId = familyPtId;
        ptFamilyRekiEntity.FamilyId = familyId;
        ptFamilyRekiEntity.SortNo = model.SortNo;
        ptFamilyRekiEntity.ByomeiCd = model.ByomeiCd;
        ptFamilyRekiEntity.Byomei = model.Byomei;
        ptFamilyRekiEntity.Cmt = model.Cmt;
        ptFamilyRekiEntity.IsDeleted = 0;
        ptFamilyRekiEntity.CreateDate = CIUtil.GetJapanDateTimeNow();
        ptFamilyRekiEntity.CreateId = userId;
        ptFamilyRekiEntity.UpdateDate = CIUtil.GetJapanDateTimeNow();
        ptFamilyRekiEntity.UpdateId = userId;
        return ptFamilyRekiEntity;
    }

    private void UpdatePtInf(List<PtInf> ptInfs, long ptId, int isDead)
    {
        var ptInf = ptInfs.FirstOrDefault(item => item.PtId == ptId);
        if (ptInf != null)
        {
            ptInf.IsDead = isDead;
        }
    }
    #endregion

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
