using Domain.Models.Family;
using Entity.Tenant;
using Helper.Common;
using Helper.Constant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

        var familyPtIdLists = ptFamilys.Select(item => item.FamilyPtId).Distinct().ToList();
        var familyIdLists = ptFamilys.Select(item => item.FamilyId).Distinct().ToList();

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

    public List<FamilyModel> GetFamilyListByPtId(int hpId, long ptId, int sinDate)
    {
        var ptFamilyRepo = NoTrackingDataContext.PtFamilys.Where(item =>
            item.HpId == hpId && item.PtId == ptId && item.IsDeleted == 0);
        var ptInfRepo = NoTrackingDataContext.PtInfs.Where(item =>
            item.HpId == hpId && item.IsDelete == 0);
        var ptFamilyRekis = NoTrackingDataContext.PtFamilyRekis
            .Where(u => u.HpId == hpId && !string.IsNullOrEmpty(u.Byomei) && u.IsDeleted == 0)
            .OrderBy(u => u.SortNo);
        var query =
        (
            from ptFamily in ptFamilyRepo
            join ptInf in ptInfRepo on ptFamily.FamilyPtId equals ptInf.PtId into ptInfList
            from ptInfItem in ptInfList.DefaultIfEmpty()
            join rekiInfo in ptFamilyRekis on ptFamily.FamilyId equals rekiInfo.FamilyId into listPtFamilyRekiInfo
            select new
            {
                PtFamily = ptFamily,
                PtInf = ptInfItem,
                ListPtFamilyRekiInfo = listPtFamilyRekiInfo
            }
        );
        return query.AsEnumerable().Select(data => new FamilyModel(
                data.PtFamily.FamilyId,
                data.PtFamily.PtId,
                data.PtFamily.SeqNo,
                data.PtFamily.ZokugaraCd ?? string.Empty,
                data.PtFamily.FamilyId,
                data.PtInf?.PtNum ?? 0,
                data.PtFamily.Name ?? string.Empty,
                data.PtFamily.KanaName ?? string.Empty,
                data.PtFamily.Sex,
                data.PtFamily.Birthday,
                CIUtil.SDateToAge(data.PtInf?.Birthday ?? 0, sinDate),
                data.PtFamily.IsDead,
                data.PtFamily.IsSeparated,
                data.PtFamily.Biko ?? string.Empty,
                data.PtFamily.SortNo,
                data.ListPtFamilyRekiInfo.Select(
                        r => new PtFamilyRekiModel(
                                r.Id,
                                r.ByomeiCd ?? string.Empty,
                                r.Byomei ?? string.Empty,
                                r.Cmt ?? string.Empty,
                                r.SortNo
                            )
                ).ToList(),
                string.Empty
            )).OrderBy(item => item.SortNo).ToList();
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
                                                            item.IsDead,
                                                            int.Parse(CIUtil.GetJapanDateTimeNow().ToString("yyyyMMdd"))
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
                    throw;
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
        familyRekiIdList = familyRekiIdList.Distinct().ToList();
        var countFromDB = NoTrackingDataContext.PtFamilyRekis.Count(x => x.HpId == hpId && familyRekiIdList.Contains(x.Id) && x.IsDeleted != 1);
        return familyRekiIdList.Count == countFromDB;
    }

    public List<RaiinInfModel> GetRaiinInfListByPtId(int hpId, long ptId)
    {
        var raiinInfList = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                        && item.PtId == ptId
                                                                        && item.IsDeleted != 1
                                                                        && item.Status >= 3)
                                                          .ToList();
        var tantoIdList = raiinInfList.Select(item => item.TantoId).ToList();
        var kaIdList = raiinInfList.Select(item => item.KaId).ToList();
        var hokenPIdList = raiinInfList.Select(item => item.HokenPid).ToList();

        var doctorList = NoTrackingDataContext.UserMsts.Where(item => item.IsDeleted != 1
                                                                    && item.JobCd == JobCdConstant.Doctor
                                                                    && tantoIdList.Contains(item.UserId))
                                                      .ToList();

        var kaMstList = NoTrackingDataContext.KaMsts.Where(item => item.IsDeleted != 1
                                                                   && kaIdList.Contains(item.KaId))
                                                    .ToList();

        var hokenPatternList = NoTrackingDataContext.PtHokenPatterns.Where(item => item.IsDeleted != 1
                                                                                   && hokenPIdList.Contains(item.HokenPid))
                                                                    .ToList();

        var hokenIdList = hokenPatternList.Select(item => item.HokenId).Distinct().ToList();
        var hokenInfList = NoTrackingDataContext.PtHokenInfs.Where(item => item.IsDeleted != 1
                                                                           && hokenIdList.Contains(item.HokenId))
                                                            .ToList();

        return raiinInfList.Select(item => ConvertToRaiinInfModel(item, doctorList, kaMstList, hokenPatternList, hokenInfList))
                           .OrderByDescending(item => item.SinDate)
                           .ToList();
    }

    public List<FamilyModel> GetMaybeFamilyList(int hpId, long ptId, int sinDate)
    {
        var mainPtInf = NoTrackingDataContext.PtInfs.FirstOrDefault(item => item.HpId == hpId && item.PtId == ptId);
        if (mainPtInf == null)
        {
            return new();
        }

        var ptFamilyPtIdList = NoTrackingDataContext.PtFamilys.Where(item => item.HpId == hpId
                                                                             && item.PtId == ptId
                                                                             && item.IsDeleted != 1)
                                                              .Select(item => item.FamilyPtId)
                                                              .ToList();

        var ptIdList = GetMaybeFamilyListByAddressOrPhone(hpId, mainPtInf);
        ptIdList.AddRange(GetPatientInfByInsurance(hpId, ptId, sinDate));

        ptIdList = ptIdList.Distinct().ToList();

        var ptInfList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                   && ptIdList.Contains(item.PtId)
                                                                   && item.PtId != ptId
                                                                   && item.IsDelete == 0
                                                                   && !ptFamilyPtIdList.Contains(item.PtId))
                                                    .OrderBy(item => item.PtNum)
                                                    .ToList();

        return ptInfList.Select(item => new FamilyModel(
                                            item.PtId,
                                            item.PtNum,
                                            item.Name ?? string.Empty,
                                            item.KanaName ?? string.Empty,
                                            item.Sex,
                                            item.Birthday,
                                            item.IsDead,
                                            sinDate
                         )).ToList();
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
                                    ptFamily.PtId,
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
                                    ptFamilyRekiFilter,
                                    string.Empty
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

    private RaiinInfModel ConvertToRaiinInfModel(RaiinInf raiinInf, List<UserMst> doctorList, List<KaMst> kaMstList, List<PtHokenPattern> hokenPatternList, List<PtHokenInf> hokenInfList)
    {
        var doctor = doctorList.FirstOrDefault(item => item.UserId == raiinInf.TantoId);
        var kaMst = kaMstList.FirstOrDefault(item => item.KaId == raiinInf.KaId);

        string hokenPatternName = string.Empty;
        var hokenPattern = hokenPatternList.FirstOrDefault(item => item.HokenPid == raiinInf.HokenPid);
        if (hokenPattern != null)
        {
            var hokenInf = hokenInfList.FirstOrDefault(item => item.HokenId == hokenPattern.HokenId);
            hokenPatternName = GetHokenName(hokenPattern.HokenKbn, hokenPattern.HokenSbtCd, hokenInf?.Houbetu ?? string.Empty);
        }

        return new RaiinInfModel(
                raiinInf.PtId,
                raiinInf.SinDate,
                raiinInf.RaiinNo,
                raiinInf.KaId,
                kaMst?.KaName ?? string.Empty,
                raiinInf.TantoId,
                doctor?.Sname ?? string.Empty,
                hokenPattern?.HokenPid ?? CommonConstants.InvalidId,
                hokenPatternName
            );
    }

    private string GetHokenName(int hokenKbn, int hokenSbtCd, string houbetu)
    {
        string result = string.Empty;
        switch (hokenKbn)
        {
            case 0:
                switch (houbetu)
                {
                    case "108":
                        result = "自費";
                        break;
                    case "109":
                        result = "自レ";
                        break;
                }
                break;
            case 11:
            case 12:
            case 13:
                result = "労災";
                break;
            case 14:
                result = "自賠";
                break;
            default:
                if (hokenSbtCd >= 100 && hokenSbtCd <= 199)
                {
                    result = "社保";
                }
                else if (hokenSbtCd >= 200 && hokenSbtCd <= 299)
                {
                    result = "国保";
                }
                else if (hokenSbtCd >= 300 && hokenSbtCd <= 399)
                {
                    result = "後期";
                }
                else if (hokenSbtCd >= 400 && hokenSbtCd <= 499)
                {
                    result = "退職";
                }
                else if (hokenSbtCd >= 500 && hokenSbtCd <= 599)
                {
                    result = "公費";
                }
                break;
        }
        return result;
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
                    continue;
                }
            }
            else
            {
                var ptFamilyEntity = listFamilyDB.FirstOrDefault(item => item.FamilyId == familyModel.FamilyId);
                if (ptFamilyEntity == null)
                {
                    continue;
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
                    continue;
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
                    continue;
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

    private List<long> GetMaybeFamilyListByAddressOrPhone(int hpId, PtInf mainPtInf)
    {
        string tel1 = string.Empty;
        string tel2 = string.Empty;
        string homeAddress = string.Empty;
        string fullSizeHomeAddress = string.Empty;
        string halfSizeHomeAddress = string.Empty;

        var systemConfig = NoTrackingDataContext.SystemConfs.FirstOrDefault(item => item.GrpCd == 1002 && item.GrpEdaNo == 0)?.Val ?? 0;
        if (systemConfig == 0)
        {
            return new();
        }
        else if (systemConfig == 1)
        {
            tel1 = mainPtInf.Tel1?.Replace("ー", string.Empty)
                                    .Replace("ｰ", string.Empty)
                                    .Replace("-", string.Empty)
                                    .Replace("　", string.Empty)
                                    .Replace(" ", string.Empty) ?? string.Empty;

            tel2 = mainPtInf.Tel2?.Replace("ー", string.Empty)
                                        .Replace("ｰ", string.Empty)
                                        .Replace("-", string.Empty)
                                        .Replace("　", string.Empty)
                                        .Replace(" ", string.Empty) ?? string.Empty;

            homeAddress = mainPtInf.HomeAddress1 + mainPtInf.HomeAddress2;
            homeAddress = homeAddress?.Replace("　", string.Empty)
                                      .Replace(" ", string.Empty) ?? string.Empty;

            fullSizeHomeAddress = HenkanJ.Instance.ToFullsize(homeAddress);
            halfSizeHomeAddress = HenkanJ.Instance.ToHalfsize(homeAddress);
        }
        else if (systemConfig == 2)
        {
            tel1 = mainPtInf.Tel1?.Replace("ー", string.Empty)
                                    .Replace("ｰ", string.Empty)
                                    .Replace("-", string.Empty)
                                    .Replace("　", string.Empty)
                                    .Replace(" ", string.Empty) ?? string.Empty;

            tel2 = mainPtInf.Tel2?.Replace("ー", string.Empty)
                                        .Replace("ｰ", string.Empty)
                                        .Replace("-", string.Empty)
                                        .Replace("　", string.Empty)
                                        .Replace(" ", string.Empty) ?? string.Empty;
        }

        var ptInfRepos = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId &&
                                                                        item.IsDelete == 0);

        var query = from ptInf in ptInfRepos
                    select new
                    {
                        PtInf = ptInf,
                        ptInf.Tel1,
                        ptInf.Tel2,
                        HomeAddress = ptInf.HomeAddress1 + ptInf.HomeAddress2
                    };

        query = query.Where(item =>
            (!string.IsNullOrEmpty(item.Tel1) &&
             tel1 != string.Empty &&
             item.Tel1.Replace("ー", string.Empty)
                      .Replace("ｰ", string.Empty)
                      .Replace("-", string.Empty)
                      .Replace("　", string.Empty)
                      .Replace(" ", string.Empty) == tel1) ||
            (!string.IsNullOrEmpty(item.Tel2) &&
             tel1 != string.Empty &&
             item.Tel2.Replace("ー", string.Empty)
                      .Replace("ｰ", string.Empty)
                      .Replace("-", string.Empty)
                      .Replace("　", string.Empty)
                      .Replace(" ", string.Empty) == tel1) ||
            (!string.IsNullOrEmpty(item.Tel1) &&
             tel2 != string.Empty &&
             item.Tel1.Replace("ー", string.Empty)
                      .Replace("ｰ", string.Empty)
                      .Replace("-", string.Empty)
                      .Replace("　", string.Empty)
                      .Replace(" ", string.Empty) == tel2) ||
            (!string.IsNullOrEmpty(item.Tel2) &&
             tel2 != string.Empty &&
             item.Tel2.Replace("ー", string.Empty)
                      .Replace("ｰ", string.Empty)
                      .Replace("-", string.Empty)
                      .Replace("　", string.Empty)
                      .Replace(" ", string.Empty) == tel2) ||
            (!string.IsNullOrEmpty(item.HomeAddress) &&
             item.HomeAddress.Replace("　", string.Empty)
                             .Replace(" ", string.Empty) == homeAddress) ||
            (!string.IsNullOrEmpty(item.HomeAddress) &&
            item.HomeAddress.Replace("　", string.Empty)
                            .Replace(" ", string.Empty) == fullSizeHomeAddress) ||
            (!string.IsNullOrEmpty(item.HomeAddress) &&
            item.HomeAddress.Replace("　", string.Empty)
                            .Replace(" ", string.Empty) == halfSizeHomeAddress));

        return query.Select(item => item.PtInf.PtId).ToList();
    }

    private List<long> GetPatientInfByInsurance(int hpId, long ptId, int sinDate)
    {
        int endDate = (sinDate / 100) * 100 + 1;

        var ptHokenInfCollection = NoTrackingDataContext.PtHokenInfs
            .Where(item =>
                item.HpId == hpId &&
                item.PtId == ptId &&
                item.IsDeleted == 0 &&
                item.StartDate <= sinDate &&
                item.EndDate >= sinDate)
            .ToList();

        var predicate = CreateSameHokNoExpression(ptHokenInfCollection);
        if (predicate == null)
        {
            return new();
        }

        var ptHokenInfRepos = NoTrackingDataContext.PtHokenInfs
            .Where(item =>
                item.HpId == hpId &&
                item.IsDeleted == 0 &&
                item.EndDate >= endDate &&
                (item.HokenKbn == 1 || item.HokenKbn == 2));

        var listPtHokenInf = ptHokenInfRepos.Where(predicate)
            .Select(item => new { item.PtId, item.HokensyaNo, item.Kigo, item.Bango })
            .Distinct()
            .ToList();

        var ptIdCollection = listPtHokenInf
            .Select(item => item.PtId)
            .Distinct()
            .ToList();

        var ptIdExpression = CreatePtIdExpression(ptIdCollection);
        if (ptIdExpression == null)
            return new();

        var ptInfCollection =
            NoTrackingDataContext.PtInfs.Where(item =>
                item.HpId == Session.HospitalID && item.PtId != ptId && item.IsDelete == 0);

        ptInfCollection = ptInfCollection.Where(ptIdExpression);

        return ptInfCollection.Select(item => item.PtId).ToList();
    }

    private Expression<Func<PtHokenInf, bool>>? CreateSameHokNoExpression(List<PtHokenInf> ptHokenInfCollection)
    {
        if (ptHokenInfCollection == null || ptHokenInfCollection.Count <= 0) return null;

        var param = Expression.Parameter(typeof(PtHokenInf));
        Expression expression = null;

        foreach (var ptHokenInf in ptHokenInfCollection)
        {
            if (ptHokenInf == null || !(ptHokenInf.HokenKbn == 1 || ptHokenInf.HokenKbn == 2))
                continue;

            if (string.IsNullOrEmpty(ptHokenInf.HokensyaNo) || string.IsNullOrEmpty(ptHokenInf.Bango)) continue;

            var valHokensyaNo = Expression.Constant(ptHokenInf.HokensyaNo);
            var memberHokensyaNo = Expression.Property(param, nameof(PtHokenInf.HokensyaNo));
            Expression expressionHokensyaNo = Expression.Equal(valHokensyaNo, memberHokensyaNo);

            var valKigo = Expression.Constant(ptHokenInf.Kigo);
            var memberKigo = Expression.Property(param, nameof(PtHokenInf.Kigo));
            Expression expressionKigo = Expression.Equal(valKigo, memberKigo);

            var valBango = Expression.Constant(ptHokenInf.Bango);
            var memberBango = Expression.Property(param, nameof(PtHokenInf.Bango));
            Expression expressionBango = Expression.Equal(valBango, memberBango);

            var expressionAnd = Expression.And(expressionHokensyaNo, expressionKigo);

            expressionAnd = Expression.And(expressionAnd, expressionBango);

            expression = expression == null ? expressionAnd : Expression.Or(expression, expressionAnd);
        }

        return expression != null
            ? Expression.Lambda<Func<PtHokenInf, bool>>(body: expression, parameters: param)
            : null;
    }

    private Expression<Func<PtInf, bool>>? CreatePtIdExpression(List<long> ptIdCollection)
    {
        if (ptIdCollection == null || ptIdCollection.Count <= 0) return null;

        var param = Expression.Parameter(typeof(PtInf));
        Expression expression = null;

        foreach (var ptId in ptIdCollection)
        {
            var valPtId = Expression.Constant(ptId);
            var memberPtId = Expression.Property(param, nameof(PtInf.PtId));
            Expression expressionPtId = Expression.Equal(valPtId, memberPtId);

            expression = expression == null ? expressionPtId : Expression.Or(expression, expressionPtId);
        }

        return expression != null
            ? Expression.Lambda<Func<PtInf, bool>>(body: expression, parameters: param)
            : null;
    }
    #endregion

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
