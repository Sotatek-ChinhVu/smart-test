using Domain.Constant;
using Domain.Models.SystemConf;
using Entity.Tenant;
using Helper.Common;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Reporting.DrugInfo.Model;

namespace Reporting.DrugInfo.DB;

public class CoDrugInfFinder : RepositoryBase, ICoDrugInfFinder
{
    private readonly ISystemConfRepository _systemConfRepository;
    private readonly IConfiguration _configuration;

    public CoDrugInfFinder(ITenantProvider tenantProvider, ISystemConfRepository systemConfRepository, IConfiguration configuration) : base(tenantProvider)
    {
        _systemConfRepository = systemConfRepository;
        _configuration = configuration;
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
        info.orderDate = CIUtil.SDateToShowWDate2(intOrderDate);

        var hpInfo = NoTrackingDataContext.HpInfs.Where(p => p.HpId == hpId && p.StartDate <= intOrderDate).OrderByDescending(p => p.StartDate).FirstOrDefault();
        if (hpInfo != null)
        {
            info.hpName = hpInfo.HpName ?? string.Empty;
            info.address1 = hpInfo.Address1 ?? string.Empty;
            info.address2 = hpInfo.Address2 ?? string.Empty;
            info.phone = hpInfo.Tel ?? string.Empty;
        }

        var ptInfo = NoTrackingDataContext.PtInfs.FirstOrDefault(pt => pt.HpId == hpId && pt.PtId == ptId);
        if (ptInfo != null)
        {
            info.ptNo = ptInfo.PtNum;
            info.ptName = ptInfo.Name ?? string.Empty;
            info.sex = ptInfo.Sex == 1 ? "M" : "F";
            info.intAge = (intOrderDate - ptInfo.Birthday) / 10000;
        }

        return info;
    }

