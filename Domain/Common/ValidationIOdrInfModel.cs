using Domain.Types;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using static Helper.Constants.OrderInfConst;

namespace Domain.Common
{
    public static class ValidationIOdrInfModel<TOdrInf, TOdrInfDetailModel> where TOdrInf : class, IOdrInfModel<TOdrInfDetailModel> where TOdrInfDetailModel : class, IOdrInfDetailModel
    {
        private const string odrValidateCode = "-1";

        public static KeyValuePair<string, OrdInfValidationStatus> Validation(TOdrInf odrInf, int flag, int sinDate = 0, int refillSetting = 999)
        {

            var validateCommon = ValidateCommon(odrInf, flag);
            if (validateCommon != OrdInfValidationStatus.Valid)
            {
                return new(odrValidateCode, validateCommon);
            }

            #region Validate business
            if (odrInf.OrdInfDetails.Any(o => o.IsSpecialItem))
            {
                var validateSpecialItem = ValidateSpecialItem(odrInf);
                if (validateSpecialItem.Value != OrdInfValidationStatus.Valid)
                {
                    return validateSpecialItem;
                }
            }

            var validateUsageOfSelfInjection = ValidateUsageOfSelfInjection(odrInf);
            if (validateUsageOfSelfInjection.Value != OrdInfValidationStatus.Valid)
            {
                return validateUsageOfSelfInjection;
            }

            var validateHasUsageButNotHasDrugOrInjection = ValidateHasUsageButNotHasDrugOrInjection(odrInf);
            if (validateHasUsageButNotHasDrugOrInjection.Value != OrdInfValidationStatus.Valid)
            {
                return validateHasUsageButNotHasDrugOrInjection;
            }

            var validateHasNotUsageOfDrug = ValidateHasNotUsageOfDrug(odrInf, flag, sinDate, refillSetting);
            if (validateHasNotUsageOfDrug.Value != OrdInfValidationStatus.Valid)
            {
                return validateHasNotUsageOfDrug;
            }

            var validateHasNotUsageOfInjection = ValidateHasNotUsageOfInjection(odrInf, flag, sinDate, refillSetting);
            if (validateHasNotUsageOfInjection.Value != OrdInfValidationStatus.Valid)
            {
                return validateHasNotUsageOfInjection;
            }

            if (odrInf.IsDrug || odrInf.IsInjection)
            {
                // 用法
                var usageCount = odrInf.OrdInfDetails?.Count(o => o.IsStandardUsage);
                if (usageCount > 1)
                {
                    return new(odrValidateCode, OrdInfValidationStatus.InvalidStandardUsageOfDrugOrInjection);
                }

                // 補助用法
                var usage2Count = odrInf.OrdInfDetails?.Count(item => item.IsSuppUsage);
                if (usage2Count > 1)
                {
                    return new(odrValidateCode, OrdInfValidationStatus.InvalidSuppUsageOfDrugOrInjection);
                }
            }

            var validateBunkatu = ValidateBunkatu(odrInf);
            if (validateBunkatu.Value != OrdInfValidationStatus.Valid)
            {
                return validateBunkatu;
            }

            if (odrInf.OrdInfDetails?.Count > 0)
            {
                var count = 0;
                foreach (var ordInfDetail in odrInf.OrdInfDetails)
                {
                    var status = ValidationDetail(ordInfDetail, flag, sinDate, refillSetting);
                    if (status != OrdInfValidationStatus.Valid)
                    {
                        if (status == OrdInfValidationStatus.InvalidSuryoOfReffill)
                            return new(count.ToString() + "_" + refillSetting, status);

                        return new(count.ToString(), status);
                    }
                    count++;
                }
            }

            if (flag == 1)
            {
                var validateInputItemBussiness = ValidateInputItemBussiness(odrInf);
                if (validateInputItemBussiness.Value != OrdInfValidationStatus.Valid)
                {
                    return validateInputItemBussiness;
                }
            }
            #endregion

            return new(odrValidateCode, OrdInfValidationStatus.Valid);
        }

