using Domain.Models.Yousiki;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Infrastructure.Repositories;

public class YousikiRepository : RepositoryBase, IYousikiRepository
{
    public YousikiRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    /// <summary>
    /// Get Yousiki1Inf List, default param when query all is status = -1
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="sinYm"></param>
    /// <param name="ptNum"></param>
    /// <param name="dataTypes"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public List<Yousiki1InfModel> GetYousiki1InfModelWithCommonInf(int hpId, int sinYm, long ptNum, int dataTypes, int status = -1)
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
            var yousiki = orderGroup.FirstOrDefault(x => (dataTypes == 0 || x.DataType == dataTypes) && (status == -1 || x.Status == status));
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
    /// Get VisitingInf in month
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="ptId"></param>
    /// <param name="sinYm"></param>
    /// <returns></returns>
    public List<VisitingInfModel> GetVisitingInfs(int hpId, long ptId, int sinYm)
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
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
        //var raiinListInfQuery = NoTrackingDataContext.RaiinListInfs.Where(item => item.HpId == hpId && item.PtId == ptId);
        //var raiinListDetailQuery = NoTrackingDataContext.RaiinListDetails.Where(item => item.HpId == hpId && item.IsDeleted == 0);

        //var raiinListInfs = (from raiinListInf in raiinListInfQuery
        //                     join raiinListDetail in raiinListDetailQuery on
        //                     new { raiinListInf.GrpId, raiinListInf.KbnCd } equals new { raiinListDetail.GrpId, raiinListDetail.KbnCd }
        //                     select new
        //                     {
        //                         RaiinListInf = raiinListInf,
        //                         RaiinListDetail = raiinListDetail,
        //                     })
        //                     .ToList();
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

        stopWatch.Stop();
        // Get the elapsed time as a TimeSpan value.
        TimeSpan ts = stopWatch.Elapsed;

        // Format and display the TimeSpan value.
        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds);
        Console.WriteLine("RunTime " + elapsedTime);
        return result;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