    public List<OrderInfoModel> GetOrderByRaiinNo(long raiinNo)
    {
        var listOrderInfo = new List<OrderInfoModel>();
        var odrInfs = NoTrackingDataContext.OdrInfs.Where(o => o.RaiinNo == raiinNo && o.IsDeleted == 0 && new[] { 21, 22, 23, 28 }.Contains(o.OdrKouiKbn) && o.InoutKbn == 0).ToList();

        var infDetails = NoTrackingDataContext.OdrInfDetails.Where(d => d.RaiinNo == raiinNo && !(d.ItemCd != null && d.ItemCd.StartsWith("8") && d.ItemCd.Length == 9)).ToList();
        if (odrInfs != null && odrInfs.Count > 0)
        {
            foreach (var odrInf in odrInfs)
            {
                var orderInfoModel = new OrderInfoModel();
                orderInfoModel.RaiinNo = odrInf.RaiinNo;
                orderInfoModel.RpNo = odrInf.RpNo;
                orderInfoModel.OdrKouiKbn = odrInf.OdrKouiKbn;

                var details = infDetails.Where(d => d.RaiinNo == odrInf.RaiinNo && d.RpNo == odrInf.RpNo && d.RpEdaNo == odrInf.RpEdaNo).OrderBy(d => d.RowNo);
                List<OrderInfDetailModel> orderInfDetailModels = new List<OrderInfDetailModel>();
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

    public List<PiImage> GetProductImages(int hpId, string itemCd)
    {
        var images = NoTrackingDataContext.PiImages.Where(p => p.HpId == hpId && p.ItemCd == itemCd).ToList();
        return images;
    }

    public List<DrugInf> GetDrugInfo(int hpId, string itemCd, int age, int gender)
    {
        List<DrugInf> result = new();
        string strSex = gender.AsString();
        var drugInf = NoTrackingDataContext.DrugInfs.Where(d => d.HpId == hpId && d.ItemCd == itemCd && d.IsDeleted == 0 && d.InfKbn == 1).ToList();
        if (!drugInf.Any())
        {
            drugInf = new List<DrugInf>();
            var tenMsts = NoTrackingDataContext.TenMsts.Where(t => t.ItemCd == itemCd);
            var drugInfoMains = NoTrackingDataContext.M34DrugInfoMains;
            var joinQuery = (from t in tenMsts
                             join dm in drugInfoMains
                             on t.YjCd equals dm.YjCd
                             select dm.KonoCd);

            var konoCodes = joinQuery.GroupBy(g => g).Select(g => g.Key).ToList();

            var indicationCodes = NoTrackingDataContext.M34IndicationCodes;
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

        var drugInf1 = NoTrackingDataContext.DrugInfs.Where(d => d.HpId == hpId && d.ItemCd == itemCd && d.IsDeleted == 0 && d.InfKbn == 2).AsEnumerable().ToList();
        if (drugInf1 == null || drugInf1.Count == 0)
        {
            drugInf1 = new List<DrugInf>();

            var tenMsts = NoTrackingDataContext.TenMsts.Where(t => t.ItemCd == itemCd);
            var precaution = NoTrackingDataContext.M34Precautions;
            var joinQuery = (from t in tenMsts
                             join pr in precaution
                             on t.YjCd equals pr.YjCd
                             select new { pr.SeqNo, pr.PrecautionCd });

            var precautionCds = joinQuery.GroupBy(g => new { g.PrecautionCd }).Select(g => new { g.Key.PrecautionCd, g.FirstOrDefault()!.SeqNo }).ToList();

            var precautionCodes = NoTrackingDataContext.M34PrecautionCodes.Where(pr => ((pr.AgeMax <= 0 && pr.AgeMin <= 0)
                                                                                                        || (pr.AgeMax >= age && pr.AgeMin <= age)
                                                                                                        || (pr.AgeMax <= 0 && pr.AgeMin <= age))
                                                                                                    && (pr.SexCd == null || pr.SexCd == string.Empty || pr.SexCd == strSex));
            IQueryable<M34PrecautionCode> precautionCodeQuery = precautionCodes;
            if ((int)_systemConfRepository.GetSettingValue(92004, 3, hpId) == 1) //IsPrecautionQueryAgeSex
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => !((pr.AgeMin > 0 || pr.AgeMax > 0)
                                                                        && (pr.AgeMax >= age || pr.AgeMax <= 0)
                                                                        && pr.AgeMin <= age
                                                                        && !string.IsNullOrEmpty(pr.SexCd)
                                                                        && pr.SexCd == strSex));
            }
            if ((int)_systemConfRepository.GetSettingValue(92004, 4, hpId) == 1) //IsPrecautionQueryAgeNoSex
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => !((pr.AgeMin > 0 || pr.AgeMax > 0)
                                                                        && (pr.AgeMax >= age || pr.AgeMax <= 0)
                                                                        && pr.AgeMin <= age
                                                                        && string.IsNullOrEmpty(pr.SexCd)));
            }
            if ((int)_systemConfRepository.GetSettingValue(92004, 5, hpId) == 1) //IsPrecautionQuerySexNoAge
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => !((pr.AgeMax <= 0 && pr.AgeMin <= 0)
                                                                        && !string.IsNullOrEmpty(pr.SexCd)
                                                                        && pr.SexCd == strSex));
            }
            if ((int)_systemConfRepository.GetSettingValue(92004, 6, hpId) == 1) //IsPrecautionQueryNoAgeNoSex
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => !((pr.AgeMax <= 0 && pr.AgeMin <= 0)
                                                                         && string.IsNullOrEmpty(pr.SexCd)));
            }
            if ((int)_systemConfRepository.GetSettingValue(92004, 7, hpId) == 1) //IsPrecautionQueryPropertyCd1
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 1);
            }
            if ((int)_systemConfRepository.GetSettingValue(92004, 8, hpId) == 1) //IsPrecautionQueryPropertyCd2
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 2);
            }
            if ((int)_systemConfRepository.GetSettingValue(92004, 9, hpId) == 1) //IsPrecautionQueryPropertyCd3
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 3);
            }
            if ((int)_systemConfRepository.GetSettingValue(92004, 10, hpId) == 1) //IsPrecautionQueryPropertyCd4
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 4);
            }
            if ((int)_systemConfRepository.GetSettingValue(92004, 11, hpId) == 1) //IsPrecautionQueryPropertyCd5
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 5);
            }
            if ((int)_systemConfRepository.GetSettingValue(92004, 12, hpId) == 1) //IsPrecautionQueryPropertyCd6
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 6);
            }
            if ((int)_systemConfRepository.GetSettingValue(92004, 13, hpId) == 1) //IsPrecautionQueryPropertyCd7
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 7);
            }
            if ((int)_systemConfRepository.GetSettingValue(92004, 14, hpId) == 1) //IsPrecautionQueryPropertyCd8
            {
                precautionCodeQuery = precautionCodeQuery.Where(pr => pr.PropertyCd != 8);
            }
            if ((int)_systemConfRepository.GetSettingValue(92004, 15, hpId) == 1) //IsPrecautionQueryPropertyCd9
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

    public void ReleaseResource()
    {
        DisposeDataContext();
        _systemConfRepository.ReleaseResource();
    }
}