        private static OrdInfValidationStatus ValidationDetail(TOdrInfDetailModel odrInfDetail, int flag, int sinDate, int refillSetting)
        {
            #region Validate common
            var validateCommonDetail = ValidateCommonDetail(odrInfDetail);
            if (validateCommonDetail != OrdInfValidationStatus.Valid)
            {
                return validateCommonDetail;
            }

            if (flag == 0)
            {
                var validateCommonDetailInputItem = ValidateCommonDetailInputItem(odrInfDetail);
                if (validateCommonDetailInputItem != OrdInfValidationStatus.Valid)
                {
                    return validateCommonDetailInputItem;
                }
            }
            #endregion

            #region Validate business

            if (!string.IsNullOrEmpty(odrInfDetail.UnitName) && odrInfDetail.Suryo == 0 || string.IsNullOrEmpty(odrInfDetail.UnitName) && odrInfDetail.Suryo > 0)
            {
                return OrdInfValidationStatus.InvalidSuryo;
            }
            if (!KohatuKbns.ContainsValue(odrInfDetail.KohatuKbn))
            {
                return OrdInfValidationStatus.InvalidKohatuKbn;
            }

            if (!DrugKbns.ContainsValue(odrInfDetail.DrugKbn))
            {
                return OrdInfValidationStatus.InvalidDrugKbn;
            }

            if (!string.IsNullOrWhiteSpace(odrInfDetail.DisplayedUnit))
            {
                if (string.IsNullOrEmpty(odrInfDetail.DisplayedQuantity))
                {
                    return OrdInfValidationStatus.InvalidQuantityUnit;
                }
                else if (odrInfDetail.Suryo > 0 && odrInfDetail.Price > 0 && odrInfDetail.Suryo * odrInfDetail.Price > 999999999)
                {
                    return OrdInfValidationStatus.InvalidPrice;
                }
            }

            if (!string.IsNullOrWhiteSpace(odrInfDetail.DisplayedUnit) && odrInfDetail.YohoKbn == 1 && odrInfDetail.Suryo > 999)
            {
                return OrdInfValidationStatus.InvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull;
            }

            if (odrInfDetail.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu && odrInfDetail.Suryo == 0 && string.IsNullOrWhiteSpace(odrInfDetail.Bunkatu))
            {
                return OrdInfValidationStatus.InvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu;
            }

            if (odrInfDetail.ItemCd == ItemCdConst.Con_Refill && odrInfDetail.Suryo > refillSetting)
            {
                return OrdInfValidationStatus.InvalidSuryoOfReffill;
            }

            var validateCmtDetail = ValidateCmtDetail(sinDate, odrInfDetail);
            if (validateCmtDetail != OrdInfValidationStatus.Valid)
            {
                return validateCmtDetail;
            }
            #endregion

            return OrdInfValidationStatus.Valid;
        }

        #region Seperate Validate OdrInf
        private static OrdInfValidationStatus ValidateCommon(TOdrInf odrInf, int flag)
        {
            if (odrInf.InoutKbn != 0 && odrInf.InoutKbn != 1)
            {
                return OrdInfValidationStatus.InvalidInoutKbn;
            }
            if (odrInf.DaysCnt < 0)
            {
                return OrdInfValidationStatus.InvalidDaysCnt;
            }
            if (!OdrKouiKbns.ContainsValue(odrInf.OdrKouiKbn))
            {
                return OrdInfValidationStatus.InvalidOdrKouiKbn;
            }

            if (flag == 0)
            {
                if (odrInf.Id < 0)
                {
                    return OrdInfValidationStatus.InvalidId;
                }
                if (odrInf.HpId <= 0)
                {
                    return OrdInfValidationStatus.InvalidHpId;
                }
                if (odrInf.Id > 0 && odrInf.RpNo <= 0 || odrInf.Id == 0 && odrInf.RpNo != 0)
                {
                    return OrdInfValidationStatus.InvalidRpNo;
                }
                if (odrInf.Id > 0 && odrInf.RpEdaNo <= 0 || odrInf.Id == 0 && odrInf.RpEdaNo != 0)
                {
                    return OrdInfValidationStatus.InvalidRpEdaNo;
                }
                if (odrInf.RpName.Length > 240)
                {
                    return OrdInfValidationStatus.InvalidRpName;
                }
                if (odrInf.SikyuKbn != 0 && odrInf.SikyuKbn != 1)
                {
                    return OrdInfValidationStatus.InvalidSikyuKbn;
                }
                if (odrInf.SyohoSbt != 0 && odrInf.SyohoSbt != 1 && odrInf.SyohoSbt != 2)
                {
                    return OrdInfValidationStatus.InvalidSyohoSbt;
                }
                if (odrInf.SanteiKbn != 0 && odrInf.SanteiKbn != 1 && odrInf.SanteiKbn != 2)
                {
                    return OrdInfValidationStatus.InvalidSanteiKbn;
                }
                if (odrInf.TosekiKbn != 0 && odrInf.TosekiKbn != 1 && odrInf.TosekiKbn != 2)
                {
                    return OrdInfValidationStatus.InvalidTosekiKbn;
                }
                if (odrInf.SortNo <= 0)
                {
                    return OrdInfValidationStatus.InvalidSortNo;
                }
                if (odrInf.IsDeleted != 0 && odrInf.IsDeleted != 1 && odrInf.IsDeleted != 2)
                {
                    return OrdInfValidationStatus.InvalidIsDeleted;
                }
            }

            return OrdInfValidationStatus.Valid;
        }

