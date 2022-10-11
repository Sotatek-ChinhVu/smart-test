using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Helper.Constants.TodayOrderConst;

namespace Domain.Types
{
    public class ValidateIOdrInfModel<TOdrInf, TOdrDetail>
    {
        protected KeyValuePair<string, TodayOrdValidationStatus> Validation(TOdrInf odrInf, int flag)
        {
            #region Validate common
            if (odrInf.InoutKbn != 0 && odrInf.InoutKbn != 1)
            {
                return new("-1", TodayOrdValidationStatus.InvalidInoutKbn);
            }
            if (DaysCnt < 0)
            {
                return new("-1", TodayOrdValidationStatus.InvalidDaysCnt);
            }

            if (flag == 0)
            {
                if (Id < 0)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidId);
                }
                if (HpId <= 0)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidHpId);
                }
                if ((Id > 0 && RpNo <= 0) || (Id == 0 && RpNo != 0))
                {
                    return new("-1", TodayOrdValidationStatus.InvalidRpNo);
                }
                if ((Id > 0 && RpNo <= 0) || (Id == 0 && RpNo != 0))
                {
                    return new("-1", TodayOrdValidationStatus.InvalidRpEdaNo);
                }
                if (RpName.Length > 240)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidRpName);
                }
                if (SikyuKbn != 0 && SikyuKbn != 1)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidSikyuKbn);
                }
                if (SyohoSbt != 0 && SyohoSbt != 1 && SyohoSbt != 2)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidSyohoSbt);
                }
                if (SanteiKbn != 0 && SanteiKbn != 1 && SanteiKbn != 2)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidSanteiKbn);
                }
                if (TosekiKbn != 0 && TosekiKbn != 1 && TosekiKbn != 2)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidTosekiKbn);
                }
                if (SortNo <= 0)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidSortNo);
                }
                if (IsDeleted != 0 && IsDeleted != 1 && IsDeleted != 2)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidIsDeleted);
                }
            }
            #endregion

            #region Validate business
            if (!OdrKouiKbns.ContainsValue(OdrKouiKbn))
            {
                return new("-1", TodayOrdValidationStatus.InvalidOdrKouiKbn);
            }
            if (OrdInfDetails.Any(o => o.IsSpecialItem))
            {
                var countItem = OrdInfDetails.FirstOrDefault(item => (item.IsDrug || item.IsInjection) && !item.IsSpecialItem);
                if (OrdInfDetails.Any(x => x.IsSpecialItem && x.ItemCd == ItemCdConst.ZanGigi || x.ItemCd == ItemCdConst.ZanTeiKyo) && countItem != null && countItem.ItemCd == ItemCdConst.Con_Refill)
                {
                    countItem = OrdInfDetails.FirstOrDefault(item => (item.IsDrug || item.IsInjection) && !item.IsSpecialItem && item.ItemCd != ItemCdConst.Con_Refill);
                }
                // Check main usage of drug and injection
                var countUsage = OrdInfDetails.FirstOrDefault(item => item.IsStandardUsage || item.IsInjectionUsage);

                // check supp usage of drug
                var countUsage2 = OrdInfDetails.FirstOrDefault(item => item.IsSuppUsage);

                if (countItem != null)
                {
                    var countIndex = OrdInfDetails.FindIndex(od => od == countItem);

                    return new(countIndex.ToString(), TodayOrdValidationStatus.InvalidSpecialItem);
                }
                else if (countUsage != null)
                {
                    var countUsage1Index = OrdInfDetails.FindIndex(od => od == countUsage);

                    return new(countUsage1Index.ToString(), TodayOrdValidationStatus.InvalidSpecialStadardUsage);
                }
                else if (countUsage2 != null)
                {
                    var countUsage2Index = OrdInfDetails.FindIndex(od => od == countUsage2);

                    return new(countUsage2Index.ToString(), TodayOrdValidationStatus.InvalidSpecialSuppUsage);
                }
            }

            if (OrdInfDetails?.Count(d => d.IsDrugUsage) > 0
                && OrdInfDetails?.Count(d => d.IsDrug || (d.SinKouiKbn == 20 && d.ItemCd.StartsWith("Z"))) == 0)
            {
                return new("-1", TodayOrdValidationStatus.InvalidHasUsageButNotDrug);
            }

            if (OrdInfDetails?.Count(d => d.IsInjectionUsage) > 0
              && OrdInfDetails?.Count(d => d.IsInjection || d.IsDrug) == 0)
            {
                return new("-1", TodayOrdValidationStatus.InvalidHasUsageButNotInjectionOrDrug);
            }

            if (IsDrug)
            {
                var drugUsage = OrdInfDetails?.LastOrDefault(d => d.IsDrugUsage);
                if (drugUsage != null)
                {
                    var drugAfterDrugUsage = OrdInfDetails?.Where(d => d.RowNo > drugUsage.RowNo && d.IsDrug && !d.IsEmpty).ToList();
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
            else if (IsInjection)
            {
                var injectionUsage = OrdInfDetails?.FirstOrDefault(d => d.IsInjectionUsage);
                if (injectionUsage != null)
                {
                    var injectionBeforeInjectionUsage = OrdInfDetails?.Where(d => d.RowNo < injectionUsage.RowNo && (d.IsInjection || d.IsDrug) && !d.IsEmpty).ToList();
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
            else if (OdrKouiKbn == 28)
            {
                var seflInjection = OrdInfDetails?.FirstOrDefault(d => d.ItemCd == ItemCdConst.ChusyaJikocyu);
                var usageCount = OrdInfDetails?.Count(d => d.IsInjectionUsage);
                if (seflInjection == null && usageCount == 0)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidHasNotBothInjectionAndUsageOf28);
                }
            }

            if (IsDrug || IsInjection)
            {
                // 用法
                var usageCount = OrdInfDetails?.Count(o => o.IsStandardUsage);
                if (usageCount > 1)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidStandardUsageOfDrugOrInjection);
                }

                // 補助用法
                var usage2Count = OrdInfDetails?.Count(item => item.IsSuppUsage);
                if (usage2Count > 1)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidSuppUsageOfDrugOrInjection);
                }
            }

            if (IsDrug)
            {
                int bunkatuItemCount = OrdInfDetails?.Count(i => i.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu) ?? 0;

                if (bunkatuItemCount > 1)
                {
                    return new("-1", TodayOrdValidationStatus.InvalidBunkatu);
                }

                var bunkatuItem = OrdInfDetails?.FirstOrDefault(i => i.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu);
                if (bunkatuItem != null)
                {
                    var usageItem = OrdInfDetails?.FirstOrDefault(item => item.IsStandardUsage);

                    if (usageItem == null)
                    {
                        var usageIndex = OrdInfDetails?.FindIndex(od => od == bunkatuItem) ?? 0;

                        return new(usageIndex.ToString(), TodayOrdValidationStatus.InvalidUsageWhenBuntakuNull);
                    }

                    var sumBukatu = SumBunkatu(bunkatuItem?.Bunkatu ?? string.Empty);

                    if (usageItem.Suryo != sumBukatu)
                    {
                        var usageIndex = OrdInfDetails?.FindIndex(od => od == usageItem) ?? 0;

                        return new(usageIndex.ToString(), TodayOrdValidationStatus.InvalidSumBunkatuDifferentSuryo);
                    }
                }
            }
            if (OrdInfDetails?.Count > 0)
            {
                var count = 0;
                foreach (var ordInfDetail in OrdInfDetails)
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
                if (OrdInfDetails?.Count == 1)
                {
                    var item = OrdInfDetails[0];

                    if (item.ItemCd == ItemCdConst.GazoDensibaitaiHozon)
                    {
                        return new("-1", TodayOrdValidationStatus.InvalidGazoDensibaitaiHozon);
                    }
                }

                var specialItems = OrdInfDetails?.Where(item => item.IsSpecialItem && item.ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu);

                var countItems = OrdInfDetails?.Where(item => (item.SinKouiKbn == 20 || item.SinKouiKbn == 30 || item.ItemCd == ItemCdConst.TouyakuChozaiNaiTon || item.ItemCd == ItemCdConst.TouyakuChozaiGai) && !item.IsSpecialItem);

                if (specialItems?.Any(x => x.ItemCd == ItemCdConst.ZanGigi || x.ItemCd == ItemCdConst.ZanTeiKyo) == true && countItems?.Any(x => x.ItemCd == ItemCdConst.Con_Refill) == true)
                {
                    countItems = OrdInfDetails?.Where(item => (item.SinKouiKbn == 20 || item.SinKouiKbn == 30 || item.ItemCd == ItemCdConst.TouyakuChozaiNaiTon || item.ItemCd == ItemCdConst.TouyakuChozaiGai) && !item.IsSpecialItem && item.ItemCd != ItemCdConst.Con_Refill);
                }

                var countUsage = OrdInfDetails?.Count(item => item.SinKouiKbn == 28 || item.IsStandardUsage || item.IsInjectionUsage) ?? 0;

                if (countItems?.Count() > 0 && countUsage == 0 && (IsDrug || OdrKouiKbn == 20))
                {
                    return new("-1", TodayOrdValidationStatus.InvalidHasDrugButNotUsage);
                }

                if (countItems?.Count() > 0 && countUsage == 0 && (IsInjection || OdrKouiKbn == 28))
                {
                    return new("-1", TodayOrdValidationStatus.InvalidHasInjectionButNotUsage);
                }

                if (countUsage == 1 && countItems?.Count() == 0)
                {
                    var tokuzaiCount = OrdInfDetails?.Count(item => item.MasterSbt.AsString().ToUpper() == "T") ?? 0;

                    if (tokuzaiCount == 0)
                    {
                        return new("-1", TodayOrdValidationStatus.InvalidTokuzai);
                    }
                }

                var tokuzaiFirstCount = OrdInfDetails?.Count(item => item.MasterSbt.AsString().ToUpper() == "T") ?? 0;

                if (tokuzaiFirstCount > 0)
                {
                    if (OdrKouiKbn == 0)
                    {
                        return new("-1", TodayOrdValidationStatus.InvalidTokuzaiKouiKbn);
                    }

                    if (countItems?.Count() == 0 && (IsDrug || OdrKouiKbn == 20 || IsInjection))
                    {
                        return new("-1", TodayOrdValidationStatus.InvalidTokuzaiDrugOrInjection);
                    }
                }

            }
            #endregion

            return new("-1", TodayOrdValidationStatus.Valid);
        }
    }
}
