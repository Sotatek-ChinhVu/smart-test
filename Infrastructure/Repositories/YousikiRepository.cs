﻿using Domain.Models.Yousiki;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class YousikiRepository : RepositoryBase, IYousikiRepository
{
    public YousikiRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<Yousiki1InfModel> GetHistoryYousiki(int hpId, int sinYm, long ptId, int dataType)
    {
        return NoTrackingDataContext.Yousiki1Infs.Where(x => x.HpId == hpId && x.PtId == ptId && x.DataType == dataType && x.IsDeleted == 0 && (x.Status == 1 || x.Status == 2) && x.SinYm < sinYm)
                                                 .OrderByDescending(x => x.SinYm)
                                                 .AsEnumerable()
                                                 .Select(x => new Yousiki1InfModel(
                                                        x.PtId,
                                                        x.SinYm,
                                                        x.DataType,
                                                        x.SeqNo,
                                                        x.IsDeleted,
                                                        x.Status))
                                                 .ToList();
    }

    public List<Yousiki1InfModel> GetYousiki1InfModel(int hpId, int sinYm, long ptNumber, int dataType)
    {
        var ptInfs = NoTrackingDataContext.PtInfs.Where(x => x.HpId == hpId &&
                                x.IsDelete == 0 &&
                                (ptNumber == 0 || x.PtNum == ptNumber));
        var yousiki1Infs = NoTrackingDataContext.Yousiki1Infs.Where(x => x.HpId == hpId &&
                            (dataType == 0 || x.DataType == dataType) &&
                            x.IsDeleted == 0 &&
                            x.SinYm == sinYm);
        var query = from yousikiInf in yousiki1Infs
                    join ptInf in ptInfs on
                    yousikiInf.PtId equals ptInf.PtId
                    select new
                    {
                        yousikiInf,
                        ptInf
                    };
        return query.AsEnumerable()
                    .Select(x => new Yousiki1InfModel(
                            x.yousikiInf.PtId,
                            x.yousikiInf.SinYm,
                            x.yousikiInf.DataType,
                            x.yousikiInf.SeqNo,
                            x.yousikiInf.IsDeleted,
                            x.yousikiInf.Status,
                            x.ptInf.PtNum,
                            x.ptInf.Name ?? string.Empty))
                    .ToList();
    }

    /// <summary>
    /// Get Yousiki1Inf List, default param when query all is status = -1
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="sinYm"></param>
    /// <param name="ptNum"></param>
    /// <param name="dataType"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public List<Yousiki1InfModel> GetYousiki1InfModelWithCommonInf(int hpId, int sinYm, long ptNum, int dataType, int status = -1)
    {
        List<Yousiki1InfModel> compoundedResultList = new();
        var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                && item.IsDelete == 0
                                                                && (ptNum == 0 || item.PtNum == ptNum));
        var yousiki1Infs = NoTrackingDataContext.Yousiki1Infs.Where(item => item.HpId == hpId
                                                                            && item.IsDeleted == 0
                                                                            && (sinYm == 0 || item.SinYm == sinYm));
        var yousiki1InfResultList = (from yousikiInf in yousiki1Infs
                                     join ptInf in ptInfs on
                                     yousikiInf.PtId equals ptInf.PtId
                                     select new Yousiki1InfModel(
                                                ptInf.PtNum,
                                                ptInf.Name ?? string.Empty,
                                                yousikiInf.PtId,
                                                yousikiInf.SinYm,
                                                yousikiInf.DataType,
                                                yousikiInf.Status,
                                                new(),
                                                yousikiInf.SeqNo,
                                                new()))
                                     .ToList();

        var groups = yousiki1InfResultList.GroupBy(x => new { x.PtId, x.SinYm }).ToList();
        foreach (var group in groups)
        {
            var orderGroup = group.OrderBy(x => x.DataType).ToList();
            var yousiki = orderGroup.FirstOrDefault(x => (dataType == 0 || x.DataType == dataType) && (status == -1 || x.Status == status));
            if (yousiki == null)
            {
                continue;
            }

            Dictionary<int, int> statusDic = orderGroup.ToDictionary(x => x.DataType, x => x.Status);
            yousiki.ChangeStatusDic(statusDic);
            compoundedResultList.Add(yousiki);
        }

        return compoundedResultList;
    }

    /// <summary>
    /// Get Yousiki1InfDetail list
    /// </summary>
    /// <param name="sinYm"></param>
    /// <param name="ptId"></param>
    /// <param name="dataType"></param>
    /// <param name="seqNo"></param>
    /// <returns></returns>
    public List<Yousiki1InfDetailModel> GetYousiki1InfDetails(int hpId, int sinYm, long ptId, int dataType, int seqNo)
    {
        var result = NoTrackingDataContext.Yousiki1InfDetails.Where(item => item.SinYm == sinYm
                                                                            && item.PtId == ptId
                                                                            && item.DataType == dataType
                                                                            && item.SeqNo == seqNo
                                                                            && item.HpId == hpId)
                                                             .Select(item => new Yousiki1InfDetailModel(
                                                                                 item.PtId,
                                                                                 item.SinYm,
                                                                                 item.DataType,
                                                                                 item.SeqNo,
                                                                                 item.CodeNo ?? string.Empty,
                                                                                 item.RowNo,
                                                                                 item.Payload,
                                                                                 item.Value ?? string.Empty))
                                                             .ToList();
        return result;
    }

    /// <summary>
    /// Get Yousiki1InfDetail list
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="sinYm"></param>
    /// <param name="ptId"></param>
    /// <param name="ptIdList"></param>
    /// <returns></returns>
    public List<Yousiki1InfDetailModel> GetYousiki1InfDetails(int hpId, int sinYm, List<long> ptIdList)
    {
        var result = NoTrackingDataContext.Yousiki1InfDetails.Where(item => item.SinYm == sinYm
                                                                            && ptIdList.Contains(item.PtId)
                                                                            && item.HpId == hpId)
                                                             .Select(item => new Yousiki1InfDetailModel(
                                                                                 item.PtId,
                                                                                 item.SinYm,
                                                                                 item.DataType,
                                                                                 item.SeqNo,
                                                                                 item.CodeNo ?? string.Empty,
                                                                                 item.RowNo,
                                                                                 item.Payload,
                                                                                 item.Value ?? string.Empty))
                                                             .ToList();
        return result;
    }

    /// <summary>
    /// Get VisitingInf in month
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="sinYm"></param>
    /// <returns></returns>
    public List<VisitingInfModel> GetVisitingInfs(int hpId, long ptId, int sinYm)
    {
        int startDate = sinYm * 100 + 01;
        int endDate = sinYm * 100 + 31;
        var raiinInfsInMonth = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                             && item.PtId == ptId
                                                                             && item.IsDeleted == DeleteTypes.None
                                                                             && item.Status >= RaiinState.TempSave
                                                                             && item.SinDate >= startDate
                                                                             && item.SinDate <= endDate);
        var ptInfRespo = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId && item.IsDelete == 0);
        var userMstRespo = NoTrackingDataContext.UserMsts.Where(item => item.HpId == hpId && item.IsDeleted == 0);
        var kaMstRespo = NoTrackingDataContext.KaMsts.Where(item => item.HpId == hpId && item.IsDeleted == 0);
        var uketukesbtMstRespo = NoTrackingDataContext.UketukeSbtMsts.Where(uketuke => uketuke.HpId == hpId && uketuke.IsDeleted == 0);
        var ptCmtInfRespo = NoTrackingDataContext.PtCmtInfs.Where(item => item.Id == hpId && item.IsDeleted == DeleteTypes.None);
        var result = (from raiinInf in raiinInfsInMonth
                      join ptInf in ptInfRespo on
                           new { raiinInf.HpId, raiinInf.PtId } equals
                           new { ptInf.HpId, ptInf.PtId }
                      join userMst in userMstRespo on
                           new { raiinInf.HpId, raiinInf.TantoId } equals
                           new { userMst.HpId, TantoId = userMst.UserId } into userMstList
                      from user in userMstList.DefaultIfEmpty()
                      join kaMst in kaMstRespo on
                           new { raiinInf.HpId, raiinInf.KaId } equals
                           new { kaMst.HpId, kaMst.KaId } into kaMstList
                      from ka in kaMstList.DefaultIfEmpty()
                      join uketukesbtMst in uketukesbtMstRespo on
                           new { raiinInf.HpId, KbnId = raiinInf.UketukeSbt } equals
                           new { uketukesbtMst.HpId, uketukesbtMst.KbnId } into uketukesbtMstList
                      from uketukesbt in uketukesbtMstList.DefaultIfEmpty()
                      join ptCmtInf in ptCmtInfRespo on
                           new { raiinInf.HpId, raiinInf.PtId } equals
                           new { ptCmtInf.HpId, ptCmtInf.PtId } into ptCmtInfList
                      from ptCmt in ptCmtInfList.DefaultIfEmpty()
                      select new VisitingInfModel
                      (
                          raiinInf.SinDate,
                          raiinInf.UketukeTime ?? string.Empty,
                          raiinInf.RaiinNo,
                          raiinInf.Status,
                          ka.KaName ?? string.Empty,
                          user.Sname ?? string.Empty,
                          raiinInf.SyosaisinKbn,
                          uketukesbt.KbnName ?? string.Empty,
                          ptCmt.Text ?? string.Empty,
                          new()
                      ))
                      .ToList();

        result = result.OrderBy(x => x.SinDate)
                       .ThenBy(x => x.UketukeTime)
                       .ThenBy(x => x.RaiinNo)
                       .ToList();

        var sinDateList = result.Select(item => item.SinDate).Distinct().ToList();
        var raiinListInfQuery = NoTrackingDataContext.RaiinListInfs.Where(item => item.HpId == hpId && item.PtId == ptId && sinDateList.Contains(item.SinDate));
        var raiinListDetailQuery = NoTrackingDataContext.RaiinListDetails.Where(item => item.HpId == hpId && item.IsDeleted == 0);

        var raiinListInfs = (from raiinListInf in raiinListInfQuery
                             join raiinListDetail in raiinListDetailQuery on
                             new { raiinListInf.GrpId, raiinListInf.KbnCd } equals new { raiinListDetail.GrpId, raiinListDetail.KbnCd }
                             select new
                             {
                                 RaiinListInf = raiinListInf,
                                 RaiinListDetail = raiinListDetail,
                             })
                             .ToList();

        foreach (var model in result)
        {
            List<RaiinListInfModel> raiinList = new();
            var raiinListInf = raiinListInfs.Where(item => item.RaiinListInf.PtId == ptId
                                                           && item.RaiinListInf.SinDate == model.SinDate
                                                           && ((model.Status != RaiinState.Reservation ? item.RaiinListInf.RaiinNo == model.RaiinNo : item.RaiinListInf.RaiinNo == 0)
                                                                || (item.RaiinListInf.RaiinNo == 0 && item.RaiinListInf.RaiinListKbn == RaiinListKbnConstants.FILE_KBN)))
                                            .OrderBy(item => item.RaiinListDetail.SortNo)
                                            .Select(item => item.RaiinListInf)
                                            .ToList();

            foreach (var item in raiinListInf)
            {
                if (raiinList.Any(r => r.GrpId == item.GrpId))
                {
                    continue;
                }
                var isContainsFile = raiinListInf.Any(x => x.GrpId == item.GrpId && x.KbnCd == item.KbnCd && item.RaiinListKbn == RaiinListKbnConstants.FILE_KBN);
                var raiinListInfModel = new RaiinListInfModel(
                                            item.PtId,
                                            item.SinDate,
                                            item.RaiinNo,
                                            item.GrpId,
                                            item.KbnCd,
                                            isContainsFile);
                raiinList.Add(raiinListInfModel);
            }

            model.UpdateRaiinListInfList(raiinList);
        }
        return result;
    }

    /// <summary>
    /// check exist Yousiki
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="sinYm"></param>
    /// <param name="ptId"></param>
    /// <returns></returns>
    public bool IsYousikiExist(int hpId, int sinYm, long ptId)
    {
        var yousiki1InfExist = NoTrackingDataContext.Yousiki1Infs.Any(item => item.HpId == hpId
                                                                              && item.IsDeleted == 0
                                                                              && item.SinYm == sinYm
                                                                              && item.PtId == ptId);
        return yousiki1InfExist;
    }

    /// <summary>
    /// check exist Yousiki
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="sinYm"></param>
    /// <param name="ptId"></param>
    /// <param name="dataType"></param>
    /// <returns></returns>
    public bool IsYousikiExist(int hpId, int sinYm, long ptId, int dataType)
    {
        var yousiki1InfExist = NoTrackingDataContext.Yousiki1Infs.Any(item => item.HpId == hpId
                                                                              && item.IsDeleted == 0
                                                                              && item.SinYm == sinYm
                                                                              && item.PtId == ptId
                                                                              && item.DataType == dataType);
        return yousiki1InfExist;
    }

    /// <summary>
    /// Get List PtIdHealthInsuranceAccepted
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="sinYm"></param>
    /// <param name="ptId"></param>
    /// <param name="dataType"></param>
    /// <returns></returns>
    public List<long> GetListPtIdHealthInsuranceAccepted(int hpId, int sinYm, long ptId, int dataType)
    {
        if (sinYm <= 0 || dataType < 1 || dataType > 3)
        {
            return new();
        }
        int startDate = sinYm * 100 + 01;
        int endDate = sinYm * 100 + 31;

        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(item => item.HpId == hpId
                                                                      && (ptId == 0 || (ptId > 0 && item.PtId == ptId))
                                                                      && item.SinDate >= startDate
                                                                      && item.SinDate <= endDate
                                                                      && item.IsDeleted == DeleteTypes.None);
        var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                && item.IsDelete == DeleteTypes.None
                                                                && (ptId == 0 || (ptId > 0 && item.PtId == ptId))
                                                                && item.IsTester == 0);
        var ptHokenInfs = NoTrackingDataContext.PtHokenInfs.Where(item => item.HpId == hpId
                                                                          && (ptId == 0 || (ptId > 0 && item.PtId == ptId))
                                                                          && (item.HokenKbn == 1 || item.HokenKbn == 2)
                                                                          && item.IsDeleted == 0);
        var ptHokenPatterns = NoTrackingDataContext.PtHokenPatterns.Where(item => item.HpId == hpId
                                                                                  && (ptId == 0 || (ptId > 0 && item.PtId == ptId))
                                                                                  && item.IsDeleted == 0);
        var hokenInfs = from ptHokenPattern in ptHokenPatterns
                        join pthoken in ptHokenInfs on new { ptHokenPattern.HpId, ptHokenPattern.PtId, ptHokenPattern.HokenId } equals new { pthoken.HpId, pthoken.PtId, pthoken.HokenId }
                        join ptinf in ptInfs on ptHokenPattern.PtId equals ptinf.PtId
                        select new
                        {
                            PtHokenPattern = ptHokenPattern
                        };

        var hokenQuery = from raiinInf in raiinInfs
                         join hokenInf in hokenInfs on new { raiinInf.HpId, raiinInf.PtId, raiinInf.HokenPid } equals new { hokenInf.PtHokenPattern.HpId, hokenInf.PtHokenPattern.PtId, hokenInf.PtHokenPattern.HokenPid }
                         select new
                         {
                             raiinInf
                         };
        var odrInfs = NoTrackingDataContext.OdrInfs.Where(item => item.HpId == hpId
                                                                  && item.IsDeleted == 0
                                                                  && (ptId == 0 || (ptId > 0 && item.PtId == ptId))
                                                                  && item.SinDate >= startDate
                                                                  && item.SinDate <= endDate
                                                                  && item.SanteiKbn == 0);
        var ordInfQuery = from raiinInf in hokenQuery
                          join odrInf in odrInfs
                          on new { raiinInf.raiinInf.HpId, raiinInf.raiinInf.PtId, raiinInf.raiinInf.RaiinNo, raiinInf.raiinInf.SinDate, raiinInf.raiinInf.HokenPid } equals new { odrInf.HpId, odrInf.PtId, odrInf.RaiinNo, odrInf.SinDate, odrInf.HokenPid }
                          select new
                          {
                              OdrInf = odrInf
                          };

        var odrInfDetails = NoTrackingDataContext.OdrInfDetails.Where(item => item.HpId == hpId
                                                                              && (ptId == 0 || (ptId > 0 && item.PtId == ptId))
                                                                              && item.SinDate >= startDate
                                                                              && item.SinDate <= endDate
                                                                              && !(item.ItemCd == "@SHIN" && item.Suryo == 5));
        var query = from ordInf in ordInfQuery
                    join ordDetail in odrInfDetails
                    on new { ordInf.OdrInf.PtId, ordInf.OdrInf.RaiinNo, ordInf.OdrInf.SinDate, ordInf.OdrInf.RpNo, ordInf.OdrInf.RpEdaNo } equals
                       new { ordDetail.PtId, ordDetail.RaiinNo, ordDetail.SinDate, ordDetail.RpNo, ordDetail.RpEdaNo }
                    select new
                    {
                        OrdInf = ordInf.OdrInf,
                        ItemCd = ordDetail.ItemCd
                    };
        if (ptId == 0)
        {
            var tenMstQuery = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId
                                                                          && item.MasterSbt == "S"
                                                                          && item.Kokuji2 != "7");
            switch (dataType)
            {
                case 1:
                    tenMstQuery = tenMstQuery.Where(item => item.CdKbn == "B" && item.CdKbnno == 1 && item.CdEdano == 3);
                    break;
                case 2:
                    tenMstQuery = tenMstQuery.Where(item => item.CdKbn == "C"
                                                            && ((item.CdKbnno == 1 && item.CdEdano == 0)
                                                                || (item.CdKbnno == 1 && item.CdEdano == 1)
                                                                || (item.CdKbnno == 2 && item.CdEdano == 0)
                                                                || (item.CdKbnno == 2 && item.CdEdano == 2)
                                                                || (item.CdKbnno == 3 && item.CdEdano == 0)));
                    break;
                case 3:
                    tenMstQuery = tenMstQuery.Where(item => item.CdKbn == "H"
                                                            && ((item.CdKbnno == 0 && item.CdEdano == 0)
                                                                || (item.CdKbnno == 1 && item.CdEdano == 0)
                                                                || (item.CdKbnno == 1 && item.CdEdano == 2)
                                                                || (item.CdKbnno == 2 && item.CdEdano == 0)
                                                                || (item.CdKbnno == 3 && item.CdEdano == 0)));
                    break;
            }
            query = from ordInf in query
                    join tenMst in tenMstQuery
                    on ordInf.ItemCd equals tenMst.ItemCd
                    select new
                    {
                        OrdInf = ordInf.OrdInf,
                        ItemCd = tenMst.ItemCd
                    };
        }

        return query.AsEnumerable().Select(x => x.OrdInf?.PtId ?? 0).GroupBy(x => x).Select(x => x.First()).ToList();
    }

    /// <summary>
    /// Add YousikiInf by month
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="userId"></param>
    /// <param name="sinYm"></param>
    /// <param name="dataType"></param>
    /// <param name="ptIdList"></param>
    /// <returns></returns>
    public bool AddYousikiInfByMonth(int hpId, int userId, int sinYm, int dataType, List<long> ptIdList)
    {
        var allYousiki1InfByPtIdList = NoTrackingDataContext.Yousiki1Infs.Where(item => item.HpId == hpId
                                                                                        && ptIdList.Contains(item.PtId)
                                                                                        && item.SinYm == sinYm)
                                                                         .ToList();
        foreach (var ptId in ptIdList)
        {
            if (dataType != 0)
            {
                // check exist common Yousiki, if does not exist then add new Yousiki with DataType = 0
                var existCommonInf = allYousiki1InfByPtIdList.Any(item => item.PtId == ptId
                                                                          && item.DataType == 0
                                                                          && item.IsDeleted == 0);
                if (!existCommonInf)
                {
                    int maxCommonSeqNo = allYousiki1InfByPtIdList.Where(item => item.PtId == ptId
                                                                                && item.DataType == 0)
                                                                 .Select(item => item.SeqNo)
                                                                 .OrderByDescending(item => item)
                                                                 .FirstOrDefault();

                    var newCommonInf = new Yousiki1Inf()
                    {
                        HpId = hpId,
                        SinYm = sinYm,
                        PtId = ptId,
                        DataType = 0,
                        SeqNo = maxCommonSeqNo + 1,
                        CreateId = userId,
                        CreateDate = CIUtil.GetJapanDateTimeNow(),
                        UpdateId = userId,
                        UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    };
                    TrackingDataContext.Yousiki1Infs.Add(newCommonInf);
                }
            }
            var existYousiki1Inf = allYousiki1InfByPtIdList.Any(item => item.PtId == ptId
                                                                        && item.DataType == dataType
                                                                        && item.IsDeleted == 0);
            if (!existYousiki1Inf)
            {
                int maxSeqNo = allYousiki1InfByPtIdList.Where(item => item.PtId == ptId
                                                                      && item.DataType == dataType)
                                                       .Select(item => item.SeqNo)
                                                       .OrderByDescending(item => item)
                                                       .FirstOrDefault();

                var newYousiki = new Yousiki1Inf()
                {
                    HpId = hpId,
                    SinYm = sinYm,
                    PtId = ptId,
                    DataType = dataType,
                    SeqNo = maxSeqNo + 1,
                    CreateId = userId,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                };
                TrackingDataContext.Yousiki1Infs.Add(newYousiki);
            }
        }
        TrackingDataContext.SaveChanges();
        return true;
    }

    public Dictionary<string, string> GetKacodeYousikiMstDict(int hpId)
    {
        var listKacodeMst = NoTrackingDataContext.KacodeYousikiMsts.Where(x => x.HpId == hpId).ToList();
        if (listKacodeMst.Count == 0)
        {
            return new Dictionary<string, string>();
        }

        return listKacodeMst.OrderBy(u => u.SortNo)
                            .ThenBy(u => u.YousikiKaCd)
                            .ToDictionary(kaMst => kaMst.YousikiKaCd.PadLeft(3, '0'), kaMst => kaMst.KaName);
    }

    /// <summary>
    /// Delete YousikiInf
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="userId"></param>
    /// <param name="sinYm"></param>
    /// <param name="ptId"></param>
    /// <param name="dataType"></param>
    /// <returns></returns>
    public bool DeleteYousikiInf(int hpId, int userId, int sinYm, long ptId, int dataType)
    {
        var yousikiInf = TrackingDataContext.Yousiki1Infs.FirstOrDefault(item => item.SinYm == sinYm
                                                                                 && item.PtId == ptId
                                                                                 && item.DataType == dataType
                                                                                 && item.HpId == hpId
                                                                                 && item.IsDeleted == 0);
        if (yousikiInf != null)
        {
            yousikiInf.IsDeleted = 1;
            yousikiInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
            yousikiInf.UpdateId = userId;
        }
        return true;
    }

    public void UpdateYosiki(int hpId, int userId, List<Yousiki1InfDetailModel> yousiki1InfDetailModels, Yousiki1InfModel yousiki1InfModel, Dictionary<int, int> dataTypes, bool isTemporarySave)
    {
        var executionStrategy = TrackingDataContext.Database.CreateExecutionStrategy();
        executionStrategy.Execute(
            () =>
            {
                using var transaction = TrackingDataContext.Database.BeginTransaction();
                try
                {
                    UpdateDateTimeYousikiInf(hpId, userId, yousiki1InfModel.SinYm, yousiki1InfModel.PtId, 0, isTemporarySave ? 1 : 2);
                    foreach (var dataType in dataTypes)
                    {
                        UpdateDateTimeYousikiInf(hpId, userId, yousiki1InfModel.SinYm, yousiki1InfModel.PtId, dataType.Key, isTemporarySave ? 1 : 2);
                        DeleteYousikiInf(hpId, userId, yousiki1InfModel.SinYm, yousiki1InfModel.PtId, dataType.Key);
                    }

                    foreach (var yousiki1InfDetailModel in yousiki1InfDetailModels)
                    {
                        if (yousiki1InfDetailModel.IsDeleted == 1)
                        {
                            var yousiki1InfDetail = TrackingDataContext.Yousiki1InfDetails.Where(x => x.HpId == hpId && x.PtId == yousiki1InfDetailModel.PtId && x.SinYm == yousiki1InfDetailModel.SinYm && x.DataType == yousiki1InfDetailModel.DataType && x.SeqNo == yousiki1InfDetailModel.SeqNo && x.CodeNo == yousiki1InfDetailModel.CodeNo && x.RowNo == yousiki1InfDetailModel.RowNo && x.Payload == yousiki1InfDetailModel.Payload).First();
                            if (yousiki1InfDetail != null)
                            {
                                TrackingDataContext.Remove(yousiki1InfDetail);
                            }
                        }
                        else
                        {
                            var yousiki1InfDetail = TrackingDataContext.Yousiki1InfDetails.Where(x => x.HpId == hpId && x.PtId == yousiki1InfDetailModel.PtId && x.SinYm == yousiki1InfDetailModel.SinYm && x.DataType == yousiki1InfDetailModel.DataType && x.SeqNo == yousiki1InfDetailModel.SeqNo && x.CodeNo == yousiki1InfDetailModel.CodeNo && x.RowNo == yousiki1InfDetailModel.RowNo && x.Payload == yousiki1InfDetailModel.Payload).First();
                            if (yousiki1InfDetail != null)
                            {
                                yousiki1InfDetail.Value = yousiki1InfDetailModel.Value;
                            }
                            else
                            {
                                var yousiki1InfDetailNew = ConvertToModel(yousiki1InfDetailModel);
                                TrackingDataContext.Yousiki1InfDetails.Add(yousiki1InfDetailNew);
                            }
                        }
                    }

                    TrackingDataContext.SaveChanges();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            });

    }

    private Yousiki1InfDetail ConvertToModel(Yousiki1InfDetailModel yousiki1InfDetailModel)
    {
        return new Yousiki1InfDetail()
        {
            PtId = yousiki1InfDetailModel.PtId,
            SinYm = yousiki1InfDetailModel.SinYm,
            DataType = yousiki1InfDetailModel.DataType,
            SeqNo = yousiki1InfDetailModel.SeqNo,
            CodeNo = yousiki1InfDetailModel.CodeNo,
            RowNo = yousiki1InfDetailModel.RowNo,
            Payload = yousiki1InfDetailModel.Payload,
            Value = yousiki1InfDetailModel.Value
        };
    }

    public bool UpdateDateTimeYousikiInf(int hpId, int userId, int sinYm, long ptId, int dataType, int status)
    {
        var yousikiInf = TrackingDataContext.Yousiki1Infs.Where(x => x.SinYm == sinYm &&
                                                                                x.PtId == ptId &&
                                                                                x.DataType == dataType &&
                                                                                x.HpId == hpId &&
                                                                                x.IsDeleted == 0).FirstOrDefault();
        if (yousikiInf != null)
        {
            yousikiInf.Status = status;
            yousikiInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
            yousikiInf.UpdateId = userId;
        }

        return true;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
