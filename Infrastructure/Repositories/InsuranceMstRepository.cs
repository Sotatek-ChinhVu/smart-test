using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Helper.Common;
using Infrastructure.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class InsuranceMstRepository: IInsuranceMstRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public InsuranceMstRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public InsuranceMstModel GetDataInsuranceMst(int hpId, long ptId, int sinDate)
        {
            // data combobox 1 toki
            var TokkiMsts = _tenantDataContext.TokkiMsts.Where(entity => entity.HpId == hpId && entity.StartDate <= sinDate && entity.EndDate >= sinDate)
                    .OrderBy(entity => entity.HpId)
                    .ThenBy(entity => entity.TokkiCd)
                    .Select(x => new TokkiMstModel(
                                    x.TokkiCd,
                                    x.TokkiName
                        ))
                    .ToList();

            int prefNo = 0;
            var hpInf = _tenantDataContext.HpInfs.FirstOrDefault(x => x.HpId == hpId);
            if(hpInf != null)
            {
                prefNo = hpInf.PrefNo;
            }

            List<HokenMstModel> allHokenMst = new List<HokenMstModel>();
            var allHokenMstEntity = _tenantDataContext.HokenMsts.Where(x => x.HpId == hpId && (x.PrefNo == prefNo || x.PrefNo == 0 || x.IsOtherPrefValid == 1))
                                .OrderBy(e => e.HpId)
                                .ThenBy(e => e.HokenNo)
                                .ThenByDescending(e => e.PrefNo)
                                .ThenBy(e => e.SortNo)
                                .ThenByDescending(e => e.StartDate)
                                .ToList();
            var RoudouMsts = _tenantDataContext.RoudouMsts.OrderBy(entity => entity.RoudouCd).ToList();
            if (allHokenMstEntity.Count > 0)
            {
                foreach (var item in allHokenMstEntity)
                {
                    var prefName = RoudouMsts.FirstOrDefault(x => x.RoudouCd == item.PrefNo.ToString())?.RoudouName;
                    var itemModelNew = new HokenMstModel(
                                        item.HpId,
                                        item.PrefNo,
                                        item.HokenNo,
                                        item.HokenSbtKbn,
                                        item.HokenKohiKbn,
                                        item.Houbetu,
                                        item.HokenName,
                                        item.HokenNameCd,
                                        item.HokenEdaNo,
                                        item.StartDate,
                                        item.EndDate,
                                        item.IsOtherPrefValid,
                                        item.HokenSname,
                                        prefName == null ? string.Empty : prefName
                        );
                    allHokenMst.Add(itemModelNew);
                }
            }
            List<HokenMstModel> OldHokenMstList = new List<HokenMstModel>();
            List<HokenMstModel> NewHokenMstList = new List<HokenMstModel>();

            var iterHokNo = 0;
            var iterHokEda = 0;
            var iterPrefNo = 0;
            // All hoken master sort by start date descending
            foreach (var hokenMst in allHokenMst)
            {
                // foreach hoken mst, pick one hoken mst with: 
                // start date <= sinday or lastest hoken master if sinday < start date
                if (iterHokNo == hokenMst.HokenNo && iterHokEda == hokenMst.HokenEdaNo && iterPrefNo == hokenMst.PrefNo)
                {
                    continue;
                }

                iterHokNo = hokenMst.HokenNo;
                iterHokEda = hokenMst.HokenEdaNo;
                iterPrefNo = hokenMst.PrefNo;
                var hokMstMapped = allHokenMst
                    .FindAll(hk =>
                    hk.HokenNo == hokenMst.HokenNo
                    && hk.HokenEdaNo == hokenMst.HokenEdaNo
                    && hk.PrefNo == hokenMst.PrefNo)
                    .OrderByDescending(hk => hk.StartDate);

                if (hokMstMapped.Count() > 1)
                {
                    // pick one newest within startDate <= sinday
                    var firstMapped = hokMstMapped.FirstOrDefault(hokMst => hokMst.StartDate <= sinDate);
                    if (firstMapped == null)
                    {
                        // does not exist any hoken master with startDate <= sinday, pick lastest hoken mst (with min start date)
                        // pick last cause by all hoken master is order by start date descending
                        var itemLast = hokMstMapped.LastOrDefault();
                        if (itemLast != null)
                        {
                            OldHokenMstList.Add(itemLast);
                        }
                    }
                    else
                    {
                        OldHokenMstList.Add(firstMapped);
                        if (firstMapped.EndDate >= sinDate)
                        {
                            NewHokenMstList.Add(firstMapped);
                        }
                    }
                }
                else
                {
                    // have just one hoken mst with HokenNo and HokenEdaNo
                    OldHokenMstList.Add(hokenMst);
                    if (hokenMst.StartDate <= sinDate && hokenMst.EndDate >= sinDate)
                    {
                        NewHokenMstList.Add(hokenMst);
                    }
                }
            }

            var NewHokenInfMstList = NewHokenMstList.FindAll(hokenInf =>
                hokenInf.HokenSbtKbn == 1
                || hokenInf.HokenSbtKbn == 8
                || hokenInf.HokenSbtKbn == 0);

            var OldHokenInfMstList = OldHokenMstList.FindAll(hokenInf =>
                hokenInf.HokenSbtKbn == 1
                || hokenInf.HokenSbtKbn == 8
                || hokenInf.HokenSbtKbn == 0);

            var KohiHokenMst = NewHokenMstList.FindAll(kohiInf =>
                kohiInf.HokenSbtKbn == 2
                || kohiInf.HokenSbtKbn == 5
                || kohiInf.HokenSbtKbn == 6
                || kohiInf.HokenSbtKbn == 7);

            var RousaiMst = NewHokenMstList.FindAll(rousaiInf =>
                rousaiInf.HokenSbtKbn == 3
                || rousaiInf.HokenSbtKbn == 4);

            var JihiMst = NewHokenMstList.FindAll(hokenInf => hokenInf.HokenSbtKbn == 8
                                                        || hokenInf.HokenSbtKbn == 9);

            // data combobox Kantoku
            var KantokuMsts = _tenantDataContext.KantokuMsts.OrderBy(entity => entity.RoudouCd).ThenBy(entity => entity.KantokuCd)
                                .Select(x => new KantokuMstModel(
                                     x.RoudouCd,
                                     x.KantokuCd,
                                     x.KantokuName
                                    )).ToList();

            // data combobox ByomeiMstAftercares
            var ByomeiMstAftercares = _tenantDataContext.ByomeiMstAftercares.OrderBy(entity => entity.ByomeiCd)
                                         .Select(x => new ByomeiMstAftercareModel(
                                                x.ByomeiCd,
                                                x.Byomei
                                             ))
                                        .ToList();

            // data combobox 9
            var dataHokenInfor = _tenantDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId).FirstOrDefault();
            var dataComboboxHokenMst = new List<HokenMstModel>();
            var dataComboboxKantokuMst = new List<KantokuMstModel>();
            if (dataHokenInfor != null)
            {
                if (dataHokenInfor.HokenKbn == 0 || dataHokenInfor.Houbetu == HokenConstant.HOUBETU_JIHI_108 || dataHokenInfor.Houbetu == HokenConstant.HOUBETU_JIHI_109)
                {
                    if (JihiMst != null)
                    {
                        dataComboboxHokenMst = JihiMst;
                    }
                }
                else if (dataHokenInfor.HokenKbn == 11 || dataHokenInfor.HokenKbn == 12 || dataHokenInfor.HokenKbn == 13)
                {
                    dataComboboxHokenMst = RousaiMst.FindAll(rousaiMst => rousaiMst.HokenNo == 103);
                    if (!string.IsNullOrEmpty(dataHokenInfor.RousaiRoudouCd))
                    {
                        dataComboboxKantokuMst = KantokuMsts.FindAll(kantoku => kantoku.RoudouCD == dataHokenInfor.RousaiRoudouCd);
                    }
                    else
                    {
                        dataComboboxKantokuMst = KantokuMsts;
                    }
                }
                else if (dataHokenInfor.HokenKbn == 14)
                {
                    dataComboboxHokenMst = RousaiMst.FindAll(rousaiMst => rousaiMst.HokenNo == 104);
                }
                else if (string.IsNullOrEmpty(dataHokenInfor.Houbetu))
                {
                    dataComboboxHokenMst = NewHokenInfMstList;
                }
                else
                {
                    var hokenMstModel = NewHokenInfMstList.Find(hoken =>
                                                                    (hoken.Houbetu == dataHokenInfor.Houbetu
                                                                    || hoken.HokenNo == 68)
                                                                    && hoken.HokenNo == dataHokenInfor.HokenNo
                                                                    && hoken.HokenEdaNo == dataHokenInfor.HokenEdaNo);

                    if (hokenMstModel == null)
                    {
                        dataComboboxHokenMst = OldHokenInfMstList.FindAll(hoken =>
                                                                    (hoken.Houbetu == dataHokenInfor.Houbetu
                                                                    || hoken.HokenNo == 68)
                                                                    && ((hoken.StartDate <= sinDate && hoken.EndDate >= sinDate)
                                                                        || (hoken.HokenNo == dataHokenInfor.HokenNo
                                                                            && hoken.HokenEdaNo == dataHokenInfor.HokenEdaNo)
                                                                        )
                                                                    );
                    }
                    else
                    {
                        dataComboboxHokenMst = NewHokenInfMstList.FindAll(hoken =>
                                                                  hoken.Houbetu == dataHokenInfor.Houbetu
                                                                  || hoken.HokenNo == 68);
                    }
                }
            }

            // data combobox 8
            var dataHokenInf = _tenantDataContext.PtHokenInfs
                    .Where(entity => entity.HpId == hpId && entity.PtId == ptId)
                    .OrderByDescending(entity => entity.HokenId)
                    .Select(x => new HokenInfModel(
                            x.HpId,
                            x.PtId,
                            x.HokenId,
                            x.HokenKbn,
                            x.HokensyaNo ?? string.Empty,
                            x.HokenKbn,
                            x.StartDate,
                            x.EndDate,
                            sinDate
                        ))
                    .ToList();

            // data combobox 7  Kohi
            var dataKohis = _tenantDataContext.PtKohis
                    .Where(entity => entity.HpId == hpId && entity.PtId == ptId)
                    .OrderByDescending(entity => entity.HokenId)
                    .Select(x => new KohiInfModel(
                          x.FutansyaNo ?? string.Empty,
                          x.JyukyusyaNo ?? string.Empty,
                          x.HokenId,
                          x.StartDate,
                          x.EndDate,
                          0,
                          x.Rate,
                          x.GendoGaku,
                          x.SikakuDate,
                          x.KofuDate,
                          x.TokusyuNo ?? string.Empty,
                          x.HokenSbtKbn,
                          x.Houbetu ?? string.Empty,
                          x.HokenNo,
                          x.HokenEdaNo,
                          x.PrefNo
                       ))
                    .ToList();

            // data combobox 2 hokenKogakuKbnDict
            Dictionary<int, string> hokenKogakuKbnDict = new Dictionary<int, string>();
            if (dataHokenInfor != null)
            {
                int startDate = dataHokenInfor.StartDate;
                int endDate = dataHokenInfor.EndDate > 0 ? dataHokenInfor.EndDate : 99999999;
                List<int> dateList = new List<int>() { sinDate, startDate, endDate };
                dateList.Sort();
                // get date value between 3 values of date => get second element
                int standardDate = dateList[1];
                var patientInf = _tenantDataContext.PtInfs.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId);
                if (patientInf != null)
                {
                    int standardAge = CIUtil.SDateToAge(patientInf.Birthday, standardDate);

                    hokenKogakuKbnDict.Add(0, "");
                    if (standardAge >= 70 || (dataHokenInfor.HokensyaNo != null && dataHokenInfor.HokensyaNo.StartsWith("39")))
                    {
                        if (sinDate < KaiseiDate.d20180801)
                        {
                            hokenKogakuKbnDict.Add(3, "3 上位");
                        }
                        hokenKogakuKbnDict.Add(4, "4 低所Ⅱ");
                        hokenKogakuKbnDict.Add(5, "5 低所Ⅰ");
                        if (sinDate < KaiseiDate.d20090101)
                        {
                            hokenKogakuKbnDict.Add(6, "6 特定収入");
                        }
                        hokenKogakuKbnDict.Add(26, "26 現役Ⅲ");
                        hokenKogakuKbnDict.Add(27, "27 現役Ⅱ");
                        hokenKogakuKbnDict.Add(28, "28 現役Ⅰ");
                    }
                    else
                    {
                        if (sinDate < KaiseiDate.d20150101)
                        {
                            hokenKogakuKbnDict.Add(17, "17 上位[A]");
                            hokenKogakuKbnDict.Add(18, "18 一般[B]");
                            hokenKogakuKbnDict.Add(19, "19 低所[C]");
                        }
                        hokenKogakuKbnDict.Add(26, "26 区ア");
                        hokenKogakuKbnDict.Add(27, "27 区イ");
                        hokenKogakuKbnDict.Add(28, "28 区ウ");
                        hokenKogakuKbnDict.Add(29, "29 区エ");
                        hokenKogakuKbnDict.Add(30, "30 区オ");
                    }
                }
            }

            return new InsuranceMstModel(TokkiMsts, hokenKogakuKbnDict, KohiHokenMst, dataKohis, dataHokenInf, dataComboboxKantokuMst, ByomeiMstAftercares, dataComboboxHokenMst);
        }

        public IEnumerable<HokensyaMstModel> SearchListDataHokensyaMst(int hpId, int pageIndex, int pageCount, int sinDate, string keyword)
        {
            int prefNo = 0;
            var hpInf = _tenantDataContext.HpInfs.FirstOrDefault(x => x.HpId == hpId);
            if (hpInf != null)
            {
                prefNo = hpInf.PrefNo;
            }

            var listAllDataHokensyaMst = _tenantDataContext.HokensyaMsts.Where(x => (!String.IsNullOrEmpty(x.HokensyaNo) && x.HokensyaNo.StartsWith(keyword))
                                                        && (x.PrefNo == 0 || x.PrefNo == prefNo) 
                                                        && (x.HokenKbn == 1 || x.HokenKbn == 2)
                                                        && x.HpId == hpId
                                                        && x.IsDelete == 0
                                                        && x.DeleteDate < sinDate)
                                                        .ToList();
            var listDataPaging = listAllDataHokensyaMst.Select(item => new HokensyaMstModel(
                                                                item.HpId,
                                                                item.Name ?? string.Empty,
                                                                item.KanaName ?? string.Empty,
                                                                item.HoubetuKbn ?? string.Empty,
                                                                item.Houbetu ?? string.Empty,
                                                                item.HokenKbn,
                                                                item.PrefNo,
                                                                item.HokensyaNo,
                                                                item.Kigo ?? string.Empty,
                                                                item.Bango ?? string.Empty,
                                                                item.RateHonnin,
                                                                item.RateKazoku,
                                                                item.PostCode ?? string.Empty,
                                                                item.Address1 ?? string.Empty,
                                                                item.Address2 ?? string.Empty,
                                                                item.Tel1 ?? string.Empty
                                                            ))
                                .OrderBy(item => item.HokensyaNo).Skip(pageIndex).Take(pageCount);
            return listDataPaging;
        }
    }
}