        private static KeyValuePair<string, OrdInfValidationStatus> ValidateSpecialItem(TOdrInf odrInf)
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

                return new(countIndex.ToString(), OrdInfValidationStatus.InvalidSpecialItem);
            }
            else if (countUsage != null)
            {
                var countUsage1Index = odrInf.OrdInfDetails.FindIndex(od => od == countUsage);

                return new(countUsage1Index.ToString(), OrdInfValidationStatus.InvalidSpecialStadardUsage);
            }
            else if (countUsage2 != null)
            {
                var countUsage2Index = odrInf.OrdInfDetails.FindIndex(od => od == countUsage2);

                return new(countUsage2Index.ToString(), OrdInfValidationStatus.InvalidSpecialSuppUsage);
            }

            return new(odrValidateCode, OrdInfValidationStatus.Valid);
        }

        private static KeyValuePair<string, OrdInfValidationStatus> ValidateInputItemBussiness(TOdrInf odrInf)
        {
            if (odrInf.OrdInfDetails?.Count == 1)
            {
                var item = odrInf.OrdInfDetails[0];

                if (item.ItemCd == ItemCdConst.GazoDensibaitaiHozon)
                {
                    return new(odrValidateCode, OrdInfValidationStatus.InvalidGazoDensibaitaiHozon);
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
                return new(odrValidateCode, OrdInfValidationStatus.InvalidHasDrugButNotUsage);
            }

            if (countItems?.Count() > 0 && countUsage == 0 && (odrInf.IsInjection || odrInf.OdrKouiKbn == 28))
            {
                return new(odrValidateCode, OrdInfValidationStatus.InvalidHasInjectionButNotUsage);
            }

            if (countUsage == 1 && countItems?.Count() == 0)
            {
                var tokuzaiCount = odrInf.OrdInfDetails?.Count(item => item.MasterSbt.AsString().ToUpper() == "T") ?? 0;

                if (tokuzaiCount == 0)
                {
                    return new(odrValidateCode, OrdInfValidationStatus.InvalidTokuzai);
                }
            }

            var tokuzaiFirstCount = odrInf.OrdInfDetails?.Count(item => item.MasterSbt.AsString().ToUpper() == "T") ?? 0;

            if (tokuzaiFirstCount > 0)
            {
                if (odrInf.OdrKouiKbn == 0)
                {
                    return new(odrValidateCode, OrdInfValidationStatus.InvalidTokuzaiKouiKbn);
                }

                if (countItems?.Count() == 0 && (odrInf.IsDrug || odrInf.OdrKouiKbn == 20 || odrInf.IsInjection))
                {
                    return new(odrValidateCode, OrdInfValidationStatus.InvalidTokuzaiDrugOrInjection);
                }
            }

            return new(odrValidateCode, OrdInfValidationStatus.Valid);
        }

        private static KeyValuePair<string, OrdInfValidationStatus> ValidateBunkatu(TOdrInf odrInf)
        {
            if (odrInf.IsDrug)
            {
                int bunkatuItemCount = odrInf.OrdInfDetails?.Count(i => i.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu) ?? 0;

                if (bunkatuItemCount > 1)
                {
                    return new(odrValidateCode, OrdInfValidationStatus.InvalidBunkatu);
                }

                var bunkatuItem = odrInf.OrdInfDetails?.FirstOrDefault(i => i.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu);
                if (bunkatuItem != null)
                {
                    var usageItem = odrInf.OrdInfDetails?.FirstOrDefault(item => item.IsStandardUsage);

                    if (usageItem == null)
                    {
                        var usageIndex = odrInf.OrdInfDetails?.FindIndex(od => od == bunkatuItem) ?? 0;

                        return new(usageIndex.ToString(), OrdInfValidationStatus.InvalidUsageWhenBuntakuNull);
                    }

                    var sumBukatu = odrInf.SumBunkatu(bunkatuItem?.Bunkatu ?? string.Empty);

                    if (usageItem.Suryo != sumBukatu)
                    {
                        var usageIndex = odrInf.OrdInfDetails?.FindIndex(od => od == usageItem) ?? 0;

                        return new(usageIndex.ToString(), OrdInfValidationStatus.InvalidSumBunkatuDifferentSuryo);
                    }
                }
            }

            return new(odrValidateCode, OrdInfValidationStatus.Valid);
        }

        private static KeyValuePair<string, OrdInfValidationStatus> ValidateUsageOfSelfInjection(TOdrInf odrInf)
        {
            if (odrInf.OdrKouiKbn == 28)
            {
                var seflInjection = odrInf.OrdInfDetails?.FirstOrDefault(d => d.ItemCd == ItemCdConst.ChusyaJikocyu);
                var usageCount = odrInf.OrdInfDetails?.Count(d => d.IsInjectionUsage);
                if (seflInjection == null && usageCount == 0)
                {
                    return new(odrValidateCode, OrdInfValidationStatus.InvalidHasNotBothInjectionAndUsageOf28);
                }
            }

            return new(odrValidateCode, OrdInfValidationStatus.Valid);
        }

        private static KeyValuePair<string, OrdInfValidationStatus> ValidateHasUsageButNotHasDrugOrInjection(TOdrInf odrInf)
        {
            if (odrInf.OrdInfDetails?.Count(d => d.IsDrugUsage) > 0 && odrInf.OrdInfDetails?.Count(d => d.IsDrug || d.SinKouiKbn == 20 && d.ItemCd.StartsWith("Z")) == 0)
            {
                return new(odrValidateCode, OrdInfValidationStatus.InvalidHasUsageButNotDrug);
            }

            if (odrInf.OrdInfDetails?.Count(d => d.IsInjectionUsage) > 0
              && odrInf.OrdInfDetails?.Count(d => d.IsInjection || d.IsDrug) == 0)
            {
                return new(odrValidateCode, OrdInfValidationStatus.InvalidHasUsageButNotInjectionOrDrug);
            }

            return new(odrValidateCode, OrdInfValidationStatus.Valid);
        }

        private static KeyValuePair<string, OrdInfValidationStatus> ValidateHasNotUsageOfDrug(TOdrInf odrInf, int flag, int sinDate, int refillSetting)
        {
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
                            var validateResult = ValidationDetail(detail, flag, sinDate, refillSetting);
                            if (validateResult != OrdInfValidationStatus.Valid) return new(count.ToString(), validateResult);
                            count++;
                        }
                    }
                }
                else
                {
                    return new(odrValidateCode, OrdInfValidationStatus.InvalidHasDrugButNotUsage);
                }
            }

            return new(odrValidateCode, OrdInfValidationStatus.Valid);
        }

        private static KeyValuePair<string, OrdInfValidationStatus> ValidateHasNotUsageOfInjection(TOdrInf odrInf, int flag, int sinDate, int refillSetting)
        {
            if (odrInf.IsInjection)
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
                            var validateResult = ValidationDetail(detail, flag, sinDate, refillSetting);
                            if (OrdInfValidationStatus.Valid != validateResult) return new(count.ToString(), validateResult);
                            count++;
                        }
                    }
                }
                else
                {
                    return new(odrValidateCode, OrdInfValidationStatus.InvalidHasInjectionButNotUsage);
                }
            }

            return new(odrValidateCode, OrdInfValidationStatus.Valid);
        }

        #endregion

        #region Seperate Validate OdrInfDetail
        private static OrdInfValidationStatus ValidateCommonDetail(TOdrInfDetailModel odrInfDetail)
        {
            if (odrInfDetail.RowNo <= 0)
            {
                return OrdInfValidationStatus.InvalidRowNo;
            }
            if (odrInfDetail.SinKouiKbn < 0)
            {
                return OrdInfValidationStatus.InvalidSinKouiKbn;
            }
            if (odrInfDetail.ItemCd.Length > 10)
            {
                return OrdInfValidationStatus.InvalidItemCd;
            }
            if (odrInfDetail.ItemName.Length > 240)
            {
                return OrdInfValidationStatus.InvalidItemName;
            }
            if (odrInfDetail.Suryo < 0 || odrInfDetail.ItemCd == ItemCdConst.JikanKihon && !(odrInfDetail.Suryo >= 0 && odrInfDetail.Suryo <= 7) || odrInfDetail.ItemCd == ItemCdConst.SyosaiKihon && !(odrInfDetail.Suryo >= 0 && odrInfDetail.Suryo <= 8))
            {
                return OrdInfValidationStatus.InvalidSuryo;
            }
            if (odrInfDetail.UnitName.Length > 24)
            {
                return OrdInfValidationStatus.InvalidUnitName;
            }
            if (!(odrInfDetail.SyohoKbn >= 0 && odrInfDetail.SyohoKbn <= 3))
            {
                return OrdInfValidationStatus.InvalidSyohoKbn;
            }
            if (!(odrInfDetail.YohoKbn >= 0 && odrInfDetail.YohoKbn <= 2))
            {
                return OrdInfValidationStatus.InvalidYohoKbn;
            }
            if (odrInfDetail.Bunkatu.Length > 10)
            {
                return OrdInfValidationStatus.InvalidBunkatuLength;
            }
            if (odrInfDetail.CmtName.Length > 240)
            {
                return OrdInfValidationStatus.InvalidCmtName;
            }
            if (odrInfDetail.CmtOpt.Length > 38)
            {
                return OrdInfValidationStatus.InvalidCmtOpt;
            }

            return OrdInfValidationStatus.Valid;
        }

        private static OrdInfValidationStatus ValidateCommonDetailInputItem(TOdrInfDetailModel odrInfDetail)
        {
            if (odrInfDetail.HpId <= 0)
            {
                return OrdInfValidationStatus.InvalidHpId;
            }
            if (odrInfDetail.RpNo <= 0)
            {
                return OrdInfValidationStatus.InvalidRpNo;
            }
            if (odrInfDetail.RpEdaNo <= 0)
            {
                return OrdInfValidationStatus.InvalidRpEdaNo;
            }
            if (odrInfDetail.RowNo <= 0)
            {
                return OrdInfValidationStatus.InvalidRowNo;
            }

            if (odrInfDetail.SinKouiKbn < 0)
            {
                return OrdInfValidationStatus.InvalidSinKouiKbn;
            }
            if (odrInfDetail.ItemCd.Length > 10)
            {
                return OrdInfValidationStatus.InvalidItemCd;
            }
            if (odrInfDetail.ItemName.Length > 240)
            {
                return OrdInfValidationStatus.InvalidItemName;
            }
            if (odrInfDetail.Suryo < 0)
            {
                return OrdInfValidationStatus.InvalidSuryo;
            }
            if (odrInfDetail.UnitName.Length > 24)
            {
                return OrdInfValidationStatus.InvalidUnitName;
            }
            if (odrInfDetail.UnitSbt != 0 && odrInfDetail.UnitSbt != 1 && odrInfDetail.UnitSbt != 2)
            {
                return OrdInfValidationStatus.InvalidUnitSbt;
            }
            if (odrInfDetail.TermVal < 0)
            {
                return OrdInfValidationStatus.InvalidTermVal;
            }
            if (!(odrInfDetail.SyohoKbn >= 0 && odrInfDetail.SyohoKbn <= 3))
            {
                return OrdInfValidationStatus.InvalidSyohoKbn;
            }
            if (!(odrInfDetail.SyohoLimitKbn >= 0 && odrInfDetail.SyohoLimitKbn <= 3))
            {
                return OrdInfValidationStatus.InvalidSyohoLimitKbn;
            }
            if (!(odrInfDetail.YohoKbn >= 0 && odrInfDetail.YohoKbn <= 2))
            {
                return OrdInfValidationStatus.InvalidYohoKbn;
            }
            if (!(odrInfDetail.IsNodspRece >= 0 && odrInfDetail.IsNodspRece <= 1))
            {
                return OrdInfValidationStatus.InvalidIsNodspRece;
            }
            if (odrInfDetail.IpnCd.Length > 12)
            {
                return OrdInfValidationStatus.InvalidIpnCd;
            }
            if (odrInfDetail.IpnName.Length > 120)
            {
                return OrdInfValidationStatus.InvalidIpnName;
            }
            if (odrInfDetail.Bunkatu.Length > 10)
            {
                return OrdInfValidationStatus.InvalidBunkatuLength;
            }
            if (odrInfDetail.CmtName.Length > 240)
            {
                return OrdInfValidationStatus.InvalidCmtName;
            }
            if (odrInfDetail.CmtOpt.Length > 38)
            {
                return OrdInfValidationStatus.InvalidCmtOpt;
            }
            if (odrInfDetail.FontColor.Length > 8)
            {
                return OrdInfValidationStatus.InvalidFontColor;
            }
            if (!(odrInfDetail.CommentNewline >= 0 && odrInfDetail.CommentNewline <= 1))
            {
                return OrdInfValidationStatus.InvalidCommentNewline;
            }

            return OrdInfValidationStatus.Valid;
        }

        private static OrdInfValidationStatus ValidateCmtDetail(int sinDate, TOdrInfDetailModel odrInfDetail)
        {
            if (odrInfDetail.Is840Cmt && odrInfDetail.CmtCol1 > 0 && (string.IsNullOrEmpty(odrInfDetail.CmtOpt) || string.IsNullOrEmpty(odrInfDetail.CmtName)))
            {
                return OrdInfValidationStatus.InvalidCmt840;
            }

            if (odrInfDetail.Is842Cmt)
            {
                if (string.IsNullOrEmpty(odrInfDetail.CmtOpt) || string.IsNullOrEmpty(odrInfDetail.CmtName))
                {
                    return OrdInfValidationStatus.InvalidCmt842;
                }
                else if (!string.IsNullOrEmpty(odrInfDetail.CmtOpt) && odrInfDetail.CmtOpt.Length > 38)
                {
                    return OrdInfValidationStatus.InvalidCmt842CmtOptMoreThan38;
                }
            }

            if (odrInfDetail.Is830Cmt)
            {
                string fullSpace = @"　";

                if (string.IsNullOrEmpty(odrInfDetail.CmtOpt) && odrInfDetail.CmtOpt != fullSpace)
                {
                    return OrdInfValidationStatus.InvalidCmt830CmtOpt;
                }
                else if (!string.IsNullOrEmpty(odrInfDetail.CmtOpt) && odrInfDetail.CmtOpt.Length > 38)
                {
                    return OrdInfValidationStatus.InvalidCmt830CmtOptMoreThan38;
                }
            }

            if (odrInfDetail.Is831Cmt && (string.IsNullOrEmpty(odrInfDetail.CmtOpt) || string.IsNullOrEmpty(odrInfDetail.CmtName)))
            {
                return OrdInfValidationStatus.InvalidCmt831;
            }

            if (odrInfDetail.Is850Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt850(odrInfDetail.CmtOpt, odrInfDetail.ItemName);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(odrInfDetail.CmtName))
                {
                    if (odrInfDetail.CmtName.Contains('日'))
                    {
                        return OrdInfValidationStatus.InvalidCmt850Date;
                    }
                    else
                    {
                        return OrdInfValidationStatus.InvalidCmt850OtherDate;
                    }
                }
            }

            if (odrInfDetail.Is851Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt851(odrInfDetail.CmtOpt);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(odrInfDetail.CmtName))
                {
                    return OrdInfValidationStatus.InvalidCmt851;
                }
            }

            if (odrInfDetail.Is852Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt852(odrInfDetail.CmtOpt);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(odrInfDetail.CmtName))
                {
                    return OrdInfValidationStatus.InvalidCmt852;
                }
            }

            if (odrInfDetail.Is853Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt853(odrInfDetail.CmtOpt, sinDate);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(odrInfDetail.CmtName))
                {
                    return OrdInfValidationStatus.InvalidCmt853;
                }
            }

            if (odrInfDetail.Is880Cmt && (string.IsNullOrEmpty(odrInfDetail.CmtOpt) || string.IsNullOrEmpty(odrInfDetail.CmtName)))
            {
                return OrdInfValidationStatus.InvalidCmt880;
            }

            return OrdInfValidationStatus.Valid;
        }
        #endregion
    }
}
