using Domain.Constant;
using Domain.Models.SystemConf;
using Entity.Tenant;
using Helper.Common;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using PostgreDataContext;
using Reporting.DrugInfo.Model;

namespace Reporting.DrugInfo.DB;

public class CoDrugInfFinder : RepositoryBase, ICoDrugInfFinder
{
    private readonly ISystemConfRepository _systemConfRepository;
    private readonly TenantDataContext _tenantOdrInfDetail;
    private readonly IConfiguration _configuration;

    public CoDrugInfFinder(ITenantProvider tenantProvider, ITenantProvider tenantOdrInfDetail, ISystemConfRepository systemConfRepository, IConfiguration configuration) : base(tenantProvider)
    {
        _systemConfRepository = systemConfRepository;
        _tenantOdrInfDetail = tenantOdrInfDetail.GetNoTrackingDataContext();
        _configuration = configuration;
    }

    public void ReleaseResource()
    {
        _systemConfRepository.ReleaseResource();
        _tenantOdrInfDetail.Dispose();
        DisposeDataContext();
    }

    public PathConf GetPathConf(int grpCode)
    {
        var pathConfs = NoTrackingDataContext.PathConfs.FirstOrDefault(p => p.GrpCd == grpCode);
        return pathConfs ?? new();
    }

    public DrugInfoModel GetBasicInfo(int hpId, long ptId, int orderDate = 0)
    {
        DrugInfoModel info = new DrugInfoModel();
        var intOrderDate = orderDate == 0 ? CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow()) : orderDate;
        info.OrderDate = CIUtil.SDateToShowWDate2(intOrderDate);

        var hpInfo = NoTrackingDataContext.HpInfs.Where(p => p.HpId == hpId && p.StartDate <= intOrderDate).OrderByDescending(p => p.StartDate).FirstOrDefault();
        if (hpInfo != null)
        {
            info.HpName = hpInfo.HpName ?? string.Empty;
            info.Address1 = hpInfo.Address1 ?? string.Empty;
            info.Address2 = hpInfo.Address2 ?? string.Empty;
            info.Phone = hpInfo.Tel ?? string.Empty;
        }

        var ptInfo = NoTrackingDataContext.PtInfs.FirstOrDefault(pt => pt.HpId == hpId && pt.PtId == ptId);
        if (ptInfo != null)
        {
            info.PtNo = ptInfo.PtNum;
            info.PtName = ptInfo.Name ?? string.Empty;
            info.Sex = ptInfo.Sex == 1 ? "M" : "F";
            info.IntAge = (intOrderDate - ptInfo.Birthday) / 10000;
        }

