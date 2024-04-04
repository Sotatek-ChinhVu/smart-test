using Domain.Models.PatientInfor;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.Yousiki;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class YousikiRepository : RepositoryBase, IYousikiRepository
{
    private const string CodeNo_Attributes = "CPP0001";
    private const string CodeNo_HeightAndWeight = "CPF0001";
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IPatientInfoRepository _patientInfoRepository;

    public YousikiRepository(IPatientInforRepository patientInforRepository, IPatientInfoRepository patientInfoRepository, ITenantProvider tenantProvider) : base(tenantProvider)
    {
        _patientInforRepository = patientInforRepository;
        _patientInfoRepository = patientInfoRepository;
    }

    public List<Yousiki1InfModel> GetHistoryYousiki(int hpId, int sinYm, long ptId, int dataType)
    {
        return NoTrackingDataContext.Yousiki1Infs.Where(x => x.HpId == hpId && x.PtId == ptId && x.DataType == dataType && x.IsDeleted == 0 && (x.Status == 1 || x.Status == 2) && x.SinYm < sinYm)
                                                 .OrderByDescending(x => x.SinYm)
                                                 .AsEnumerable()
                                                 .Select(x => new Yousiki1InfModel(
                                                        hpId,
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
                            hpId,
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
                                                hpId,
                                                ptInf.PtNum,
                                                ptInf.Name ?? string.Empty,
                                                ptInf.IsTester == 1,
                                                yousikiInf.PtId,
                                                yousikiInf.SinYm,
                                                yousikiInf.DataType,
                                                yousikiInf.Status,
                                                new(),
                                                yousikiInf.SeqNo,
                                                new()))
                                     .ToList();

        var ptIdList = yousiki1InfResultList.Select(item => item.PtId).Distinct().ToList();
        var allYousiki1InfList = NoTrackingDataContext.Yousiki1Infs.Where(item => item.HpId == hpId
                                                                                  && (sinYm == 0 || item.SinYm == sinYm)
                                                                                  && ptIdList.Contains(item.PtId))
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

            Dictionary<int, int> statusDic = new();

            foreach (var item in orderGroup)
            {
                if (!statusDic.ContainsKey(item.DataType))
                {
                    statusDic.Add(item.DataType, item.Status);
                }
            }

            Dictionary<int, int> dataTypeSeqNoDic = new();
            for (int i = 0; i <= 3; i++)
            {
                var seqNo = allYousiki1InfList.Where(item => item.DataType == i
                                                             && (status == -1 || item.Status == status)
                                                             && item.PtId == yousiki.PtId
                                                             && item.SinYm == yousiki.SinYm)
                                              .OrderByDescending(item => item.SeqNo)
                                              .FirstOrDefault()?.SeqNo ?? 0;
                dataTypeSeqNoDic.Add(i, seqNo);
            }

            yousiki.ChangeStatusDic(statusDic, dataTypeSeqNoDic);
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
    public Yousiki1InfModel GetYousiki1InfDetails(int hpId, int sinYm, long ptId, int dataType, int seqNo)
    {
        var yousiki1Inf = NoTrackingDataContext.Yousiki1Infs.FirstOrDefault(item => item.SinYm == sinYm
                                                                                    && item.PtId == ptId
                                                                                    && item.DataType == dataType
                                                                                    && item.SeqNo == seqNo
                                                                                    && item.HpId == hpId
                                                                                    && item.IsDeleted == 0);
        var ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(item => item.HpId == hpId && item.PtId == ptId && item.IsDelete == 0);
        if (yousiki1Inf == null || ptInf == null)
        {
            return new();
        }
        var yousiki1InfDetailList = NoTrackingDataContext.Yousiki1InfDetails.Where(item => item.SinYm == sinYm
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
        GetDefaultYousiki1InfDetailList(hpId, dataType, yousiki1Inf, ref yousiki1InfDetailList);
        return new Yousiki1InfModel(hpId,
                                    ptInf.PtNum,
                                    ptInf.Name ?? string.Empty,
                                    ptInf.IsTester == 1,
                                    yousiki1Inf.PtId,
                                    yousiki1Inf.SinYm,
                                    yousiki1Inf.DataType,
                                    yousiki1Inf.Status,
                                    new(),
                                    yousiki1Inf.SeqNo,
                                    yousiki1InfDetailList.Select(itemDetail => new Yousiki1InfDetailModel(
                                                                                        itemDetail.PtId,
                                                                                        itemDetail.SinYm,
                                                                                        itemDetail.DataType,
                                                                                        itemDetail.SeqNo,
                                                                                        itemDetail.CodeNo ?? string.Empty,
                                                                                        itemDetail.RowNo,
                                                                                        itemDetail.Payload,
                                                                                        itemDetail.Value ?? string.Empty))
                                                         .ToList());
    }

    /// <summary>
    /// Get default Yousiki1InfDetail list
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="dataType"></param>
    /// <param name="yousiki1Inf"></param>
    /// <param name="yousiki1InfDetailList"></param>
    private void GetDefaultYousiki1InfDetailList(int hpId, int dataType, Yousiki1Inf yousiki1Inf, ref List<Yousiki1InfDetailModel> yousiki1InfDetailList)
    {
        #region Common
        #region 属性
        bool isExistAttributes = yousiki1InfDetailList.Any(item => item.DataType == dataType && item.CodeNo == CodeNo_Attributes);
        if (!isExistAttributes)
        {
            var ptInf = _patientInforRepository.GetPtInf(hpId, yousiki1Inf.PtId);

            // BirthDay
            yousiki1InfDetailList.Add(CreateYousiki1InfDetailModel(yousiki1Inf, CodeNo_Attributes, 0, 1, ptInf.Birthday.AsString()));

            //Sex
            yousiki1InfDetailList.Add(CreateYousiki1InfDetailModel(yousiki1Inf, CodeNo_Attributes, 0, 2, ptInf.Sex.AsString()));

            // HomePost
            string homePost = ptInf.HomePost.AsString();
            homePost = homePost.Replace("-", "");
            homePost = homePost.Replace("ー", "");
            homePost = homePost.Replace(" ", "");
            homePost = homePost.Replace("　", "");
            homePost = homePost.PadRight(7, '0');
            yousiki1InfDetailList.Add(CreateYousiki1InfDetailModel(yousiki1Inf, CodeNo_Attributes, 0, 3, homePost));
        }
        #endregion

        #region 身長・体重
        bool isExistHeightAndWeightPtInf = yousiki1InfDetailList.Any(item => item.DataType == dataType && item.CodeNo == CodeNo_HeightAndWeight);
        if (!isExistHeightAndWeightPtInf)
        {
            int sinDate = CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow());
            if (yousiki1Inf.SinYm > 0)
            {
                sinDate = yousiki1Inf.SinYm * 100 + 31;
            }

            string bodyHeight = "000";
            string bodyWeight = "000";
            var ptPhysicalInfoList = _patientInfoRepository.GetPtPhysicalInfoToYousiki(hpId, yousiki1Inf.PtId, sinDate);

            // BodyHeight
            var kensaInfHeight = ptPhysicalInfoList.FirstOrDefault(item => item.KensaItemCd == "V0001");
            if (kensaInfHeight != null && !string.IsNullOrEmpty(kensaInfHeight.ResultVal))
            {
                bodyHeight = Math.Round(kensaInfHeight.ResultVal.AsDouble(), 0, MidpointRounding.AwayFromZero).AsInteger().AsString();
                yousiki1InfDetailList.Add(CreateYousiki1InfDetailModel(yousiki1Inf, CodeNo_HeightAndWeight, 0, 1, bodyHeight));
            }

            // BodyWeight
            var kensaInfWeight = ptPhysicalInfoList.FirstOrDefault(item => item.KensaItemCd == "V0002");
            if (kensaInfWeight != null && !string.IsNullOrEmpty(kensaInfWeight.ResultVal))
            {
                bodyWeight = Math.Round(kensaInfWeight.ResultVal.AsDouble(), 1, MidpointRounding.AwayFromZero).ToString("0.0");
                yousiki1InfDetailList.Add(CreateYousiki1InfDetailModel(yousiki1Inf, CodeNo_HeightAndWeight, 0, 2, bodyWeight));
            }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// CreateYousiki1InfDetailModel
    /// </summary>
    /// <param name="codeNo"></param>
    /// <param name="rowNo"></param>
    /// <param name="payLoad"></param>
    /// <param name="valueDefault"></param>
    /// <returns></returns>
    private Yousiki1InfDetailModel CreateYousiki1InfDetailModel(Yousiki1Inf yousiki1Inf, string codeNo, int rowNo, int payLoad, string value)
    {
        var detail = new Yousiki1InfDetailModel(yousiki1Inf.PtId, yousiki1Inf.SinYm, yousiki1Inf.DataType, yousiki1Inf.SeqNo, codeNo, rowNo, payLoad, value);
        return detail;
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

    public List<Yousiki1InfDetailModel> GetYousiki1InfDetailsByCodeNo(int hpId, int sinYm, long ptId, int dataType, int seqNo, string codeNo)
    {
        var result = NoTrackingDataContext.Yousiki1InfDetails.Where(item => item.SinYm == sinYm
                                                                            && item.PtId == ptId
                                                                            && item.DataType == dataType
                                                                            && item.SeqNo == seqNo
                                                                            && item.HpId == hpId
                                                                            && item.CodeNo == codeNo)
                                                             .OrderBy(x => x.RowNo)
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
    public (List<VisitingInfModel> visitingInfList, Dictionary<int, string> allGrpDictionary) GetVisitingInfs(int hpId, long ptId, int sinYm)
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
        var ptCmtInfRespo = NoTrackingDataContext.PtCmtInfs.Where(item => item.HpId == hpId && item.IsDeleted == DeleteTypes.None);
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
                          ka.YousikiKaCd ?? string.Empty,
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

        var raiinKbnMsts = NoTrackingDataContext.RaiinKbnMsts.Where(x => x.HpId == hpId).ToList();
        Dictionary<int, string> allGrpDictionary = new();
        foreach (var model in result)
        {
            List<RaiinListInfModel> raiinList = new();
            var raiinListInf = raiinListInfs.Where(item => item.RaiinListInf.PtId == ptId
                                                           && item.RaiinListInf.SinDate == model.SinDate
                                                           && ((model.Status != RaiinState.Reservation ? item.RaiinListInf.RaiinNo == model.RaiinNo : item.RaiinListInf.RaiinNo == 0)
                                                                || (item.RaiinListInf.RaiinNo == 0 && item.RaiinListInf.RaiinListKbn == RaiinListKbnConstants.FILE_KBN)))
                                            .OrderBy(item => item.RaiinListDetail.SortNo)
                                            .Select(item => new { item.RaiinListInf, item.RaiinListDetail })
                                            .ToList();

            foreach (var item in raiinListInf)
            {
                var raiinListInfItem = item.RaiinListInf;
                var raiinListInfDetailItem = item.RaiinListDetail;
                if (raiinList.Any(r => r.GrpId == raiinListInfItem.GrpId))
                {
                    continue;
                }
                var isContainsFile = raiinListInf.Select(x => x.RaiinListInf).Any(x => x.GrpId == raiinListInfItem.GrpId && x.KbnCd == raiinListInfItem.KbnCd && raiinListInfItem.RaiinListKbn == RaiinListKbnConstants.FILE_KBN);
                var raiinListInfModel = new RaiinListInfModel(
                                            raiinListInfItem.PtId,
                                            raiinListInfItem.SinDate,
                                            raiinListInfItem.RaiinNo,
                                            raiinListInfItem.GrpId,
                                            raiinKbnMsts.FirstOrDefault(x => x.GrpCd == raiinListInfItem.GrpId)?.GrpName ?? string.Empty,
                                            raiinListInfItem.KbnCd,
                                            raiinListInfDetailItem.KbnName ?? string.Empty,
                                            raiinListInfDetailItem.ColorCd ?? string.Empty,
                                            isContainsFile);
                raiinList.Add(raiinListInfModel);
                if (!allGrpDictionary.ContainsKey(raiinListInfModel.GrpId))
                {
                    allGrpDictionary.Add(raiinListInfModel.GrpId, raiinListInfModel.GrpName);
                }
            }

            model.UpdateRaiinListInfList(raiinList);
        }
        return (result, allGrpDictionary);
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
    /// Get ListYousiki1Inf for export data
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="sinYm"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public List<Yousiki1InfModel> GetListYousiki1Inf(int hpId, int sinYm, int status = -1)
    {
        var yousiki1InfList = NoTrackingDataContext.Yousiki1Infs.Where(item => item.HpId == hpId && item.SinYm == sinYm && item.IsDeleted == 0 && (status == -1 || item.Status == status)).ToList();
        var ptIdList = yousiki1InfList.Select(item => item.PtId).Distinct().ToList();
        var yousiki1InfDetailList = NoTrackingDataContext.Yousiki1InfDetails.Where(item => item.HpId == hpId && item.SinYm == sinYm && ptIdList.Contains(item.PtId)).ToList();
        var ptInfList = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId && item.IsDelete == 0 && ptIdList.Contains(item.PtId)).ToList();

        var yousiki1InfQuery = from yousiki1Inf in yousiki1InfList
                               join ptInf in ptInfList on
                                  new { yousiki1Inf.HpId, yousiki1Inf.PtId } equals
                                  new { ptInf.HpId, ptInf.PtId }
                               join yousiki1InfDetail in yousiki1InfDetailList on
                                 new { yousiki1Inf.HpId, yousiki1Inf.PtId, yousiki1Inf.DataType, yousiki1Inf.SinYm, yousiki1Inf.SeqNo } equals
                                 new { yousiki1InfDetail.HpId, yousiki1InfDetail.PtId, yousiki1InfDetail.DataType, yousiki1InfDetail.SinYm, yousiki1InfDetail.SeqNo } into listYousiki1InfDetail
                               select new
                               {
                                   ptInf,
                                   yousiki1Inf,
                                   ListYousiki1InfDetail = listYousiki1InfDetail
                               };

        return yousiki1InfQuery
               .Select(item => new Yousiki1InfModel(
                                   hpId,
                                   item.ptInf.PtNum,
                                   item.ptInf.Name ?? string.Empty,
                                   item.ptInf.IsTester == 1,
                                   item.yousiki1Inf.PtId,
                                   item.yousiki1Inf.SinYm,
                                   item.yousiki1Inf.DataType,
                                   item.yousiki1Inf.Status,
                                   new(),
                                   item.yousiki1Inf.SeqNo,
                                   item.ListYousiki1InfDetail.Select(itemDetail => new Yousiki1InfDetailModel(
                                                                                       itemDetail.PtId,
                                                                                       itemDetail.SinYm,
                                                                                       itemDetail.DataType,
                                                                                       itemDetail.SeqNo,
                                                                                       itemDetail.CodeNo ?? string.Empty,
                                                                                       itemDetail.RowNo,
                                                                                       itemDetail.Payload,
                                                                                       itemDetail.Value ?? string.Empty))
                                                             .OrderBy(item => item.CodeNo)
                                                             .ThenBy(item => item.RowNo)
                                                             .ThenBy(item => item.Payload)
                                                             .ToList()))
               .ToList();
    }

    /// <summary>
    /// Get RaiinInfs in month
    /// </summary>
    /// <param name="sinYm"></param>
    /// <returns></returns>
    public List<ForeignKFileModel> GetRaiinInfsInMonth(int hpId, int sinYm)
    {
        var ptInfs = NoTrackingDataContext.PtInfs.Where(p => p.HpId == hpId);

        var kaikeiInfs = NoTrackingDataContext.KaikeiInfs.Where(p => p.HpId == hpId);

        var raiinInfs = NoTrackingDataContext.RaiinInfs.Where(p => p.HpId == hpId
                                                                   && p.IsDeleted == DeleteTypes.None
                                                                   && p.SinDate / 100 == sinYm
                                                                   && p.Status >= RaiinState.Calculate);

        var joinkaikeiInfQuery = from raiinInf in raiinInfs
                                 join kaikeiInf in kaikeiInfs
                                 on new { raiinInf.PtId, raiinInf.SinDate, raiinInf.RaiinNo }
                                 equals new { kaikeiInf.PtId, kaikeiInf.SinDate, kaikeiInf.RaiinNo }
                                 select new
                                 {
                                     RaiinInf = raiinInf,
                                     KaikeiInf = kaikeiInf
                                 };
        var joinPtInfQuery = from joinkaikeiInf in joinkaikeiInfQuery
                             join ptInf in ptInfs
                             on joinkaikeiInf.RaiinInf.PtId equals ptInf.PtId
                             select new
                             {
                                 joinkaikeiInf.RaiinInf.SinDate,
                                 joinkaikeiInf.KaikeiInf.HokenKbn,
                                 PtInf = ptInf
                             };
        var result = joinPtInfQuery.Where(item => item.HokenKbn == 1 || item.HokenKbn == 2)
                                   .GroupBy(item => new { item.SinDate, item.PtInf.PtId })
                                   .AsEnumerable()
                                   .Select(item => item.FirstOrDefault())
                                   .OrderBy(item => item?.PtInf.PtNum)
                                   .ThenBy(item => item?.SinDate)
                                   .Select(item => new ForeignKFileModel(
                                                       item?.SinDate ?? 0,
                                                       item?.PtInf.PtNum ?? 0,
                                                       item?.PtInf.KanaName ?? string.Empty,
                                                       item?.PtInf.Sex ?? 0,
                                                       item?.PtInf.Birthday ?? 0,
                                                       item?.PtInf.IsTester == 1))
                                   .ToList();
        return result;
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

        TrackingDataContext.SaveChanges();
        return true;
    }


    /// <summary>
    /// Delete YousikiInf
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="userId"></param>
    /// <param name="sinYm"></param>
    /// <param name="ptId"></param>
    /// <returns></returns>
    public bool DeleteYousikiInf(int hpId, int userId, int sinYm, long ptId)
    {
        var yousikiInfList = TrackingDataContext.Yousiki1Infs.Where(item => item.SinYm == sinYm
                                                                            && item.PtId == ptId
                                                                            && item.HpId == hpId
                                                                            && item.IsDeleted == 0)
                                                             .ToList();
        foreach (var yousikiInf in yousikiInfList)
        {
            yousikiInf.IsDeleted = 1;
            yousikiInf.UpdateDate = CIUtil.GetJapanDateTimeNow();
            yousikiInf.UpdateId = userId;
        }

        TrackingDataContext.SaveChanges();
        return true;
    }

    public void UpdateYosiki(int hpId, int userId, List<Yousiki1InfDetailModel> yousiki1InfDetailModels, Yousiki1InfModel yousiki1InfModel, List<CategoryModel> categoryModels, bool isTemporarySave)
    {
        UpdateDateTimeYousikiInf(hpId, userId, yousiki1InfModel.SinYm, yousiki1InfModel.PtId, 0, isTemporarySave ? 1 : 2);

        foreach (var categoryModel in categoryModels)
        {
            if (categoryModel.IsDeleted == 1)
            {
                DeleteYousikiInf(hpId, userId, yousiki1InfModel.SinYm, yousiki1InfModel.PtId, categoryModel.DataType);
            }

            UpdateDateTimeYousikiInf(hpId, userId, yousiki1InfModel.SinYm, yousiki1InfModel.PtId, categoryModel.DataType, isTemporarySave ? 1 : 2);
        }

        foreach (var yousiki1InfDetailModel in yousiki1InfDetailModels)
        {
            if (yousiki1InfDetailModel.Equals(null))
            {
                continue;
            }

            if (yousiki1InfDetailModel.IsDeleted == 1)
            {
                var yousiki1InfDetail = TrackingDataContext.Yousiki1InfDetails.FirstOrDefault(x => x.HpId == hpId && x.PtId == yousiki1InfDetailModel.PtId && x.SinYm == yousiki1InfDetailModel.SinYm && x.DataType == yousiki1InfDetailModel.DataType && x.SeqNo == yousiki1InfDetailModel.SeqNo && x.CodeNo == yousiki1InfDetailModel.CodeNo && x.RowNo == yousiki1InfDetailModel.RowNo && x.Payload == yousiki1InfDetailModel.Payload);
                if (yousiki1InfDetail != null)
                {
                    TrackingDataContext.Remove(yousiki1InfDetail);
                }
            }
            else
            {
                var yousiki1InfDetail = TrackingDataContext.Yousiki1InfDetails.FirstOrDefault(x => x.HpId == hpId && x.PtId == yousiki1InfDetailModel.PtId && x.SinYm == yousiki1InfDetailModel.SinYm && x.DataType == yousiki1InfDetailModel.DataType && x.SeqNo == yousiki1InfDetailModel.SeqNo && x.CodeNo == yousiki1InfDetailModel.CodeNo && x.RowNo == yousiki1InfDetailModel.RowNo && x.Payload == yousiki1InfDetailModel.Payload);
                if (yousiki1InfDetail != null)
                {
                    yousiki1InfDetail.Value = yousiki1InfDetailModel.Value;
                }
                else
                {
                    var yousiki1InfDetailNew = ConvertToModel(hpId, yousiki1InfDetailModel);
                    TrackingDataContext.Yousiki1InfDetails.Add(yousiki1InfDetailNew);
                }
            }
        }

        TrackingDataContext.SaveChanges();
    }

    private Yousiki1InfDetail ConvertToModel(int hpId, Yousiki1InfDetailModel yousiki1InfDetailModel)
    {
        return new Yousiki1InfDetail()
        {
            HpId = hpId,
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
        var yousikiInf = TrackingDataContext.Yousiki1Infs.FirstOrDefault(x => x.SinYm == sinYm &&
                                                                                x.PtId == ptId &&
                                                                                x.DataType == dataType &&
                                                                                x.HpId == hpId &&
                                                                                x.IsDeleted == 0);
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
        _patientInforRepository.ReleaseResource();
        _patientInfoRepository.ReleaseResource();
    }
}
