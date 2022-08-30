using Domain.Models.OrdInfDetails;
using Helper.Common;
using Helper.Constants;
using static Helper.Constants.TodayOrderConst;

namespace Domain.Models.OrdInfs
{
    public class OrdInfModel
    {
        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public long RpNo { get; private set; }
        public long RpEdaNo { get; private set; }
        public long PtId { get; set; }
        public int SinDate { get; private set; }
        public int HokenPid { get; private set; }
        public int OdrKouiKbn { get; private set; }
        public string RpName { get; private set; }
        public int InoutKbn { get; private set; }
        public int SikyuKbn { get; private set; }
        public int SyohoSbt { get; private set; }
        public int SanteiKbn { get; private set; }
        public int TosekiKbn { get; private set; }
        public int DaysCnt { get; private set; }
        public int SortNo { get; private set; }
        public int IsDeleted { get; private set; }
        public long Id { get; private set; }
        public GroupKoui GroupKoui { get; private set; }
        public List<OrdInfDetailModel> OrdInfDetails { get; private set; }


        public OrdInfModel(int hpId, long raiinNo, long rpNo, long rpEdaNo, long ptId, int sinDate, int hokenPid, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, int isDeleted, long id, List<OrdInfDetailModel> ordInfDetails)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            PtId = ptId;
            SinDate = sinDate;
            HokenPid = hokenPid;
            OdrKouiKbn = odrKouiKbn;
            RpName = rpName;
            InoutKbn = inoutKbn;
            SikyuKbn = sikyuKbn;
            SyohoSbt = syohoSbt;
            SanteiKbn = santeiKbn;
            TosekiKbn = tosekiKbn;
            DaysCnt = daysCnt;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            Id = id;
            GroupKoui = GroupKoui.From(odrKouiKbn);
            OrdInfDetails = ordInfDetails;
        }

        // 処方 - Drug
        public bool IsDrug
        {
            get
            {
                return OdrKouiKbn >= 21 && OdrKouiKbn <= 23;
            }
        }

        // 注射 - Injection
        public bool IsInjection
        {
            get
            {
                return OdrKouiKbn >= 30 && OdrKouiKbn <= 34;
            }
        }

        private double SumBunkatu(string bunkatu)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(bunkatu))
                    return 0;

                var nums = bunkatu.Split('+');

                return nums.Sum(n => n.AsDouble());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public KeyValuePair<int, TodayOrdValidationStatus> Validation()
        {
            if (!TodayOrderConst.OdrKouiKbns.Values.Contains(OdrKouiKbn))
            {
                return new(-1, TodayOrdValidationStatus.InvalidOdrKouiKbn);
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

                    return new(countIndex, TodayOrdValidationStatus.InvalidSpecialItem);
                }
                else if (countUsage != null)
                {
                    var countUsage1Index = OrdInfDetails.FindIndex(od => od == countUsage);

                    return new(countUsage1Index, TodayOrdValidationStatus.InvalidSpecialStadardUsage);
                }
                else if (countUsage2 != null)
                {
                    var countUsage2Index = OrdInfDetails.FindIndex(od => od == countUsage2);

                    return new(countUsage2Index, TodayOrdValidationStatus.InvalidSpecialSuppUsage);
                }
            }

            if (OrdInfDetails?.Count(d => d.IsDrugUsage) > 0
                && OrdInfDetails?.Count(d => d.IsDrug || (d.SinKouiKbn == 20 && d.ItemCd.StartsWith("Z"))) == 0)
            {
                return new(-1, TodayOrdValidationStatus.InvalidHasUsageButNotDrug);
            }

            if (OrdInfDetails?.Count(d => d.IsInjectionUsage) > 0
              && OrdInfDetails?.Count(d => d.IsInjection || d.IsDrug) == 0)
            {
                return new(-1, TodayOrdValidationStatus.InvalidHasUsageButNotInjectionOrDrug);
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
                            var validateResult = detail.Validation();
                            if (validateResult != TodayOrdValidationStatus.Valid) return new(count, validateResult);
                            count++;
                        }
                    }
                }
                else
                {
                    return new(-1, TodayOrdValidationStatus.InvalidHasDrugButNotUsage);
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
                            var validateResult = detail.Validation();
                            if (TodayOrdValidationStatus.Valid != validateResult) return new(count, validateResult);
                            count++;
                        }
                    }
                }
                else
                {
                    return new(-1, TodayOrdValidationStatus.InvalidHasInjectionButNotUsage);
                }
            }
            else if (OdrKouiKbn == 28)
            {
                var seflInjection = OrdInfDetails?.FirstOrDefault(d => d.ItemCd == ItemCdConst.ChusyaJikocyu);
                var usageCount = OrdInfDetails?.Count(d => d.IsInjectionUsage);
                if (seflInjection == null && usageCount == 0)
                {
                    return new(-1, TodayOrdValidationStatus.InvalidHasNotBothInjectionAndUsageOf28);
                }
            }

            if (IsDrug || IsInjection)
            {
                // 用法
                var usageCount = OrdInfDetails?.Count(o => o.IsStandardUsage);
                if (usageCount > 1)
                {
                    return new(-1, TodayOrdValidationStatus.InvalidStandardUsageOfDrugOrInjection);
                }

                // 補助用法
                var usage2Count = OrdInfDetails?.Count(item => item.IsSuppUsage);
                if (usage2Count > 1)
                {
                    return new(-1, TodayOrdValidationStatus.InvalidSuppUsageOfDrugOrInjection);
                }
            }

            if (IsDrug)
            {
                int bunkatuItemCount = OrdInfDetails?.Count(i => i.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu) ?? 0;

                if (bunkatuItemCount > 1)
                {
                    return new(-1, TodayOrdValidationStatus.InvalidBunkatu);
                }

                var bunkatuItem = OrdInfDetails?.FirstOrDefault(i => i.ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu);
                if (bunkatuItem != null)
                {
                    var usageItem = OrdInfDetails?.FirstOrDefault(item => item.IsStandardUsage);

                    if (usageItem == null)
                    {
                        var usageIndex = OrdInfDetails?.FindIndex(od => od == bunkatuItem) ?? 0;

                        return new(usageIndex, TodayOrdValidationStatus.InvalidUsageWhenBuntakuNull);
                    }

                    var sumBukatu = SumBunkatu(bunkatuItem?.Bunkatu ?? string.Empty);

                    if (usageItem.Suryo != sumBukatu)
                    {
                        var usageIndex = OrdInfDetails?.FindIndex(od => od == usageItem) ?? 0;

                        return new(usageIndex, TodayOrdValidationStatus.InvalidSumBunkatuDifferentSuryo);
                    }
                }
            }
            if (OrdInfDetails?.Count > 0)
            {
                var count = 0;
                foreach (var ordInfDetail in OrdInfDetails)
                {
                    var status = ordInfDetail.Validation();
                    if (status != TodayOrdValidationStatus.Valid)
                    {
                        return new(count, status);
                    }
                    count++;
                }
            }
            return new(-1, TodayOrdValidationStatus.Valid);
        }
    }
}
