using Domain.Models.Family;
using Entity.Tenant;
using Helper.Common;
using Helper.Constant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Infrastructure.Repositories;

public class FamilyRepository : RepositoryBase, IFamilyRepository
{
    public FamilyRepository(ITenantProvider tenantProvider) : base(tenantProvider) { }

    public List<FamilyModel> GetListFamily(int hpId, long ptId, int sinDate)
    {
        var ptFamilys = NoTrackingDataContext.PtFamilys.Where(item => item.HpId == hpId
                                                                        && item.PtId == ptId
                                                                        && item.IsDeleted != 1)
                                                        .ToList();

        var listFamilyPtIds = ptFamilys.Select(item => item.FamilyPtId).ToList();
        var listFamilyIds = ptFamilys.Select(item => item.FamilyId).ToList();

        var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                && listFamilyPtIds.Contains(item.PtId)
                                                                && item.IsDelete != 1)
                                                 .ToList();

        var ptFamilyRekis = NoTrackingDataContext.PtFamilyRekis.Where(u => u.HpId == hpId
                                                                            && listFamilyIds.Contains(u.FamilyId)
                                                                            && !string.IsNullOrEmpty(u.Byomei)
                                                                            && u.IsDeleted != 1)
                                                               .OrderBy(u => u.SortNo)
                                                               .ToList();

