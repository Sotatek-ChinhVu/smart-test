﻿using Domain.Models.PatientInfor;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace CommonChecker.DB
{
    public class RealtimeOrderErrorFinder : RepositoryBase, IRealtimeOrderErrorFinder
    {

        public RealtimeOrderErrorFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        {

        }

        public string FindAgeComment(int hpId, string commentCode)
        {
            string ageComment = string.Empty;
            var ageCommentInfo = NoTrackingDataContext.M14CmtCode.FirstOrDefault(i => i.HpId == hpId && i.AttentionCmtCd == commentCode);
            if (ageCommentInfo != null)
            {
                ageComment = ageCommentInfo.AttentionCmt ?? string.Empty;
            }
            return ageComment;
        }

        public string FindAnalogueName(int hpId, string analogueCode)
        {
            string analogueName = string.Empty;
            var analogueInfo = NoTrackingDataContext.M56AnalogueCd.FirstOrDefault(i => i.HpId == hpId && i.AnalogueCd == analogueCode);
            if (analogueInfo != null)
            {
                analogueName = analogueInfo.AnalogueName ?? string.Empty;
            }
            return analogueName;
        }

        public Dictionary<string, string> FindAnalogueNameDic(int hpId, List<string> analogueCodeList)
        {
            analogueCodeList = analogueCodeList.Distinct().ToList();
            Dictionary<string, string> result = new();
            var analogueInfoList = NoTrackingDataContext.M56AnalogueCd.Where(item => item.HpId == hpId && analogueCodeList.Contains(item.AnalogueCd)).ToList();
            foreach (var analogueCd in analogueCodeList)
            {
                result.Add(analogueCd, analogueInfoList.FirstOrDefault(item => item.AnalogueCd == analogueCd)?.AnalogueName ?? string.Empty);
            }
            return result;
        }

        public string FindClassName(int hpId, string classCd)
        {
            string className = string.Empty;
            var classInfo = NoTrackingDataContext.M56DrugClass.FirstOrDefault(i => i.HpId == hpId && i.ClassCd == classCd);
            if (classInfo != null)
            {
                className = classInfo.ClassName ?? string.Empty;
            }
            return className;
        }

        public string FindComponentName(int hpId, string conponentCode)
        {
            string componentName = string.Empty;
            var componentInfo = NoTrackingDataContext.M56ExIngCode.FirstOrDefault(i => i.HpId == hpId && i.SeibunCd == conponentCode && i.SeibunIndexCd == "000");
            if (componentInfo != null)
            {
                componentName = componentInfo.SeibunName ?? string.Empty;
            }
            return componentName;
        }

        public Dictionary<string, string> FindComponentNameDic(int hpId, List<string> conponentCodeList)
        {
            conponentCodeList = conponentCodeList.Distinct().ToList();
            var componentInfoList = NoTrackingDataContext.M56ExIngCode.Where(item => item.HpId == hpId && conponentCodeList.Contains(item.SeibunCd) && item.SeibunIndexCd == "000").ToList();
            Dictionary<string, string> result = new();
            foreach (var seibunCd in conponentCodeList)
            {
                result.Add(seibunCd, componentInfoList.FirstOrDefault(item => item.SeibunCd == seibunCd)?.SeibunName ?? string.Empty);
            }
            return result;
        }

        public string FindDiseaseComment(int hpId, string commentCode)
        {
            string diseaseComment = string.Empty;
            var diseaseCommentInfo = NoTrackingDataContext.M42ContraCmt.FirstOrDefault(i => i.HpId == hpId && i.CmtCd == commentCode);
            if (diseaseCommentInfo != null)
            {
                diseaseComment = diseaseCommentInfo.Cmt ?? string.Empty;
            }
            return diseaseComment;
        }

        public string FindDiseaseName(int hpId, string byotaiCd)
        {
            string diseaseName = string.Empty;
            var diseaseInfo = NoTrackingDataContext.M42ContraindiDisCon.FirstOrDefault(i => i.HpId == hpId && i.ByotaiCd == byotaiCd);
            if (diseaseInfo != null)
            {
                diseaseName = diseaseInfo.Byomei ?? string.Empty;
            }
            return diseaseName;
        }

        public Dictionary<string, string> FindDiseaseNameDic(int hpId, List<string> byotaiCdList)
        {
            byotaiCdList = byotaiCdList.Distinct().ToList();
            Dictionary<string, string> result = new();
            var diseaseInfoList = NoTrackingDataContext.M42ContraindiDisCon.Where(item => item.HpId == hpId && byotaiCdList.Contains(item.ByotaiCd)).ToList();
            foreach (var byotaiCd in byotaiCdList)
            {
                result.Add(byotaiCd, diseaseInfoList.FirstOrDefault(item => item.ByotaiCd == byotaiCd)?.Byomei ?? string.Empty);
            }
            return result;
        }

        public string FindDrvalrgyName(int hpId, string drvalrgyCode)
        {
            string drvalrgyName = string.Empty;
            var drvalrgyInfo = NoTrackingDataContext.M56DrvalrgyCode.FirstOrDefault(i => i.HpId == hpId && i.DrvalrgyCd == drvalrgyCode);
            if (drvalrgyInfo != null)
            {
                drvalrgyName = drvalrgyInfo.DrvalrgyName ?? string.Empty;
            }
            return drvalrgyName;
        }

        public Dictionary<string, string> FindDrvalrgyNameDic(int hpId, List<string> drvalrgyCodeList)
        {
            drvalrgyCodeList = drvalrgyCodeList.Distinct().ToList();
            Dictionary<string, string> result = new();
            var drvalrgyInfoList = NoTrackingDataContext.M56DrvalrgyCode.Where(item => item.HpId == hpId && drvalrgyCodeList.Contains(item.DrvalrgyCd)).ToList();
            foreach (var drvalrgyCd in drvalrgyCodeList)
            {
                result.Add(drvalrgyCd, drvalrgyInfoList.FirstOrDefault(item => item.DrvalrgyCd == drvalrgyCd)?.DrvalrgyName ?? string.Empty);
            }
            return result;
        }

        public string FindFoodName(int hpId, string foodCode)
        {
            string foodName = string.Empty;
            var foodInfo = NoTrackingDataContext.M12FoodAlrgyKbn.FirstOrDefault(i => i.FoodKbn == foodCode && i.HpId == hpId);
            if (foodInfo != null)
            {
                foodName = foodInfo.FoodName ?? string.Empty;
            }
            return foodName;
        }

        public Dictionary<string, string> FindFoodNameDic(int hpId, List<string> foodCodeList)
        {
            foodCodeList = foodCodeList.Distinct().ToList();
            Dictionary<string, string> result = new();
            var foodInfoList = NoTrackingDataContext.M12FoodAlrgyKbn.Where(i => i.HpId == hpId && foodCodeList.Contains(i.FoodKbn)).ToList();
            foreach (var foodKbn in foodCodeList)
            {
                result.Add(foodKbn, foodInfoList.FirstOrDefault(item => item.FoodKbn == foodKbn)?.FoodName ?? string.Empty);
            }
            return result;
        }

        public string FindIppanNameByIppanCode(string ippanCode)
        {
            var ippanInfo = NoTrackingDataContext.IpnNameMsts.FirstOrDefault(i => i.IpnNameCd == ippanCode);
            return ippanInfo == null ? string.Empty : ippanInfo.IpnName ?? string.Empty;
        }

        public string FindItemName(int hpId , string yjCd, int sinday)
        {
            var itemInfo = NoTrackingDataContext.TenMsts.FirstOrDefault(d => d.HpId == hpId && d.StartDate <= sinday && sinday <= d.EndDate && d.YjCd == yjCd);
            return itemInfo != null ? itemInfo.Name ?? string.Empty : string.Empty;
        }

        public Dictionary<string, string> FindItemNameDic(int hpId, List<string> yjCdList, int sinday)
        {
            yjCdList = yjCdList.Distinct().ToList();
            var itemInfoList = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId && item.StartDate <= sinday && sinday <= item.EndDate && item.YjCd != null && yjCdList.Contains(item.YjCd)).ToList();
            Dictionary<string, string> result = new();
            foreach (var yjcd in yjCdList)
            {
                result.Add(yjcd, itemInfoList.FirstOrDefault(item => item.YjCd == yjcd)?.Name ?? string.Empty);
            }
            return result;
        }

        public string FindItemNameByItemCode(int hpId, string itemCd, int sinday)
        {
            var itemInfo = NoTrackingDataContext.TenMsts.FirstOrDefault(t => t.HpId == hpId && t.ItemCd == itemCd && t.StartDate <= sinday && sinday <= t.EndDate);
            return itemInfo != null ? itemInfo.Name ?? string.Empty : string.Empty;
        }

        public Dictionary<string, string> FindItemNameByItemCodeDic(int hpId, List<string> itemCdList, int sinday)
        {
            itemCdList = itemCdList.Distinct().ToList();
            Dictionary<string, string> result = new();
            var itemInfoList = NoTrackingDataContext.TenMsts.Where(item => item.HpId == hpId && itemCdList.Contains(item.ItemCd) && item.StartDate <= sinday && sinday <= item.EndDate).ToList();
            foreach (var itemCd in itemCdList)
            {
                result.Add(itemCd, itemInfoList.FirstOrDefault(item => item.ItemCd == itemCd)?.Name ?? string.Empty);
            }
            return result;
        }

        public string FindKijyoComment(int hpId, string commentCode)
        {
            string kijyoComment = string.Empty;
            var kijyoCommentInfo = NoTrackingDataContext.M01KijyoCmt.FirstOrDefault(i => i.HpId == hpId && i.CmtCd == commentCode);
            if (kijyoCommentInfo != null)
            {
                kijyoComment = kijyoCommentInfo.Cmt ?? string.Empty;
            }
            return kijyoComment;
        }

        public Dictionary<string, string> FindKijyoCommentDic(int hpId, List<string> commentCodeList)
        {
            commentCodeList = commentCodeList.Distinct().ToList();
            Dictionary<string, string> result = new();
            var kijyoCommentInfo = NoTrackingDataContext.M01KijyoCmt.Where(i => i.HpId == hpId && commentCodeList.Contains(i.CmtCd)).ToList();
            foreach (var cmtCd in commentCodeList)
            {
                result.Add(cmtCd, kijyoCommentInfo.FirstOrDefault(item => item.CmtCd == cmtCd)?.Cmt ?? string.Empty);
            }
            return result;
        }

        public string FindKinkiComment(int hpId, string commentCode)
        {
            string kinkiComment = string.Empty;
            var kinkiCommentInfo = NoTrackingDataContext.M01KinkiCmt.FirstOrDefault(i => i.HpId == hpId && i.CmtCd == commentCode);
            if (kinkiCommentInfo != null)
            {
                kinkiComment = kinkiCommentInfo.Cmt ?? string.Empty;
            }
            return kinkiComment;
        }

        public Dictionary<string, string> FindKinkiCommentDic(int hpId, List<string> commentCodeList)
        {
            commentCodeList = commentCodeList.Distinct().ToList();
            Dictionary<string, string> result = new();
            var kinkiCommentInfo = NoTrackingDataContext.M01KinkiCmt.Where(item => item.HpId == hpId && commentCodeList.Contains(item.CmtCd)).ToList();
            foreach (var cmtCd in commentCodeList)
            {
                result.Add(cmtCd, kinkiCommentInfo.FirstOrDefault(item => item.CmtCd == cmtCd)?.Cmt ?? string.Empty);
            }
            return result;
        }

        public string FindOTCItemName(int hpId, int serialNum)
        {
            string oTCITemName = string.Empty;
            var oTCITemNameInfo = NoTrackingDataContext.M38OtcMain.FirstOrDefault(i => i.HpId == hpId && i.SerialNum == serialNum);
            if (oTCITemNameInfo != null)
            {
                oTCITemName = oTCITemNameInfo.TradeName ?? string.Empty;
            }
            return oTCITemName;
        }

        public Dictionary<string, string> FindOTCItemNameDic(int hpId, List<string> serialNumList)
        {
            serialNumList = serialNumList.Distinct().ToList();
            Dictionary<string, string> result = new();
            var oTCITemNameInfoList = NoTrackingDataContext.M38OtcMain.Where(i => i.HpId == hpId && serialNumList.Contains(i.SerialNum.ToString())).ToList();
            foreach (var serialNum in serialNumList)
            {
                result.Add(serialNum, oTCITemNameInfoList.FirstOrDefault(item => item.SerialNum.ToString() == serialNum)?.TradeName ?? string.Empty);
            }
            return result;
        }

        public string FindSuppleItemName(int hpId, string seibunCd)
        {
            string suppleItemName = string.Empty;
            var suppleItemNameInfo = NoTrackingDataContext.M41SuppleIngres.FirstOrDefault(i => i.HpId == hpId && i.SeibunCd == seibunCd);
            if (suppleItemNameInfo != null)
            {
                suppleItemName = suppleItemNameInfo.Seibun ?? string.Empty;
            }
            return suppleItemName;
        }

        public Dictionary<string, string> FindSuppleItemNameDic(int hpId, List<string> seibunCdList)
        {
            seibunCdList = seibunCdList.Distinct().ToList();
            Dictionary<string, string> result = new();
            var suppleItemNameInfoList = NoTrackingDataContext.M41SuppleIngres.Where(i => i.HpId == hpId && seibunCdList.Contains(i.SeibunCd)).ToList();
            foreach (var seibunCd in seibunCdList)
            {
                result.Add(seibunCd, suppleItemNameInfoList.FirstOrDefault(item => item.SeibunCd == seibunCd)?.Seibun ?? string.Empty);
            }
            return result;
        }

        public string GetOTCComponentInfo(int hpId, string seibunCd)
        {
            var otcComponentInfo = NoTrackingDataContext.M38IngCode.FirstOrDefault(i => i.SeibunCd == seibunCd && i.HpId == hpId);
            return otcComponentInfo != null ? otcComponentInfo.Seibun ?? string.Empty : string.Empty;
        }

        public Dictionary<string, string> GetOTCComponentInfoDic(int hpId, List<string> seibunCdList)
        {
            seibunCdList = seibunCdList.Distinct().ToList();
            Dictionary<string, string> result = new();
            var otcComponentInfoList = NoTrackingDataContext.M38IngCode.Where(i => i.HpId == hpId && seibunCdList.Contains(i.SeibunCd)).ToList();
            foreach (var seibunCd in seibunCdList)
            {
                result.Add(seibunCd, otcComponentInfoList.FirstOrDefault(item => item.SeibunCd == seibunCd)?.Seibun ?? string.Empty);
            }
            return result;
        }

        public string GetSupplementComponentInfo(int hpId, string seibunCd)
        {
            var supplementComponentInfo = NoTrackingDataContext.M41SuppleIngres.FirstOrDefault(i => i.HpId == hpId && i.SeibunCd == seibunCd);
            return supplementComponentInfo != null ? supplementComponentInfo.Seibun ?? string.Empty : string.Empty;
        }

        public Dictionary<string, string> GetSupplementComponentInfoDic(int hpId, List<string> seibunCdList)
        {
            seibunCdList = seibunCdList.Distinct().ToList();
            Dictionary<string, string> result = new();
            var supplementComponentInfo = NoTrackingDataContext.M41SuppleIngres.Where(item => item.HpId == hpId && seibunCdList.Contains(item.SeibunCd)).ToList();
            foreach (var seibunCd in seibunCdList)
            {
                result.Add(seibunCd, supplementComponentInfo.FirstOrDefault(item => item.SeibunCd == seibunCd)?.Seibun ?? string.Empty);
            }
            return result;
        }

        public string GetUsageDosage(int hpId, string yjCd)
        {
            var dosageInfo =
                  (from dosageDrug in NoTrackingDataContext.DosageDrugs.Where(d => d.HpId == hpId && d.YjCd == yjCd)
                   join dosageDosage in NoTrackingDataContext.DosageDosages.Where(d => d.HpId == hpId && !string.IsNullOrEmpty(d.UsageDosage))
                   on dosageDrug.DoeiCd equals dosageDosage.DoeiCd
                   select new
                   {
                       UsageDosage = dosageDosage.UsageDosage == null ? string.Empty : dosageDosage.UsageDosage.Replace("；", Environment.NewLine)
                   }
                  ).FirstOrDefault();
            return dosageInfo != null ? dosageInfo.UsageDosage : string.Empty;
        }

        public Dictionary<string, string> GetUsageDosageDic(int hpId, List<string> yjCdList)
        {
            yjCdList = yjCdList.Distinct().ToList();
            Dictionary<string, string> result = new();
            var dosageInfoList =
                  (from dosageDrug in NoTrackingDataContext.DosageDrugs.Where(d => d.HpId == hpId && yjCdList.Contains(d.YjCd))
                   join dosageDosage in NoTrackingDataContext.DosageDosages.Where(d => d.HpId == hpId && !string.IsNullOrEmpty(d.UsageDosage))
                   on dosageDrug.DoeiCd equals dosageDosage.DoeiCd
                   select new
                   {
                       dosageDrug.YjCd,
                       UsageDosage = dosageDosage.UsageDosage == null ? string.Empty : dosageDosage.UsageDosage.Replace("；", Environment.NewLine)
                   }
                  ).ToList();

            foreach (var yjCd in yjCdList)
            {
                result.Add(yjCd, dosageInfoList.FirstOrDefault(item => item.YjCd == yjCd)?.UsageDosage ?? string.Empty);
            }
            return result;
        }

        public bool IsNoMasterData(int hpId)
        {
            return NoTrackingDataContext.M56ExEdIngredients.Count(item => item.HpId == hpId) == 0;
        }

        public PatientInforModel? FindPatientByPtId(int hpId, long ptId)
        {
            var ptInf = NoTrackingDataContext.PtInfs.FirstOrDefault(p => p.HpId == hpId && p.PtId == ptId && p.IsDelete != 1);
            if (ptInf == null)
            {
                return null;
            }
            return new PatientInforModel(ptInf.PtId,
                                         ptInf.PtNum,
                                         ptInf.Name ?? string.Empty,
                                         ptInf.KanaName ?? string.Empty,
                                         ptInf.Sex,
                                         ptInf.Birthday
                                         );
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }
    }
}
