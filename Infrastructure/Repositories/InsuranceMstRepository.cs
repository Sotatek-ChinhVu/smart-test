using Domain.Models.HokenMst;
using Domain.Models.InsuranceMst;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Repositories
{
    public class InsuranceMstRepository : RepositoryBase, IInsuranceMstRepository
    {
        public InsuranceMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public InsuranceMstModel GetDataInsuranceMst(int hpId, long ptId, int sinDate)
        {
            // data combobox 1 toki
            var TokkiMsts = NoTrackingDataContext.TokkiMsts.Where(entity => entity.HpId == hpId && entity.StartDate <= sinDate && entity.EndDate >= sinDate)
                    .OrderBy(entity => entity.HpId)
                    .ThenBy(entity => entity.TokkiCd)
                    .Select(x => new TokkiMstModel(
                                    x.TokkiCd,
                                    x.TokkiName ?? string.Empty
                        ))
                    .ToList();

            int prefNo = 0;
            var hpInf = NoTrackingDataContext.HpInfs.FirstOrDefault(x => x.HpId == hpId);
            if (hpInf != null)
            {
                prefNo = hpInf.PrefNo;
            }

            List<HokenMstModel> allHokenMst = new List<HokenMstModel>();
            var allHokenMstEntity = NoTrackingDataContext.HokenMsts.Where(x => x.HpId == hpId && (x.PrefNo == prefNo || x.PrefNo == 0 || x.IsOtherPrefValid == 1))
                                .OrderBy(e => e.HpId)
                                .ThenBy(e => e.HokenNo)
                                .ThenByDescending(e => e.PrefNo)
                                .ThenBy(e => e.SortNo)
                                .ThenByDescending(e => e.StartDate)
                                .ToList();
            var RoudouMsts = NoTrackingDataContext.RoudouMsts.OrderBy(entity => entity.RoudouCd).ToList();
            if (allHokenMstEntity.Count > 0)
            {
                foreach (var item in allHokenMstEntity)
                {
                    var prefName = RoudouMsts.FirstOrDefault(x => x.RoudouCd == item.PrefNo.ToString())?.RoudouName;
                    var itemModelNew = new HokenMstModel(
                                        item.FutanKbn,
                                        item.FutanRate,
                                        item.StartDate,
                                        item.EndDate,
                                        item.HokenNo,
                                        item.HokenEdaNo,
                                        item.HokenSname ?? string.Empty,
                                        item.Houbetu ?? string.Empty,
                                        item.HokenSbtKbn,
                                        item.CheckDigit,
                                        item.AgeStart,
                                        item.AgeEnd,
                                        item.IsFutansyaNoCheck,
                                        item.IsJyukyusyaNoCheck,
                                        item.JyukyuCheckDigit,
                                        item.IsTokusyuNoCheck,
                                        item.HokenName ?? string.Empty,
                                        item.HokenNameCd ?? string.Empty,
                                        item.HokenKohiKbn,
                                        item.IsOtherPrefValid,
                                        item.ReceKisai,
                                        item.IsLimitList,
                                        item.IsLimitListSum,
                                        item.EnTen,
                                        item.KaiLimitFutan,
                                        item.DayLimitFutan,
                                        item.MonthLimitFutan,
                                        item.MonthLimitCount,
                                        item.LimitKbn,
                                        item.CountKbn,
                                        item.FutanYusen,
                                        item.CalcSpKbn,
                                        item.MonthSpLimit,
                                        item.KogakuTekiyo,
                                        item.KogakuTotalKbn,
                                        item.KogakuHairyoKbn,
                                        item.ReceSeikyuKbn,
                                        item.ReceKisaiKokho,
                                        item.ReceKisai2,
                                        item.ReceTenKisai,
                                        item.ReceFutanRound,
                                        item.ReceZeroKisai,
                                        item.ReceSpKbn,
                                        prefName == null ? string.Empty : prefName,
                                        item.PrefNo,
                                        item.SortNo
                        );
                    allHokenMst.Add(itemModelNew);
                }
            }

            // data combobox Kantoku
            var kantokuMsts = NoTrackingDataContext.KantokuMsts.OrderBy(entity => entity.RoudouCd).ThenBy(entity => entity.KantokuCd)
                                .Select(x => new KantokuMstModel(
                                     x.RoudouCd,
                                     x.KantokuCd,
                                     x.KantokuName ?? string.Empty
                                    )).ToList();

            // data combobox ByomeiMstAftercares
            var byomeiMstAftercares = NoTrackingDataContext.ByomeiMstAftercares.OrderBy(entity => entity.ByomeiCd)
                                         .Select(x => new ByomeiMstAftercareModel(
                                                x.ByomeiCd,
                                                x.Byomei
                                             ))
                                        .ToList();

            // data combobox 9
            var dataHokenInfor = NoTrackingDataContext.PtHokenInfs.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId);
            List<KantokuMstModel> dataComboboxKantokuMst;

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
            else
            {
                dataComboboxKantokuMst = kantokuMsts;
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
                var patientInf = NoTrackingDataContext.PtInfs.FirstOrDefault(x => x.HpId == hpId && x.PtId == ptId);
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

            var dataRoudouMst = RoudouMsts.Select(x => new RoudouMstModel(
                                            x.RoudouCd,
                                            x.RoudouName ?? string.Empty
                                            )).ToList();

            return new InsuranceMstModel(TokkiMsts, hokenKogakuKbnDict, GetHokenMstList(sinDate, true), dataComboboxKantokuMst, byomeiMstAftercares, GetHokenMstList(sinDate, false), dataRoudouMst, allHokenMst);
        }

        private List<HokenMstModel> GetHokenMstList(int today, bool isKohi)
        {
            var hospitalInfo = NoTrackingDataContext.HpInfs
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
                query = NoTrackingDataContext.HokenMsts.Where(kohiInf =>
                    (kohiInf.HokenSbtKbn == 2 || kohiInf.HokenSbtKbn == 5 || kohiInf.HokenSbtKbn == 6 || kohiInf.HokenSbtKbn == 7)
                    && kohiInf.StartDate < today
                    && kohiInf.EndDate > today
                    && (kohiInf.PrefNo == prefCd || kohiInf.PrefNo == 0 || kohiInf.IsOtherPrefValid == 1));
            }
            else
            {
                query = NoTrackingDataContext.HokenMsts.Where(hokenInf =>
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

            List<RoudouMst> roudouMsts = NoTrackingDataContext.RoudouMsts.ToList();
            entities?.ForEach(h =>
            {
                string prefName = string.Empty;
                if (roudouMsts.Any(roudou => roudou.RoudouCd.AsInteger() == h.PrefNo))
                {
                    prefName = roudouMsts.First(roudou => roudou.RoudouCd.AsInteger() == h.PrefNo)!.RoudouName ?? string.Empty;
                }

                list.Add(new HokenMstModel(h.FutanKbn,
                                            h.FutanRate,
                                            h.StartDate,
                                            h.EndDate,
                                            h.HokenNo,
                                            h.HokenEdaNo,
                                            h.HokenSname ?? string.Empty,
                                            h.Houbetu ?? string.Empty,
                                            h.HokenSbtKbn,
                                            h.CheckDigit,
                                            h.AgeStart,
                                            h.AgeEnd,
                                            h.IsFutansyaNoCheck,
                                            h.IsJyukyusyaNoCheck,
                                            h.JyukyuCheckDigit,
                                            h.IsTokusyuNoCheck,
                                            h.HokenName ?? string.Empty,
                                            h.HokenNameCd ?? string.Empty,
                                            h.HokenKohiKbn,
                                            h.IsOtherPrefValid,
                                            h.ReceKisai,
                                            h.IsLimitList,
                                            h.IsLimitListSum,
                                            h.EnTen,
                                            h.KaiLimitFutan,
                                            h.DayLimitFutan,
                                            h.MonthLimitFutan,
                                            h.MonthLimitCount,
                                            h.LimitKbn,
                                            h.CountKbn,
                                            h.FutanYusen,
                                            h.CalcSpKbn,
                                            h.MonthSpLimit,
                                            h.KogakuTekiyo,
                                            h.KogakuTotalKbn,
                                            h.KogakuHairyoKbn,
                                            h.ReceSeikyuKbn,
                                            h.ReceKisaiKokho,
                                            h.ReceKisai2,
                                            h.ReceTenKisai,
                                            h.ReceFutanRound,
                                            h.ReceZeroKisai,
                                            h.ReceSpKbn,
                                            prefName == null ? string.Empty : prefName,
                                            h.PrefNo,
                                            h.SortNo));
            });

            return list;
        }

        public IEnumerable<HokensyaMstModel> SearchListDataHokensyaMst(int hpId, int sinDate, string keyword)
        {
            int prefNo = 0;
            var hpInf = NoTrackingDataContext.HpInfs.FirstOrDefault(x => x.HpId == hpId);
            if (hpInf != null)
            {
                prefNo = hpInf.PrefNo;
            }

            var listAllDataHokensyaMst = NoTrackingDataContext.HokensyaMsts.Where(x => (!String.IsNullOrEmpty(x.HokensyaNo) && x.HokensyaNo.StartsWith(keyword))
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
                                                                item.HokensyaNo ?? string.Empty,
                                                                item.Kigo ?? string.Empty,
                                                                item.Bango ?? string.Empty,
                                                                item.RateHonnin,
                                                                item.RateKazoku,
                                                                item.PostCode ?? string.Empty,
                                                                item.Address1 ?? string.Empty,
                                                                item.Address2 ?? string.Empty,
                                                                item.Tel1 ?? string.Empty,
                                                                item.IsKigoNa
                                                            ))
                                .OrderBy(item => item.HokensyaNo).ToList();
            return listDataPaging;
        }

        public HokenMstModel GetHokenMstByFutansyaNo(int hpId, int sinDate, string futansyaNo)
        {
            var hospitalInfo = NoTrackingDataContext.HpInfs
                .Where(p => p.HpId == hpId)
                .OrderByDescending(p => p.StartDate)
                .FirstOrDefault();

            int prefCd = 0;
            if (hospitalInfo != null)
            {
                prefCd = hospitalInfo.PrefNo;
            }

            string kohiHobetu = string.Empty;
            if (futansyaNo.Length == 8)
            {
                kohiHobetu = futansyaNo.Substring(0, 2);
            }

            List<HokenMstModel> list = new List<HokenMstModel>();

            IQueryable<HokenMst> query;

            query = NoTrackingDataContext.HokenMsts.Where(kohiInf =>
                (kohiInf.HokenSbtKbn == 2 || kohiInf.HokenSbtKbn == 5 || kohiInf.HokenSbtKbn == 6 || kohiInf.HokenSbtKbn == 7)
                && kohiInf.StartDate < sinDate
                && kohiInf.EndDate > sinDate
                && (kohiInf.PrefNo == prefCd || kohiInf.PrefNo == 0 || kohiInf.IsOtherPrefValid == 1)
                && (string.IsNullOrEmpty(kohiHobetu) || kohiInf.Houbetu == kohiHobetu));

            List<HokenMst> entities = query
                .OrderBy(entity => entity.HpId)
                .ThenBy(entity => entity.HokenNo)
                .ThenBy(entity => entity.SortNo)
                .ThenBy(entity => entity.HokenSbtKbn)
                .ThenBy(entity => entity.StartDate)
                .ToList();

            entities?.ForEach(h =>
            {
                list.Add(new HokenMstModel(h.FutanKbn,
                                            h.FutanRate,
                                            h.StartDate,
                                            h.EndDate,
                                            h.HokenNo,
                                            h.HokenEdaNo,
                                            h.HokenSname ?? string.Empty,
                                            h.Houbetu ?? string.Empty,
                                            h.HokenSbtKbn,
                                            h.CheckDigit,
                                            h.AgeStart,
                                            h.AgeEnd,
                                            h.IsFutansyaNoCheck,
                                            h.IsJyukyusyaNoCheck,
                                            h.JyukyuCheckDigit,
                                            h.IsTokusyuNoCheck,
                                            h.HokenName ?? string.Empty,
                                            h.HokenNameCd ?? string.Empty,
                                            h.HokenKohiKbn,
                                            h.IsOtherPrefValid,
                                            h.ReceKisai,
                                            h.IsLimitList,
                                            h.IsLimitListSum,
                                            h.EnTen,
                                            h.KaiLimitFutan,
                                            h.DayLimitFutan,
                                            h.MonthLimitFutan,
                                            h.MonthLimitCount,
                                            h.LimitKbn,
                                            h.CountKbn,
                                            h.FutanYusen,
                                            h.CalcSpKbn,
                                            h.MonthSpLimit,
                                            h.KogakuTekiyo,
                                            h.KogakuTotalKbn,
                                            h.KogakuHairyoKbn,
                                            h.ReceSeikyuKbn,
                                            h.ReceKisaiKokho,
                                            h.ReceKisai2,
                                            h.ReceTenKisai,
                                            h.ReceFutanRound,
                                            h.ReceZeroKisai,
                                            h.ReceSpKbn,
                                            string.Empty,
                                            h.PrefNo,
                                            h.SortNo));
            });

            // Get KohiMst
            if (futansyaNo.Length == 8)
            {
                string digit1 = futansyaNo[0].AsString();
                string digit2 = futansyaNo[1].AsString();
                string digit3 = futansyaNo[2].AsString();
                string digit4 = futansyaNo[3].AsString();
                string digit5 = futansyaNo[4].AsString();
                string digit6 = futansyaNo[5].AsString();
                string digit7 = futansyaNo[6].AsString();
                string digit8 = futansyaNo[7].AsString();
                var defHokenNo = NoTrackingDataContext.DefHokenNos.Where(x => x.HpId == hpId
                                                                                && x.Digit1 == digit1
                                                                                && x.Digit2 == digit2
                                                                                && (x.Digit3 == null || x.Digit3.Trim() == "" || x.Digit3 == digit3)
                                                                                && (x.Digit4 == null || x.Digit4.Trim() == "" || x.Digit4 == digit4)
                                                                                && (x.Digit5 == null || x.Digit5.Trim() == "" || x.Digit5 == digit5)
                                                                                && (x.Digit6 == null || x.Digit6.Trim() == "" || x.Digit6 == digit6)
                                                                                && (x.Digit7 == null || x.Digit7.Trim() == "" || x.Digit7 == digit7)
                                                                                && (x.Digit8 == null || x.Digit8.Trim() == "" || x.Digit8 == digit8)
                                                                                && x.IsDeleted != 1)
                                                           .OrderBy(x => x.SortNo).FirstOrDefault();
                if (defHokenNo != null)
                {
                    var kohiHokenMst = list.FirstOrDefault(x => x.HokenNo == defHokenNo.HokenNo && x.HokenEdaNo == defHokenNo.HokenEdaNo);
                    if (kohiHokenMst != null)
                    {
                        return kohiHokenMst;
                    }
                }
                else
                {
                    var kohiHokenMst = list.FirstOrDefault();
                    if (kohiHokenMst != null)
                    {
                        return kohiHokenMst;
                    }
                }
            }

            return new HokenMstModel();

        }

        public bool SaveHokenSyaMst(HokensyaMstModel model, int userId)
        {
            var hoken = TrackingDataContext.HokensyaMsts.FirstOrDefault(x => x.HpId == model.HpId && (x.HokensyaNo != null && x.HokensyaNo.Equals(model.HokensyaNo)));
            if (hoken is null)
            {
                HokensyaMst create = new HokensyaMst()
                {
                    HpId = model.HpId,
                    Name = model.Name,
                    KanaName = model.KanaName,
                    HoubetuKbn = model.HoubetuKbn,
                    Houbetu = model.Houbetu,
                    HokenKbn = model.HokenKbn,
                    PrefNo = model.PrefNo,
                    HokensyaNo = model.HokensyaNo,
                    Kigo = model.Kigo,
                    Bango = model.Bango,
                    RateHonnin = model.RateHonnin,
                    RateKazoku = model.RateKazoku,
                    PostCode = model.PostCode,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    Tel1 = model.Tel1,
                    CreateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateDate = CIUtil.GetJapanDateTimeNow(),
                    UpdateId = userId,
                    CreateId = userId
                };
                TrackingDataContext.HokensyaMsts.Add(create);
            }
            else
            {
                hoken.Name = model.Name;
                hoken.KanaName = model.KanaName;
                hoken.HoubetuKbn = model.HoubetuKbn;
                hoken.Houbetu = model.Houbetu;
                hoken.HokenKbn = model.HokenKbn;
                hoken.PrefNo = model.PrefNo;
                hoken.Kigo = model.Kigo;
                hoken.Bango = model.Bango;
                hoken.RateHonnin = model.RateHonnin;
                hoken.RateKazoku = model.RateKazoku;
                hoken.PostCode = model.PostCode;
                hoken.Address1 = model.Address1;
                hoken.Address2 = model.Address2;
                hoken.Tel1 = model.Tel1;
                hoken.UpdateDate = CIUtil.GetJapanDateTimeNow();
                hoken.UpdateId = userId;
            }
            return TrackingDataContext.SaveChanges() > 0;
        }

        public HokensyaMstModel FindHokenSyaMstInf(int hpId, string hokensyaNo, int hokenKbn, string houbetuNo, string hokensyaNoSearch)
        {
            var entity = NoTrackingDataContext.HokensyaMsts.FirstOrDefault(hokensya => hokensya.HpId == hpId
                                                        && hokensya.Houbetu == houbetuNo
                                                        && hokensya.HokensyaNo == hokensyaNoSearch);

            if(entity is null)
            {
                entity = new HokensyaMst()
                {
                    Houbetu = houbetuNo,
                    HokensyaNo = hokensyaNo,
                    HokenKbn = hokenKbn,
                    IsKigoNa = 0,
                };
            }

            return new HokensyaMstModel(entity.HpId
                                        ,entity.Name ?? string.Empty
                                        ,entity.KanaName ?? string.Empty
                                        ,entity.HoubetuKbn ?? string.Empty
                                        ,entity.Houbetu ?? string.Empty
                                        ,entity.HokenKbn
                                        ,entity.PrefNo
                                        ,entity.HokensyaNo ?? string.Empty
                                        ,entity.Kigo ?? string.Empty
                                        ,entity.Bango ?? string.Empty
                                        ,entity.RateHonnin
                                        ,entity.RateKazoku
                                        ,entity.PostCode ?? string.Empty
                                        ,entity.Address1 ?? string.Empty
                                        ,entity.Address2 ?? string.Empty
                                        ,entity.Tel1 ?? string.Empty
                                        ,entity.IsKigoNa); ;
        }

        public List<InsuranceMasterDetailModel> GetInsuranceMasterDetails(int hpId, int FHokenNo, int FHokenSbtKbn, bool IsJitan, bool IsTaken)
        {
            var hospitalInfo = NoTrackingDataContext.HpInfs
                .Where(p => p.HpId == hpId)
                .OrderByDescending(p => p.StartDate)
                .FirstOrDefault();

            int prefCd = 0;
            if (hospitalInfo != null)
                prefCd = hospitalInfo.PrefNo;

            var hokenMst = (NoTrackingDataContext.HokenMsts
                                            .Where(x => 
                                                   x.HpId == hpId
                                                && (FHokenNo == 0 || x.HokenNo == FHokenNo)
                                                && (FHokenSbtKbn == 0 || x.HokenSbtKbn == FHokenSbtKbn)
                                                && (!IsJitan || x.PrefNo > 0)
                                                && (IsTaken || (x.PrefNo == 0 || x.PrefNo == prefCd))
                                            )
                            .GroupBy(hk => new { hk.HpId, hk.PrefNo, hk.HokenNo })
                            .Select(grp => grp.OrderByDescending(h => h.StartDate).FirstOrDefault())).ToList();

            var hokenDetail = (NoTrackingDataContext.HokenMsts.Where(x => x.HpId == hpId)
                            .GroupBy(hk => new { hk.HpId, hk.PrefNo, hk.HokenNo, hk.HokenEdaNo })
                            .Select(grp => grp.OrderByDescending(h => h.StartDate).FirstOrDefault())).ToList();

            var result = from mst in hokenMst
                        join detail in hokenDetail
                            on
                            new { mst.HpId, mst.HokenNo, mst.PrefNo } equals
                            new { detail.HpId, detail.HokenNo, detail.PrefNo } into details
                        select new InsuranceMasterDetailModel(
                                        new HokenMstModel(mst.FutanKbn,
                                                          mst.FutanRate,
                                                          mst.StartDate,
                                                          mst.EndDate,
                                                          mst.HokenNo,
                                                          mst.HokenEdaNo,
                                                          mst.HokenSname ?? string.Empty,
                                                          mst.Houbetu ?? string.Empty,
                                                          mst.HokenSbtKbn,
                                                          mst.CheckDigit,
                                                          mst.AgeStart,
                                                          mst.AgeEnd,
                                                          mst.IsFutansyaNoCheck,
                                                          mst.IsJyukyusyaNoCheck,
                                                          mst.JyukyuCheckDigit,
                                                          mst.IsTokusyuNoCheck,
                                                          mst.HokenName ?? string.Empty,
                                                          mst.HokenNameCd ?? string.Empty,
                                                          mst.HokenKohiKbn,
                                                          mst.IsOtherPrefValid,
                                                          mst.ReceKisai,
                                                          mst.IsLimitList,
                                                          mst.IsLimitListSum,
                                                          mst.EnTen,
                                                          mst.KaiLimitFutan,
                                                          mst.DayLimitFutan,
                                                          mst.MonthLimitFutan,
                                                          mst.MonthLimitCount,
                                                          mst.LimitKbn,
                                                          mst.CountKbn,
                                                          mst.FutanYusen,
                                                          mst.CalcSpKbn,
                                                          mst.MonthSpLimit,
                                                          mst.KogakuTekiyo,
                                                          mst.KogakuTotalKbn,
                                                          mst.KogakuHairyoKbn,
                                                          mst.ReceSeikyuKbn,
                                                          mst.ReceKisaiKokho,
                                                          mst.ReceKisai2,
                                                          mst.ReceTenKisai,
                                                          mst.ReceFutanRound,
                                                          mst.ReceZeroKisai,
                                                          mst.ReceSpKbn,
                                                          string.Empty,
                                                          mst.PrefNo,
                                                          mst.SortNo),
                                        details.Select(x => new HokenMstModel(x.FutanKbn,
                                                          x.FutanRate,
                                                          x.StartDate,
                                                          x.EndDate,
                                                          x.HokenNo,
                                                          x.HokenEdaNo,
                                                          x.HokenSname ?? string.Empty,
                                                          x.Houbetu ?? string.Empty,
                                                          x.HokenSbtKbn,
                                                          x.CheckDigit,
                                                          x.AgeStart,
                                                          x.AgeEnd,
                                                          x.IsFutansyaNoCheck,
                                                          x.IsJyukyusyaNoCheck,
                                                          x.JyukyuCheckDigit,
                                                          x.IsTokusyuNoCheck,
                                                          x.HokenName ?? string.Empty,
                                                          x.HokenNameCd ?? string.Empty,
                                                          x.HokenKohiKbn,
                                                          x.IsOtherPrefValid,
                                                          x.ReceKisai,
                                                          x.IsLimitList,
                                                          x.IsLimitListSum,
                                                          x.EnTen,
                                                          x.KaiLimitFutan,
                                                          x.DayLimitFutan,
                                                          x.MonthLimitFutan,
                                                          x.MonthLimitCount,
                                                          x.LimitKbn,
                                                          x.CountKbn,
                                                          x.FutanYusen,
                                                          x.CalcSpKbn,
                                                          x.MonthSpLimit,
                                                          x.KogakuTekiyo,
                                                          x.KogakuTotalKbn,
                                                          x.KogakuHairyoKbn,
                                                          x.ReceSeikyuKbn,
                                                          x.ReceKisaiKokho,
                                                          x.ReceKisai2,
                                                          x.ReceTenKisai,
                                                          x.ReceFutanRound,
                                                          x.ReceZeroKisai,
                                                          x.ReceSpKbn,
                                                          string.Empty,
                                                          x.PrefNo,
                                                          x.SortNo))
                                        );

            return result.OrderBy(h => h.Master.HokenNo).ToList();
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

        public List<SelectMaintenanceModel> GetSelectMaintenance(int hpId, int hokenNo, int hokenEdaNo, int prefNo, int startDate)
        {
            var result = new List<SelectMaintenanceModel>();

            var hospitalInfo = NoTrackingDataContext.HpInfs
                .Where(p => p.HpId == hpId)
                .OrderByDescending(p => p.StartDate)
                .FirstOrDefault();

            int prefCd = 0;
            if (hospitalInfo != null)
                prefCd = hospitalInfo.PrefNo;

            int today = CIUtil.DateTimeToInt(DateTime.Now);

            bool CheckOneStartDate = NoTrackingDataContext.HokenMsts.Count(x => x.HpId == hpId &&
                                                                                x.HokenNo == hokenNo &&
                                                                                x.HokenEdaNo == hokenEdaNo &&
                                                                                x.PrefNo == prefNo) <= 1;

            if(!CheckOneStartDate)
            {
                var hokenMsts = NoTrackingDataContext.HokenMsts.Where(x => x.HpId == hpId &&
                                                                          x.HokenEdaNo == hokenEdaNo &&
                                                                          x.HokenNo == hokenNo &&
                                                                          x.PrefNo == prefNo).OrderBy(x => x.StartDate)
                                                                          .Select(x => new HokenMstModel(x.FutanKbn,
                                                                                                         x.FutanRate,
                                                                                                         x.StartDate,
                                                                                                         x.EndDate,
                                                                                                         x.HokenNo,
                                                                                                         x.HokenEdaNo,
                                                                                                         x.HokenSname ?? string.Empty,
                                                                                                         x.Houbetu ?? string.Empty,
                                                                                                         x.HokenSbtKbn,
                                                                                                         x.CheckDigit,
                                                                                                         x.AgeStart,
                                                                                                         x.AgeEnd,
                                                                                                         x.IsFutansyaNoCheck,
                                                                                                         x.IsJyukyusyaNoCheck,
                                                                                                         x.JyukyuCheckDigit,
                                                                                                         x.IsTokusyuNoCheck,
                                                                                                         x.HokenName ?? string.Empty,
                                                                                                         x.HokenNameCd ?? string.Empty,
                                                                                                         x.HokenKohiKbn,
                                                                                                         x.IsOtherPrefValid,
                                                                                                         x.ReceKisai,
                                                                                                         x.IsLimitList,
                                                                                                         x.IsLimitListSum,
                                                                                                         x.EnTen,
                                                                                                         x.KaiLimitFutan,
                                                                                                         x.DayLimitFutan,
                                                                                                         x.MonthLimitFutan,
                                                                                                         x.MonthLimitCount,
                                                                                                         x.LimitKbn,
                                                                                                         x.CountKbn,
                                                                                                         x.FutanYusen,
                                                                                                         x.CalcSpKbn,
                                                                                                         x.MonthSpLimit,
                                                                                                         x.KogakuTekiyo,
                                                                                                         x.KogakuTotalKbn,
                                                                                                         x.KogakuHairyoKbn,
                                                                                                         x.ReceSeikyuKbn,
                                                                                                         x.ReceKisaiKokho,
                                                                                                         x.ReceKisai2,
                                                                                                         x.ReceTenKisai,
                                                                                                         x.ReceFutanRound,
                                                                                                         x.ReceZeroKisai,
                                                                                                         x.ReceSpKbn,
                                                                                                         string.Empty,
                                                                                                         x.PrefNo,
                                                                                                         x.SortNo)).ToList();
                hokenMsts.ForEach(x =>
                {
                    result.Add(new SelectMaintenanceModel(x));
                });
            }
            else
            {
                var hokenMaster = NoTrackingDataContext.HokenMsts.FirstOrDefault(u => u.HpId == hpId &&
                                                                                    u.HokenNo == hokenNo &&
                                                                                    u.HokenEdaNo == hokenEdaNo &&
                                                                                    (u.PrefNo == prefNo
                                                                                    || u.IsOtherPrefValid == 1) &&
                                                                                    u.StartDate <= startDate &&
                                                                                    u.EndDate >= startDate);

                if (hokenMaster == null)
                {
                    hokenMaster = NoTrackingDataContext.HokenMsts.FirstOrDefault(u => u.HpId == hpId &&
                                                                                    u.HokenNo == hokenNo &&
                                                                                    u.HokenEdaNo == hokenEdaNo &&
                                                                                    (u.PrefNo == prefNo
                                                                                    || u.IsOtherPrefValid == 1));
                }
                if (hokenMaster == null)
                    result.Add(new SelectMaintenanceModel(new HokenMstModel()));
                else
                    result.Add(new SelectMaintenanceModel(new HokenMstModel(hokenMaster.FutanKbn,
                                                                            hokenMaster.FutanRate,
                                                                            hokenMaster.StartDate,
                                                                            hokenMaster.EndDate,
                                                                            hokenMaster.HokenNo,
                                                                            hokenMaster.HokenEdaNo,
                                                                            hokenMaster.HokenSname ?? string.Empty,
                                                                            hokenMaster.Houbetu ?? string.Empty,
                                                                            hokenMaster.HokenSbtKbn,
                                                                            hokenMaster.CheckDigit,
                                                                            hokenMaster.AgeStart,
                                                                            hokenMaster.AgeEnd,
                                                                            hokenMaster.IsFutansyaNoCheck,
                                                                            hokenMaster.IsJyukyusyaNoCheck,
                                                                            hokenMaster.JyukyuCheckDigit,
                                                                            hokenMaster.IsTokusyuNoCheck,
                                                                            hokenMaster.HokenName ?? string.Empty,
                                                                            hokenMaster.HokenNameCd ?? string.Empty,
                                                                            hokenMaster.HokenKohiKbn,
                                                                            hokenMaster.IsOtherPrefValid,
                                                                            hokenMaster.ReceKisai,
                                                                            hokenMaster.IsLimitList,
                                                                            hokenMaster.IsLimitListSum,
                                                                            hokenMaster.EnTen,
                                                                            hokenMaster.KaiLimitFutan,
                                                                            hokenMaster.DayLimitFutan,
                                                                            hokenMaster.MonthLimitFutan,
                                                                            hokenMaster.MonthLimitCount,
                                                                            hokenMaster.LimitKbn,
                                                                            hokenMaster.CountKbn,
                                                                            hokenMaster.FutanYusen,
                                                                            hokenMaster.CalcSpKbn,
                                                                            hokenMaster.MonthSpLimit,
                                                                            hokenMaster.KogakuTekiyo,
                                                                            hokenMaster.KogakuTotalKbn,
                                                                            hokenMaster.KogakuHairyoKbn,
                                                                            hokenMaster.ReceSeikyuKbn,
                                                                            hokenMaster.ReceKisaiKokho,
                                                                            hokenMaster.ReceKisai2,
                                                                            hokenMaster.ReceTenKisai,
                                                                            hokenMaster.ReceFutanRound,
                                                                            hokenMaster.ReceZeroKisai,
                                                                            hokenMaster.ReceSpKbn,
                                                                            string.Empty,
                                                                            hokenMaster.PrefNo,
                                                                            hokenMaster.SortNo)));
            }

            return result;
        }

        public bool DeleteHokenMaster(int hpId, int hokenNo, int hokenEdaNo, int prefNo, int startDate)
        {
            var hokenMaster = TrackingDataContext.HokenMsts.FirstOrDefault(u => u.HpId == hpId &&
                                                                             u.HokenNo == hokenNo &&
                                                                             u.HokenEdaNo == hokenEdaNo &&
                                                                             u.PrefNo == prefNo &&
                                                                             u.StartDate == startDate);

            if (hokenMaster is null)
                return false;
            else
            {
                TrackingDataContext.HokenMsts.Remove(hokenMaster);
                return TrackingDataContext.SaveChanges() > 0;
            }
        }

        /// <summary>
        /// Item 1 is sortNO
        /// Item 2 is HokenEdaNo
        /// </summary>
        /// <param name="hpId"></param>
        /// <param name="hokenNo"></param>
        /// <param name="prefNo"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public (int sortNo, int hokenEdaNo) GetInfoCloneInsuranceMst(int hpId, int hokenNo, int prefNo, int startDate)
        {
            int sortNo = NoTrackingDataContext.HokenMsts.Where(u => u.HpId == hpId &&
                                                               u.HokenNo == hokenNo &&
                                                               u.PrefNo == prefNo &&
                                                               u.StartDate == startDate).Max(u => u.SortNo) + 1;


            int hokenEdaNo = NoTrackingDataContext.HokenMsts.Where(u => u.HpId == hpId &&
                                                                   u.HokenNo == hokenNo &&
                                                                   u.PrefNo == prefNo &&
                                                                   u.StartDate == startDate).Max(u => u.HokenEdaNo) + 1;

            return (sortNo, hokenEdaNo);
        }
    }
}