        return info;
    }

    public List<OrderInfoModel> GetOrderByRaiinNo(long raiinNo)
    {
        // multiple threads to speed up performance
        List<OrderInfoModel> listOrderInfo = new();
        var odrKouiKbnFilter = new[] { 21, 22, 23, 28 };
        List<OdrInfDetail> infDetails = new();
        List<OdrInf> odrInfs = new();

        Task taskListOdrInf = Task.Factory.StartNew(() => odrInfs = NoTrackingDataContext.OdrInfs.Where(item => odrKouiKbnFilter.Contains(item.OdrKouiKbn) && item.InoutKbn == 0 && item.RaiinNo == raiinNo && item.IsDeleted == 0).ToList());
        Task taskListOdrInfDetail = Task.Factory.StartNew(() => infDetails = _tenantOdrInfDetail.OdrInfDetails.Where(item => !(item.ItemCd != null && item.ItemCd.StartsWith("8") && item.ItemCd.Length == 9) && item.RaiinNo == raiinNo).ToList());
        Task.WaitAll(taskListOdrInf, taskListOdrInfDetail);

        if (odrInfs != null && odrInfs.Count > 0)
        {
            foreach (var odrInf in odrInfs)
            {
                var orderInfoModel = new OrderInfoModel();
                orderInfoModel.RaiinNo = odrInf.RaiinNo;
                orderInfoModel.RpNo = odrInf.RpNo;
                orderInfoModel.OdrKouiKbn = odrInf.OdrKouiKbn;

                var details = infDetails.Where(d => d.RaiinNo == odrInf.RaiinNo && d.RpNo == odrInf.RpNo && d.RpEdaNo == odrInf.RpEdaNo).OrderBy(d => d.RowNo);
                List<OrderInfDetailModel> orderInfDetailModels = new();
                if (details != null)
                {
                    foreach (var detail in details)
                    {
                        var orderInfDetailModel = new OrderInfDetailModel();
                        orderInfDetailModel.RaiinNo = detail.RaiinNo;
                        orderInfDetailModel.RpNo = detail.RpNo;
                        orderInfDetailModel.RpEdaNo = detail.RpEdaNo;
                        orderInfDetailModel.RowNo = detail.RowNo;
                        orderInfDetailModel.SinKouiKbn = detail.SinKouiKbn;
                        orderInfDetailModel.ItemCd = detail.ItemCd ?? string.Empty;
                        orderInfDetailModel.ItemName = detail.ItemName ?? string.Empty;
                        orderInfDetailModel.YohoKbn = detail.YohoKbn;
                        orderInfDetailModel.Suryo = detail.Suryo;
                        orderInfDetailModel.UnitName = detail.UnitName ?? string.Empty;
                        orderInfDetailModels.Add(orderInfDetailModel);
                    }
                }
                orderInfoModel.OrderInfDetailCollection = orderInfDetailModels;
                listOrderInfo.Add(orderInfoModel);
            }
        }
        return listOrderInfo;
    }

    public string GetYJCode(string itemCd)
    {
        int sinDate = CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow());
        var tenMst = NoTrackingDataContext.TenMsts.FirstOrDefault(t => t.ItemCd == itemCd && t.StartDate <= sinDate && t.EndDate >= sinDate);
        if (tenMst != null)
        {
            return tenMst.YjCd ?? string.Empty;
        }
        return string.Empty;
    }

    public List<SingleDosageMstModel> GetSingleDosageMstCollection(int hpId, string unitName)
    {
        var singleDosageMsts = NoTrackingDataContext.SingleDoseMsts.Where(s => s.HpId == hpId && s.UnitName == unitName)
            .AsEnumerable().Select(s => new SingleDosageMstModel() { HpId = s.HpId, Id = s.Id, UnitName = s.UnitName ?? string.Empty }).ToList();
        return singleDosageMsts;

    }

    public TenMstModel GetTenMstModel(string itemCd)
    {
        var tenMstModel = new TenMstModel();
        int sinDate = CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow());
        var tenMst = NoTrackingDataContext.TenMsts.FirstOrDefault(t => t.ItemCd == itemCd && t.StartDate <= sinDate && t.EndDate >= sinDate);
        if (tenMst != null)
        {
            tenMstModel.ItemCD = tenMst.ItemCd;
            tenMstModel.Rise = tenMst.FukuyoRise;
            tenMstModel.Morning = tenMst.FukuyoMorning;
            tenMstModel.Evening = tenMst.FukuyoNight;
            tenMstModel.DayTime = tenMst.FukuyoDaytime;
            tenMstModel.Sleep = tenMst.FukuyoSleep;
        }
        return tenMstModel;
    }

    public List<PiImage> GetProductImages(string itemCd)
    {
        var images = NoTrackingDataContext.PiImages.Where(p => p.ItemCd == itemCd).ToList();
        return images;
    }

    /// <summary>
    /// Get common data for function GetDrugInfo to speed up performance
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="itemCdList"></param>
    /// <param name="age"></param>
    /// <param name="gender"></param>
    public (List<DrugInf> drugInfList, List<TenMst> tenMstList, List<M34DrugInfoMain> m34DrugInfoMainList, List<M34IndicationCode> m34IndicationCodeList, List<M34Precaution> m34PrecautionList, List<M34PrecautionCode> m34PrecautionCodeList) GetQueryDrugList(int hpId, List<string> itemCdList, int age, int gender)
    {
        itemCdList = itemCdList.Distinct().ToList();
        string strSex = gender.AsString();
        List<DrugInf> drugInfList = NoTrackingDataContext.DrugInfs.Where(item => item.HpId == hpId
                                                                                 && itemCdList.Contains(item.ItemCd)
                                                                                 && item.IsDeleted == 0)
                                                                 .ToList();
        List<TenMst> tenMstList = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId && itemCdList.Contains(item.ItemCd))
                                                               .Distinct()
                                                               .ToList();

        var yjCdList = tenMstList.Select(item => item.YjCd).Distinct().ToList();
        List<M34DrugInfoMain> m34DrugInfoMainList = NoTrackingDataContext.M34DrugInfoMains.Where(item => yjCdList.Contains(item.YjCd)).Distinct().ToList();

        var konoCodes = m34DrugInfoMainList.Select(item => item.KonoCd).Distinct().ToList();
        List<M34IndicationCode> m34IndicationCodeList = NoTrackingDataContext.M34IndicationCodes.Where(item => konoCodes.Contains(item.KonoCd)).ToList();
        List<M34Precaution> m34PrecautionList = NoTrackingDataContext.M34Precautions.Where(item => yjCdList.Contains(item.YjCd)).ToList();
        List<M34PrecautionCode> m34PrecautionCodeList = NoTrackingDataContext.M34PrecautionCodes.Where(pr => ((pr.AgeMax <= 0 && pr.AgeMin <= 0)
                                                                                                               || (pr.AgeMax >= age && pr.AgeMin <= age)
                                                                                                               || (pr.AgeMax <= 0 && pr.AgeMin <= age))
                                                                                                             && (pr.SexCd == null
                                                                                                                 || pr.SexCd == string.Empty
                                                                                                                 || pr.SexCd == strSex))
                                                                                                .ToList();
        return (drugInfList, tenMstList, m34DrugInfoMainList, m34IndicationCodeList, m34PrecautionList, m34PrecautionCodeList);
    }

    public List<DrugInf> GetDrugInfo(int hpId, string itemCd, int age, int gender, List<DrugInf> drugInfList, List<TenMst> tenMstList, List<M34DrugInfoMain> m34DrugInfoMainList, List<M34IndicationCode> m34IndicationCodeList, List<M34Precaution> m34PrecautionList, List<M34PrecautionCode> m34PrecautionCodeList, List<SystemConfModel> allSystemConfigList)
    {
        List<DrugInf> result = new();
        string strSex = gender.AsString();
        var drugInf = drugInfList.Where(d => d.HpId == hpId && d.ItemCd == itemCd && d.IsDeleted == 0 && d.InfKbn == 1).ToList();
        if (!drugInf.Any())
        {
            drugInf = new List<DrugInf>();
            var tenMsts = tenMstList.Where(t => t.ItemCd == itemCd);
            var drugInfoMains = m34DrugInfoMainList;
            var joinQuery = (from t in tenMsts
                             join dm in drugInfoMains
                             on t.YjCd equals dm.YjCd
                             select dm.KonoCd);

            var konoCodes = joinQuery.GroupBy(g => g).Select(g => g.Key).ToList();

            var indicationCodes = m34IndicationCodeList;
            var joinWithCodes = (from konoCd in konoCodes
                                 join indicationCode in indicationCodes
                                 on konoCd equals indicationCode.KonoCd
                                 select indicationCode).ToList();
            if (joinWithCodes != null)
            {
                foreach (var p in joinWithCodes)
                {
                    DrugInf inf = new DrugInf();
                    inf.ItemCd = itemCd;
                    inf.InfKbn = 1;
                    inf.DrugInfo = p.KonoSimpleCmt;
                    drugInf.Add(inf);
                }
            }
        }
        result.AddRange(drugInf);

        var drugInf1 = drugInfList.Where(d => d.HpId == hpId && d.ItemCd == itemCd && d.IsDeleted == 0 && d.InfKbn == 2).ToList();
        if (drugInf1 == null || drugInf1.Count == 0)
        {
            drugInf1 = new List<DrugInf>();

            var tenMsts = tenMstList.Where(t => t.ItemCd == itemCd);
            var precaution = m34PrecautionList;
            var joinQuery = (from t in tenMsts
                             join pr in precaution
                             on t.YjCd equals pr.YjCd
                             select new { pr.SeqNo, pr.PrecautionCd });

            var precautionCds = joinQuery.GroupBy(g => new { g.PrecautionCd }).Select(g => new { g.Key.PrecautionCd, g.FirstOrDefault()!.SeqNo }).ToList();

            var precautionCodes = m34PrecautionCodeList.Where(pr => ((pr.AgeMax <= 0 && pr.AgeMin <= 0)
                                                                      || (pr.AgeMax >= age && pr.AgeMin <= age)
                                                                      || (pr.AgeMax <= 0 && pr.AgeMin <= age))
                                                                  && (pr.SexCd == null || pr.SexCd == string.Empty || pr.SexCd == strSex));
            var precautionCodeQuery = precautionCodes;
            if (GetSettingValue(allSystemConfigList, 92004, 3) == 1) //IsPrecautionQueryAgeSex
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => !((pr.AgeMin > 0 || pr.AgeMax > 0)
                                                                        && (pr.AgeMax >= age || pr.AgeMax <= 0)
                                                                        && pr.AgeMin <= age
                                                                        && !string.IsNullOrEmpty(pr.SexCd)
                                                                        && pr.SexCd == strSex));
            }
            if (GetSettingValue(allSystemConfigList, 92004, 4) == 1) //IsPrecautionQueryAgeNoSex
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => !((pr.AgeMin > 0 || pr.AgeMax > 0)
                                                                        && (pr.AgeMax >= age || pr.AgeMax <= 0)
                                                                        && pr.AgeMin <= age
                                                                        && string.IsNullOrEmpty(pr.SexCd)));
            }
            if (GetSettingValue(allSystemConfigList, 92004, 5) == 1) //IsPrecautionQuerySexNoAge
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => !((pr.AgeMax <= 0 && pr.AgeMin <= 0)
                                                                        && !string.IsNullOrEmpty(pr.SexCd)
                                                                        && pr.SexCd == strSex));
            }
            if (GetSettingValue(allSystemConfigList, 92004, 6) == 1) //IsPrecautionQueryNoAgeNoSex
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => !((pr.AgeMax <= 0 && pr.AgeMin <= 0)
                                                                         && string.IsNullOrEmpty(pr.SexCd)));
            }
            if (GetSettingValue(allSystemConfigList, 92004, 7) == 1) //IsPrecautionQueryPropertyCd1
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 1);
            }
            if (GetSettingValue(allSystemConfigList, 92004, 8) == 1) //IsPrecautionQueryPropertyCd2
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 2);
            }
            if (GetSettingValue(allSystemConfigList, 92004, 9) == 1) //IsPrecautionQueryPropertyCd3
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 3);
            }
            if (GetSettingValue(allSystemConfigList, 92004, 10) == 1) //IsPrecautionQueryPropertyCd4
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 4);
            }
            if (GetSettingValue(allSystemConfigList, 92004, 11) == 1) //IsPrecautionQueryPropertyCd5
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 5);
            }
            if (GetSettingValue(allSystemConfigList, 92004, 12) == 1) //IsPrecautionQueryPropertyCd6
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 6);
            }
            if (GetSettingValue(allSystemConfigList, 92004, 13) == 1) //IsPrecautionQueryPropertyCd7
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 7);
            }
            if (GetSettingValue(allSystemConfigList, 92004, 14) == 1) //IsPrecautionQueryPropertyCd8
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 8);
            }
            if (GetSettingValue(allSystemConfigList, 92004, 15) == 1) //IsPrecautionQueryPropertyCd9
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 9);
            }

            var precautionCodeList = precautionCodeQuery.Distinct().ToList();

            var joinWithCodes = (from pe in precautionCds
                                 join peCode in precautionCodeList
                                 on pe.PrecautionCd equals peCode.PrecautionCd
                                 orderby pe.SeqNo
                                 select peCode).ToList();
            if (joinWithCodes != null)
            {
                foreach (var p in joinWithCodes)
                {
                    DrugInf inf = new DrugInf();
                    inf.ItemCd = itemCd;
                    inf.InfKbn = 2;
                    inf.DrugInfo = p.PrecautionCmt;
                    drugInf1.Add(inf);
                }
            }
        }
        result.AddRange(drugInf1);
        return result;
    }

    /// <summary>
    /// Get setting value from allSystemConfigList by groupCd and grpEdaNo
    /// </summary>
    /// <param name="allSystemConfigList"></param>
    /// <param name="groupCd"></param>
    /// <param name="grpEdaNo"></param>
    /// <returns></returns>
    private int GetSettingValue(List<SystemConfModel> allSystemConfigList, int groupCd, int grpEdaNo)
    {
        var systemConf = allSystemConfigList.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo);
        return systemConf != null ? (int)systemConf.Val : 0;
    }

    public PathPicture GetDefaultPathPicture()
    {
        // piczai pichou
        string pathServerDefault = _configuration["PathImageDrugFolder"] ?? string.Empty;

        var pathConfDb = NoTrackingDataContext.PathConfs.Where(p => p.GrpCd == PicImageConstant.GrpCodeDefault || p.GrpCd == PicImageConstant.GrpCodeCustomDefault).ToList();

        var pathConfDf = pathConfDb.FirstOrDefault(p => p.GrpCd == PicImageConstant.GrpCodeDefault);

        // PicZai
        string defaultPicZai = "";
        string defaultPicHou = "";
        if (pathConfDf != null)
        {
            defaultPicZai = pathConfDf.Path + "zaikei/";
            defaultPicHou = pathConfDf.Path + "housou/";
        }
        else
        {
            defaultPicZai = pathServerDefault + "zaikei/";
            defaultPicHou = pathServerDefault + "housou/";
        }
        return new PathPicture(defaultPicZai, defaultPicHou);
    }
}