        return ptFamilys.Select(item => ConvertToFamilyModel(sinDate, item, ptInfs, ptFamilyRekis))
                        .OrderBy(item => item.SortNo)
                        .ToList();
    }

    public List<FamilyModel> GetListFamilyReverser(int hpId, long familyPtId, List<long> listPtIdInput)
    {
        var listPtIdExist = NoTrackingDataContext.PtFamilys.Where(item => item.HpId == hpId
                                                                    && listPtIdInput.Contains(item.PtId)
                                                                    && item.FamilyPtId == familyPtId
                                                                    && item.IsDeleted != 1)
                                                        .Select(item => item.PtId)
                                                        .ToList();

        var listPtInf = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                && listPtIdInput.Contains(item.PtId)
                                                                && !listPtIdExist.Contains(item.PtId)
                                                                && item.IsDelete != 1)
                                                 .ToList();

        return listPtInf.Select(item => new FamilyModel(
                                                            item.PtId,
                                                            item.PtNum,
                                                            item.Name ?? string.Empty,
                                                            item.KanaName ?? string.Empty,
                                                            item.Sex,
                                                            item.Birthday,
                                                            item.IsDead
                                                        )).ToList();
    }

    public bool SaveListFamily(int hpId, int userId, List<FamilyModel> listFamily)
    {
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        return executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    if (SaveListFamilyAction(hpId, userId, listFamily))
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

    public bool CheckExistListFamilyReki(int hpId, List<long> listFamilyRekiId)
    {
        var countFromDB = NoTrackingDataContext.PtFamilyRekis.Count(x => x.HpId == hpId && listFamilyRekiId.Contains(x.Id) && x.IsDeleted != 1);
        return listFamilyRekiId.Count == countFromDB;
    }

    public List<RaiinInfModel> GetListRaiinInfByPtId(int hpId, long ptId)
    {
        var listRaiinInf = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                        && item.PtId == ptId
                                                                        && item.IsDeleted != 1)
                                                          .ToList();
        var listTantoId = listRaiinInf.Select(item => item.TantoId).ToList();
        var listKaId = listRaiinInf.Select(item => item.KaId).ToList();
        var listHokenPId = listRaiinInf.Select(item => item.HokenPid).ToList();

        var listDoctor = NoTrackingDataContext.UserMsts.Where(item => item.IsDeleted != 1
                                                                    && item.JobCd == JobCdConstant.Doctor
                                                                    && listTantoId.Contains(item.UserId))
                                                      .ToList();
        var listKaMst = NoTrackingDataContext.KaMsts.Where(item => item.IsDeleted != 1
                                                                   && listKaId.Contains(item.KaId))
                                                    .ToList();
        var listHokenPattern = NoTrackingDataContext.PtHokenPatterns.Where(item => item.IsDeleted != 1
                                                                                && listHokenPId.Contains(item.HokenPid))
                                                                    .ToList();
        var listKohiHokenId = listHokenPattern.Select(item => item.Kohi1Id).ToList();
        listKohiHokenId.AddRange(listHokenPattern.Select(item => item.Kohi2Id).ToList());
        listKohiHokenId.AddRange(listHokenPattern.Select(item => item.Kohi3Id).ToList());
        listKohiHokenId.AddRange(listHokenPattern.Select(item => item.Kohi4Id).ToList());

        var ptKohis = NoTrackingDataContext.PtKohis.Where(item => item.IsDeleted != 1
                                                                    && item.PtId == ptId
                                                                    && listKohiHokenId.Contains(item.HokenId))
                                                          .ToList();
        return listRaiinInf.Select(item => ConvertToRaiinInfModel(item, listDoctor, listKaMst, listHokenPattern, ptKohis))
                           .OrderByDescending(item => item.SinDate)
                           .ToList();
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

    private RaiinInfModel ConvertToRaiinInfModel(RaiinInf raiinInf, List<UserMst> listDoctor, List<KaMst> listKaMst, List<PtHokenPattern> listHokenPattern, List<PtKohi> listPtKohi)
    {
        var doctor = listDoctor.FirstOrDefault(item => item.UserId == raiinInf.TantoId);
        var kaMst = listKaMst.FirstOrDefault(item => item.KaId == raiinInf.KaId);
        var hokenPattern = listHokenPattern.FirstOrDefault(item => item.HokenPid == raiinInf.HokenPid);
        var ptKohi1 = listPtKohi.FirstOrDefault(item => item.HokenId == hokenPattern?.Kohi1Id);
        var ptKohi2 = listPtKohi.FirstOrDefault(item => item.HokenId == hokenPattern?.Kohi2Id);
        var ptKohi3 = listPtKohi.FirstOrDefault(item => item.HokenId == hokenPattern?.Kohi3Id);
        var ptKohi4 = listPtKohi.FirstOrDefault(item => item.HokenId == hokenPattern?.Kohi4Id);
        return new RaiinInfModel(
                raiinInf.PtId,
                raiinInf.SinDate,
                raiinInf.RaiinNo,
                raiinInf.KaId,
                kaMst?.KaName ?? string.Empty,
                raiinInf.TantoId,
                doctor?.Name ?? string.Empty,
                hokenPattern?.HokenPid ?? CommonConstants.InvalidId,
                hokenPattern?.StartDate ?? 0,
                hokenPattern?.EndDate ?? 0,
                hokenPattern?.HokenSbtCd ?? CommonConstants.InvalidId,
                hokenPattern?.HokenKbn ?? CommonConstants.InvalidId,
                ptKohi1?.HokenSbtKbn ?? CommonConstants.InvalidId,
                ptKohi1?.Houbetu ?? string.Empty,
                ptKohi2?.HokenSbtKbn ?? CommonConstants.InvalidId,
                ptKohi2?.Houbetu ?? string.Empty,
                ptKohi3?.HokenSbtKbn ?? CommonConstants.InvalidId,
                ptKohi3?.Houbetu ?? string.Empty,
                ptKohi4?.HokenSbtKbn ?? CommonConstants.InvalidId,
                ptKohi4?.Houbetu ?? string.Empty
            );
    }

    private bool SaveListFamilyAction(int hpId, int userId, List<FamilyModel> listFamily)
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
                if (!SaveListFamilyReki(hpId, userId, ptFamilyEntity.FamilyPtId, ptFamilyEntity.FamilyId, listFamilyRekiDB, familyModel.ListPtFamilyRekis))
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
                if (!SaveListFamilyReki(hpId, userId, ptFamilyEntity.FamilyPtId, ptFamilyEntity.FamilyId, listFamilyRekiDB, familyModel.ListPtFamilyRekis))
                {
                    return false;
                }
            }
        }
        TrackingDataContext.SaveChanges();
        return true;
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

    private bool SaveListFamilyReki(int hpId, int userId, long familyPtId, long familyId, List<PtFamilyReki> listPtFamilyRekiEntity, List<PtFamilyRekiModel> listPtFamilyRekiModel)
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
