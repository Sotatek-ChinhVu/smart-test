using Domain.Models.InsuranceMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Extendsions;
using Infrastructure.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class InsuranceMstRepository : IInsuranceMstRepository
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
            if (hpInf != null)
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

            // data combobox Kantoku
            var kantokuMsts = _tenantDataContext.KantokuMsts.OrderBy(entity => entity.RoudouCd).ThenBy(entity => entity.KantokuCd)
                                .Select(x => new KantokuMstModel(
                                     x.RoudouCd,
                                     x.KantokuCd,
                                     x.KantokuName
                                    )).ToList();

            // data combobox ByomeiMstAftercares
            var byomeiMstAftercares = _tenantDataContext.ByomeiMstAftercares.OrderBy(entity => entity.ByomeiCd)
                                         .Select(x => new ByomeiMstAftercareModel(
                                                x.ByomeiCd,
                                                x.Byomei
                                             ))
                                        .ToList();

            // data combobox 9
            var dataHokenInfor = _tenantDataContext.PtHokenInfs.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId);
            var dataComboboxKantokuMst = new List<KantokuMstModel>();
            if (dataHokenInfor != null &&
                (dataHokenInfor.HokenKbn == 11 || dataHokenInfor.HokenKbn == 12 || dataHokenInfor.HokenKbn == 13))
            {
                if (!string.IsNullOrEmpty(dataHokenInfor.RousaiRoudouCd))
                {
                    dataComboboxKantokuMst = kantokuMsts.FindAll(kantoku => kantoku.RoudouCD == dataHokenInfor.RousaiRoudouCd);
                }
                else
                {
                    dataComboboxKantokuMst = kantokuMsts;
                }
            }

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

            return new InsuranceMstModel(TokkiMsts, hokenKogakuKbnDict, GetHokenMstList(sinDate, true), dataComboboxKantokuMst, byomeiMstAftercares, GetHokenMstList(sinDate, false));
        }

        private List<HokenMstModel> GetHokenMstList(int today, bool isKohi)
        {
            var hospitalInfo = _tenantDataContext.HpInfs
                .Where(p => p.HpId == 1)
                .OrderByDescending(p => p.StartDate)
                .FirstOrDefault();

            int prefCd = 0;
            if (hospitalInfo != null)
            {
                prefCd = hospitalInfo.PrefNo;
            }

            List<HokenMstModel> list = new List<HokenMstModel>();

            IQueryable<HokenMst> query;

            if (isKohi)
            {
                query = _tenantDataContext.HokenMsts.Where(kohiInf =>
                    (kohiInf.HokenSbtKbn == 2 || kohiInf.HokenSbtKbn == 5 || kohiInf.HokenSbtKbn == 6 || kohiInf.HokenSbtKbn == 7)
                    && kohiInf.StartDate < today
                    && kohiInf.EndDate > today
                    && (kohiInf.PrefNo == prefCd || kohiInf.PrefNo == 0 || kohiInf.IsOtherPrefValid == 1));
            }
            else
            {
                query = _tenantDataContext.HokenMsts.Where(hokenInf =>
                    (hokenInf.HokenSbtKbn == 1 || hokenInf.HokenSbtKbn == 8)
                    && hokenInf.StartDate < today
                    && hokenInf.EndDate > today
                    && (hokenInf.PrefNo == prefCd || hokenInf.PrefNo == 0 || hokenInf.IsOtherPrefValid == 1));
            }

            List<HokenMst> entities = query
                .OrderBy(entity => entity.HpId)
                .ThenBy(entity => entity.HokenNo)
                .ThenBy(entity => entity.SortNo)
                .ThenBy(entity => entity.HokenSbtKbn)
                .ThenBy(entity => entity.StartDate)
                .ToList();

            List<RoudouMst> roudouMsts = _tenantDataContext.RoudouMsts.ToList();
            entities?.ForEach(h =>
            {
                string prefName = string.Empty;
                if (roudouMsts.Any(roudou => roudou.RoudouCd.AsInteger() == h.PrefNo))
                {
                    prefName = roudouMsts.First(roudou => roudou.RoudouCd.AsInteger() == h.PrefNo)!.RoudouName;
                }

                list.Add(new HokenMstModel(h.HpId, h.PrefNo, h.HokenNo, h.HokenSbtKbn, h.HokenKohiKbn, h.Houbetu, h.HokenName, h.HokenNameCd, h.HokenEdaNo, h.StartDate, h.EndDate, h.IsOtherPrefValid, h.HokenSname, prefName));
            });

            return list;
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
