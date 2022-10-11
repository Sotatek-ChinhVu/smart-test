using Helper.Constants;
using Helper.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Helper.Constants.TodayOrderConst;

namespace Domain.Types
{
    public class ValidateIOdrInfModel<TOdrInf, TOdrInfDetailModel> where TOdrInf : class, IOdrInfModel<TOdrInfDetailModel> where TOdrInfDetailModel : class, IOdrInfDetailModel
    {
        protected KeyValuePair<string, TodayOrdValidationStatus> Validation(TOdrInf odrInf, int flag)
        {
            #region Validate common
            if (odrInf.InoutKbn != 0 && odrInf.InoutKbn != 1)
            {
                return new("-1", TodayOrdValidationStatus.InvalidInoutKbn);
            }
            if (odrInf.DaysCnt < 0)
            {
                return new("-1", TodayOrdValidationStatus.InvalidDaysCnt);
            }

            if (flag == 0)
            {
                if (odrInf.Id < 0)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidId);
                }
                if (odrInf.HpId <= 0)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidHpId);
                }
                if ((odrInf.Id > 0 && odrInf.RpNo <= 0) || (odrInf.Id == 0 && odrInf.RpNo != 0))
                {
                    return new("-1", TodayOrdValidationStatus.InvalidRpNo);
                }
                if ((odrInf.Id > 0 && odrInf.RpNo <= 0) || (odrInf.Id == 0 && odrInf.RpNo != 0))
                {
                    return new("-1", TodayOrdValidationStatus.InvalidRpEdaNo);
                }
                if (odrInf.RpName.Length > 240)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidRpName);
                }
                if (odrInf.SikyuKbn != 0 && odrInf.SikyuKbn != 1)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidSikyuKbn);
                }
                if (odrInf.SyohoSbt != 0 && odrInf.SyohoSbt != 1 && odrInf.SyohoSbt != 2)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidSyohoSbt);
                }
                if (odrInf.SanteiKbn != 0 && odrInf.SanteiKbn != 1 && odrInf.SanteiKbn != 2)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidSanteiKbn);
                }
                if (odrInf.TosekiKbn != 0 && odrInf.TosekiKbn != 1 && odrInf.TosekiKbn != 2)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidTosekiKbn);
                }
                if (odrInf.SortNo <= 0)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidSortNo);
                }
                if (odrInf.IsDeleted != 0 && odrInf.IsDeleted != 1 && odrInf.IsDeleted != 2)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidIsDeleted);
                }
            }
            #endregion

            #region Validate business
            if (!OdrKouiKbns.ContainsValue(odrInf.OdrKouiKbn))
            {
                return new("-1", TodayOrdValidationStatus.InvalidOdrKouiKbn);
            }
            if (odrInf.OrdInfDetails.Any(o => o.IsSpecialItem))
            {
                var countItem = odrInf.OrdInfDetails.FirstOrDefault(item => (item.IsDrug || item.IsInjection) && !item.IsSpecialItem);
                if (odrInf.OrdInfDetails.Any(x => x.IsSpecialItem && x.ItemCd == ItemCdConst.ZanGigi || x.ItemCd == ItemCdConst.ZanTeiKyo) && countItem != null && countItem.ItemCd == ItemCdConst.Con_Refill)
                {
                    countItem = odrInf.OrdInfDetails.FirstOrDefault(item => (item.IsDrug || item.IsInjection) && !item.IsSpecialItem && item.ItemCd != ItemCdConst.Con_Refill);
                }
                // Check main usage of drug and injection
                var countUsage = odrInf.OrdInfDetails.FirstOrDefault(item => item.IsStandardUsage || item.IsInjectionUsage);

                // check supp usage of drug
                var countUsage2 = odrInf.OrdInfDetails.FirstOrDefault(item => item.IsSuppUsage);

                if (countItem != null)
                {
                    var countIndex = odrInf.OrdInfDetails.FindIndex(od => od == countItem);

                    return new(countIndex.ToString(), TodayOrdValidationStatus.InvalidSpecialItem);
                }
                else if (countUsage != null)
                {
                    var countUsage1Index = odrInf.OrdInfDetails.FindIndex(od => od == countUsage);

                    return new(countUsage1Index.ToString(), TodayOrdValidationStatus.InvalidSpecialStadardUsage);
                }
                else if (countUsage2 != null)
                {
                    var countUsage2Index = odrInf.OrdInfDetails.FindIndex(od => od == countUsage2);

                    return new(countUsage2Index.ToString(), TodayOrdValidationStatus.InvalidSpecialSuppUsage);
                }
            }

            if (odrInf.OrdInfDetails?.Count(d => d.IsDrugUsage) > 0
                && odrInf.OrdInfDetails?.Count(d => d.IsDrug || (d.SinKouiKbn == 20 && d.ItemCd.StartsWith("Z"))) == 0)
            {
                return new("-1", TodayOrdValidationStatus.InvalidHasUsageButNotDrug);
            }

            if (odrInf.OrdInfDetails?.Count(d => d.IsInjectionUsage) > 0
              && odrInf.OrdInfDetails?.Count(d => d.IsInjection || d.IsDrug) == 0)
            {
                return new("-1", TodayOrdValidationStatus.InvalidHasUsageButNotInjectionOrDrug);
            }

            if (odrInf.IsDrug)
            {
                var drugUsage = odrInf.OrdInfDetails?.LastOrDefault(d => d.IsDrugUsage);
                if (drugUsage != null)
                {
                    var drugAfterDrugUsage = odrInf.OrdInfDetails?.Where(d => d.RowNo > drugUsage.RowNo && d.IsDrug && !d.IsEmpty).ToList();
                    if (drugAfterDrugUsage?.Any() == true)
                    {
                        var count = 0;
                        foreach (var detail in drugAfterDrugUsage)
                        {
                            var validateResult = detail.Validation(0);
                            if (validateResult != TodayOrdValidationStatus.Valid) return new(count.ToString(), validateResult);
                            count++;
                        }
                    }
                }
                else
                {
                    return new("-1", TodayOrdValidationStatus.InvalidHasDrugButNotUsage);
                }
            }
            else if (odrInf.IsInjection)
            {
                var injectionUsage = odrInf.OrdInfDetails?.FirstOrDefault(d => d.IsInjectionUsage);
                if (injectionUsage != null)
                {
                    var injectionBeforeInjectionUsage = odrInf.OrdInfDetails?.Where(d => d.RowNo < injectionUsage.RowNo && (d.IsInjection || d.IsDrug) && !d.IsEmpty).ToList();
                    injectionBeforeInjectionUsage?.Reverse();
                    if (injectionBeforeInjectionUsage?.Any() == true)
                    {
                        var count = 0;
                        foreach (var detail in injectionBeforeInjectionUsage)
                        {
                            var validateResult = detail.Validation(flag);
                            if (TodayOrdValidationStatus.Valid != validateResult) return new(count.ToString(), validateResult);
                            count++;
                        }
                    }
                }
                else
                {
                    return new("-1", TodayOrdValidationStatus.InvalidHasInjectionButNotUsage);
                }
            }
            else if (odrInf.OdrKouiKbn == 28)
            {
                var seflInjection = odrInf.OrdInfDetails?.FirstOrDefault(d => d.ItemCd == ItemCdConst.ChusyaJikocyu);
                var usageCount = odrInf.OrdInfDetails?.Count(d => d.IsInjectionUsage);
                if (seflInjection == null && usageCount == 0)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidHasNotBothInjectionAndUsageOf28);
                }
            }

            if (odrInf.IsDrug || odrInf.IsInjection)
            {
                // 用法
                var usageCount = odrInf.OrdInfDetails?.Count(o => o.IsStandardUsage);
                if (usageCount > 1)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidStandardUsageOfDrugOrInjection);
                }

                // 補助用法
                var usage2Count = odrInf.OrdInfDetails?.Count(item => item.IsSuppUsage);
                if (usage2Count > 1)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidSuppUsageOfDrugOrInjection);
                }
            }

            if (odrInf.IsDrug)
            {
                int bunkatuItemCount = odrInf.OrdInfDetails?.Count(i => i.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu) ?? 0;

                if (bunkatuItemCount > 1)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidBunkatu);
                }

                var bunkatuItem = odrInf.OrdInfDetails?.FirstOrDefault(i => i.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu);
                if (bunkatuItem != null)
                {
                    var usageItem = odrInf.OrdInfDetails?.FirstOrDefault(item => item.IsStandardUsage);

                    if (usageItem == null)
                    {
                        var usageIndex = odrInf.OrdInfDetails?.FindIndex(od => od == bunkatuItem) ?? 0;

                        return new(usageIndex.ToString(), TodayOrdValidationStatus.InvalidUsageWhenBuntakuNull);
                    }

                    var sumBukatu = odrInf.SumBunkatu(bunkatuItem?.Bunkatu ?? string.Empty);

                    if (usageItem.Suryo != sumBukatu)
                    {
                        var usageIndex = odrInf.OrdInfDetails?.FindIndex(od => od == usageItem) ?? 0;

                        return new(usageIndex.ToString(), TodayOrdValidationStatus.InvalidSumBunkatuDifferentSuryo);
                    }
                }
            }
            if (odrInf.OrdInfDetails?.Count > 0)
            {
                var count = 0;
                foreach (var ordInfDetail in odrInf.OrdInfDetails)
                {
                    var status = ordInfDetail.Validation(flag);
                    if (status != TodayOrdValidationStatus.Valid)
                    {
                        if (status == TodayOrdValidationStatus.InvalidSuryoOfReffill)
                            return new(count.ToString() + "_" + ordInfDetail.RefillSetting, status);

                        return new(count.ToString(), status);
                    }
                    count++;
                }
            }

            if (flag == 1)
            {
                if (odrInf.OrdInfDetails?.Count == 1)
                {
                    var item = odrInf.OrdInfDetails[0];

                    if (item.ItemCd == ItemCdConst.GazoDensibaitaiHozon)
                    {
                        return new("-1", TodayOrdValidationStatus.InvalidGazoDensibaitaiHozon);
                    }
                }

                var specialItems = odrInf.OrdInfDetails?.Where(item => item.IsSpecialItem && item.ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu);

                var countItems = odrInf.OrdInfDetails?.Where(item => (item.SinKouiKbn == 20 || item.SinKouiKbn == 30 || item.ItemCd == ItemCdConst.TouyakuChozaiNaiTon || item.ItemCd == ItemCdConst.TouyakuChozaiGai) && !item.IsSpecialItem);

                if (specialItems?.Any(x => x.ItemCd == ItemCdConst.ZanGigi || x.ItemCd == ItemCdConst.ZanTeiKyo) == true && countItems?.Any(x => x.ItemCd == ItemCdConst.Con_Refill) == true)
                {
                    countItems = odrInf.OrdInfDetails?.Where(item => (item.SinKouiKbn == 20 || item.SinKouiKbn == 30 || item.ItemCd == ItemCdConst.TouyakuChozaiNaiTon || item.ItemCd == ItemCdConst.TouyakuChozaiGai) && !item.IsSpecialItem && item.ItemCd != ItemCdConst.Con_Refill);
                }

                var countUsage = odrInf.OrdInfDetails?.Count(item => item.SinKouiKbn == 28 || item.IsStandardUsage || item.IsInjectionUsage) ?? 0;

                if (countItems?.Count() > 0 && countUsage == 0 && (odrInf.IsDrug || odrInf.OdrKouiKbn == 20))
                {
                    return new("-1", TodayOrdValidationStatus.InvalidHasDrugButNotUsage);
                }

                if (countItems?.Count() > 0 && countUsage == 0 && (odrInf.IsInjection || odrInf.OdrKouiKbn == 28))
                {
                    return new("-1", TodayOrdValidationStatus.InvalidHasInjectionButNotUsage);
                }

                if (countUsage == 1 && countItems?.Count() == 0)
                {
                    var tokuzaiCount = odrInf.OrdInfDetails?.Count(item => item.MasterSbt.AsString().ToUpper() == "T") ?? 0;

                    if (tokuzaiCount == 0)
                    {
                        return new("-1", TodayOrdValidationStatus.InvalidTokuzai);
                    }
                }

                var tokuzaiFirstCount = odrInf.OrdInfDetails?.Count(item => item.MasterSbt.AsString().ToUpper() == "T") ?? 0;

                if (tokuzaiFirstCount > 0)
                {
                    if (odrInf.OdrKouiKbn == 0)
                    {
                        return new("-1", TodayOrdValidationStatus.InvalidTokuzaiKouiKbn);
                    }

                    if (countItems?.Count() == 0 && (odrInf.IsDrug || odrInf.OdrKouiKbn == 20 || odrInf.IsInjection))
                    {
                        return new("-1", TodayOrdValidationStatus.InvalidTokuzaiDrugOrInjection);
                    }
                }

            }
            #endregion

            return new("-1", TodayOrdValidationStatus.Valid);
        }

        public TodayOrdValidationStatus ValidationDetail(TOdrInfDetailModel odrInfDetail, int flag)
        {
            #region Validate common

            if (odrInfDetail.RowNo <= 0)
            {
                return TodayOrdValidationStatus.InvalidRowNo;
            }
            if (odrInfDetail.SinKouiKbn < 0)
            {
                return TodayOrdValidationStatus.InvalidSinKouiKbn;
            }
            if (odrInfDetail.ItemCd.Length > 10)
            {
                return TodayOrdValidationStatus.InvalidItemCd;
            }
            if (odrInfDetail.ItemName.Length > 240)
            {
                return TodayOrdValidationStatus.InvalidItemName;
            }
            if (odrInfDetail.Suryo < 0 || (odrInfDetail.ItemCd == ItemCdConst.JikanKihon && !(odrInfDetail.Suryo >= 0 && odrInfDetail.Suryo <= 7)) || (odrInfDetail.ItemCd == ItemCdConst.SyosaiKihon && !(odrInfDetail.Suryo >= 0 && odrInfDetail.Suryo <= 8)))
            {
                return TodayOrdValidationStatus.InvalidSuryo;
            }
            if (UnitName.Length > 24)
            {
                return TodayOrdValidationStatus.InvalidUnitName;
            }
            if (!(SyohoKbn >= 0 && SyohoKbn <= 3))
            {
                return TodayOrdValidationStatus.InvalidSyohoKbn;
            }
            if (!(YohoKbn >= 0 && YohoKbn <= 2))
            {
                return TodayOrdValidationStatus.InvalidYohoKbn;
            }
            if (Bunkatu.Length > 10)
            {
                return TodayOrdValidationStatus.InvalidBunkatuLength;
            }
            if (CmtName.Length > 240)
            {
                return TodayOrdValidationStatus.InvalidCmtName;
            }
            if (CmtOpt.Length > 38)
            {
                return TodayOrdValidationStatus.InvalidCmtOpt;
            }

            if (flag == 0)
            {
                if (HpId <= 0)
                {
                    return TodayOrdValidationStatus.InvalidHpId;
                }
                if (RaiinNo <= 0)
                {
                    return TodayOrdValidationStatus.InvalidRaiinNo;
                }
                if (RpNo <= 0)
                {
                    return TodayOrdValidationStatus.InvalidRpNo;
                }
                if (RpEdaNo <= 0)
                {
                    return TodayOrdValidationStatus.InvalidRpEdaNo;
                }
                if (RowNo <= 0)
                {
                    return TodayOrdValidationStatus.InvalidRowNo;
                }
                if (PtId <= 0)
                {
                    return TodayOrdValidationStatus.InvalidPtId;
                }
                if (SinDate <= 0)
                {
                    return TodayOrdValidationStatus.InvalidSinDate;
                }
                if (SinKouiKbn < 0)
                {
                    return TodayOrdValidationStatus.InvalidSinKouiKbn;
                }
                if (ItemCd.Length > 10)
                {
                    return TodayOrdValidationStatus.InvalidItemCd;
                }
                if (ItemName.Length > 240)
                {
                    return TodayOrdValidationStatus.InvalidItemName;
                }
                if (Suryo < 0)
                {
                    return TodayOrdValidationStatus.InvalidSuryo;
                }
                if (UnitName.Length > 24)
                {
                    return TodayOrdValidationStatus.InvalidUnitName;
                }
                if (UnitSbt != 0 && UnitSbt != 1 && UnitSbt != 2)
                {
                    return TodayOrdValidationStatus.InvalidUnitSbt;
                }
                if (TermVal < 0)
                {
                    return TodayOrdValidationStatus.InvalidTermVal;
                }
                if (!(SyohoKbn >= 0 && SyohoKbn <= 3))
                {
                    return TodayOrdValidationStatus.InvalidSyohoKbn;
                }
                if (!(SyohoLimitKbn >= 0 && SyohoLimitKbn <= 3))
                {
                    return TodayOrdValidationStatus.InvalidSyohoLimitKbn;
                }
                if (!(YohoKbn >= 0 && YohoKbn <= 2))
                {
                    return TodayOrdValidationStatus.InvalidYohoKbn;
                }
                if (!(IsNodspRece >= 0 && IsNodspRece <= 1))
                {
                    return TodayOrdValidationStatus.InvalidIsNodspRece;
                }
                if (IpnCd.Length > 12)
                {
                    return TodayOrdValidationStatus.InvalidIpnCd;
                }
                if (IpnName.Length > 120)
                {
                    return TodayOrdValidationStatus.InvalidIpnName;
                }
                if (!(JissiKbn >= 0 && JissiKbn <= 1))
                {
                    return TodayOrdValidationStatus.InvalidJissiKbn;
                }
                if (JissiId < 0)
                {
                    return TodayOrdValidationStatus.InvalidJissiId;
                }
                if (JissiMachine.Length > 60)
                {
                    return TodayOrdValidationStatus.InvalidJissiMachine;
                }
                if (ReqCd.Length > 10)
                {
                    return TodayOrdValidationStatus.InvalidReqCd;
                }
                if (Bunkatu.Length > 10)
                {
                    return TodayOrdValidationStatus.InvalidBunkatuLength;
                }
                if (CmtName.Length > 240)
                {
                    return TodayOrdValidationStatus.InvalidCmtName;
                }
                if (CmtOpt.Length > 38)
                {
                    return TodayOrdValidationStatus.InvalidCmtOpt;
                }
                if (FontColor.Length > 8)
                {
                    return TodayOrdValidationStatus.InvalidFontColor;
                }
                if (!(CommentNewline >= 0 && CommentNewline <= 1))
                {
                    return TodayOrdValidationStatus.InvalidCommentNewline;
                }
            }
            #endregion

            #region Validate business

            if ((!string.IsNullOrEmpty(UnitName) && Suryo == 0) || (string.IsNullOrEmpty(UnitName) && Suryo > 0))
            {
                return TodayOrdValidationStatus.InvalidSuryo;
            }
            if (!KohatuKbns.ContainsValue(KohatuKbn))
            {
                return TodayOrdValidationStatus.InvalidKohatuKbn;
            }

            if (!DrugKbns.ContainsValue(DrugKbn))
            {
                return TodayOrdValidationStatus.InvalidDrugKbn;
            }

            if (!string.IsNullOrWhiteSpace(DisplayedUnit))
            {
                if (string.IsNullOrEmpty(DisplayedQuantity))
                {
                    return TodayOrdValidationStatus.InvalidQuantityUnit;
                }
                else if (Suryo > 0 && (Price > 0 && Suryo * Price > 999999999))
                {
                    return TodayOrdValidationStatus.InvalidPrice;
                }
            }

            if (!string.IsNullOrWhiteSpace(DisplayedUnit) && (YohoKbn == 1 && Suryo > 999))
            {
                return TodayOrdValidationStatus.InvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull;
            }

            if (ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu && (Suryo == 0 && string.IsNullOrWhiteSpace(Bunkatu)))
            {
                return TodayOrdValidationStatus.InvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu;
            }

            if (ItemCd == ItemCdConst.Con_Refill && Suryo > RefillSetting)
            {
                return TodayOrdValidationStatus.InvalidSuryoOfReffill;
            }

            if (Is840Cmt && (CmtCol1 > 0 && (string.IsNullOrEmpty(CmtOpt) || string.IsNullOrEmpty(CmtName))))
            {
                return TodayOrdValidationStatus.InvalidCmt840;
            }

            if (Is842Cmt)
            {
                if (string.IsNullOrEmpty(CmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    return TodayOrdValidationStatus.InvalidCmt842;
                }
                else if (!string.IsNullOrEmpty(CmtOpt) && CmtOpt.Length > 38)
                {
                    return TodayOrdValidationStatus.InvalidCmt842CmtOptMoreThan38;
                }
            }

            if (Is830Cmt)
            {
                string fullSpace = @"　";

                if (string.IsNullOrEmpty(CmtOpt) && CmtOpt != fullSpace)
                {
                    return TodayOrdValidationStatus.InvalidCmt830CmtOpt;
                }
                else if (!string.IsNullOrEmpty(CmtOpt) && CmtOpt.Length > 38)
                {
                    return TodayOrdValidationStatus.InvalidCmt830CmtOptMoreThan38;
                }
            }

            if (Is831Cmt && (string.IsNullOrEmpty(CmtOpt) || string.IsNullOrEmpty(CmtName)))
            {
                return TodayOrdValidationStatus.InvalidCmt831;
            }

            if (Is850Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt850(CmtOpt, ItemName);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    if (CmtName.Contains('日'))
                    {
                        return TodayOrdValidationStatus.InvalidCmt850Date;
                    }
                    else
                    {
                        return TodayOrdValidationStatus.InvalidCmt850OtherDate;
                    }
                }
            }

            if (Is851Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt851(CmtOpt);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    return TodayOrdValidationStatus.InvalidCmt851;
                }
            }

            if (Is852Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt852(CmtOpt);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    return TodayOrdValidationStatus.InvalidCmt852;
                }
            }

            if (Is853Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt853(CmtOpt, SinDate);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    return TodayOrdValidationStatus.InvalidCmt853;
                }
            }

            if (Is880Cmt && (string.IsNullOrEmpty(CmtOpt) || string.IsNullOrEmpty(CmtName)))
            {
                return TodayOrdValidationStatus.InvalidCmt880;
            }
            #endregion

            return TodayOrdValidationStatus.Valid;
        }
    }
}
