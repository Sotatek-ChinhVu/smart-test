using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CommonChecker.DB
{
    public class RealtimeOrderErrorFinder : RepositoryBase, IRealtimeOrderErrorFinder
    {

        public RealtimeOrderErrorFinder(ITenantProvider tenantProvider) : base(tenantProvider)
        {

        }

        public string FindAgeComment(string commentCode)
        {
            string ageComment = string.Empty;
            var ageCommentInfo = NoTrackingDataContext.M14CmtCode.FirstOrDefault(i => i.AttentionCmtCd == commentCode);
            if (ageCommentInfo != null)
            {
                ageComment = ageCommentInfo.AttentionCmt ?? string.Empty;
            }
            return ageComment;
        }

        public string FindAnalogueName(string analogueCode)
        {
            string analogueName = string.Empty;
            var analogueInfo = NoTrackingDataContext.M56AnalogueCd.FirstOrDefault(i => i.AnalogueCd == analogueCode);
            if (analogueInfo != null)
            {
                analogueName = analogueInfo.AnalogueName ?? string.Empty;
            }
            return analogueName;
        }

        public string FindClassName(string classCd)
        {
            string className = string.Empty;
            var classInfo = NoTrackingDataContext.M56DrugClass.FirstOrDefault(i => i.ClassCd == classCd);
            if (classInfo != null)
            {
                className = classInfo.ClassName ?? string.Empty;
            }
            return className;
        }

        public string FindComponentName(string conponentCode)
        {
            string componentName = string.Empty;
            var componentInfo = NoTrackingDataContext.M56ExIngCode.FirstOrDefault(i => i.SeibunCd == conponentCode && i.SeibunIndexCd == "000");
            if (componentInfo != null)
            {
                componentName = componentInfo.SeibunName ?? string.Empty;
            }
            return componentName;
        }

        public string FindDiseaseComment(string commentCode)
        {
            string diseaseComment = string.Empty;
            var diseaseCommentInfo = NoTrackingDataContext.M42ContraCmt.FirstOrDefault(i => i.CmtCd == commentCode);
            if (diseaseCommentInfo != null)
            {
                diseaseComment = diseaseCommentInfo.Cmt ?? string.Empty;
            }
            return diseaseComment;
        }

        public string FindDiseaseName(string byotaiCd)
        {
            string diseaseName = string.Empty;
            var diseaseInfo = NoTrackingDataContext.M42ContraindiDisCon.FirstOrDefault(i => i.ByotaiCd == byotaiCd);
            if (diseaseInfo != null)
            {
                diseaseName = diseaseInfo.Byomei ?? string.Empty;
            }
            return diseaseName;
        }

        public string FindDrvalrgyName(string drvalrgyCode)
        {
            string drvalrgyName = string.Empty;
            var drvalrgyInfo = NoTrackingDataContext.M56DrvalrgyCode.FirstOrDefault(i => i.DrvalrgyCd == drvalrgyCode);
            if (drvalrgyInfo != null)
            {
                drvalrgyName = drvalrgyInfo.DrvalrgyName ?? string.Empty;
            }
            return drvalrgyName;
        }

        public string FindFoodName(string foodCode)
        {
            string foodName = string.Empty;
            var foodInfo = NoTrackingDataContext.M12FoodAlrgyKbn.FirstOrDefault(i => i.FoodKbn == foodCode);
            if (foodInfo != null)
            {
                foodName = foodInfo.FoodName ?? string.Empty;
            }
            return foodName;
        }

        public string FindIppanNameByIppanCode(string ippanCode)
        {
            var ippanInfo = NoTrackingDataContext.IpnNameMsts.FirstOrDefault(i => i.IpnNameCd == ippanCode);
            return ippanInfo == null ? string.Empty : ippanInfo.IpnName ?? string.Empty;
        }

        public string FindItemName(string yjCd, int sinday)
        {
            var itemInfo = NoTrackingDataContext.TenMsts.FirstOrDefault(d => d.StartDate <= sinday && sinday <= d.EndDate && d.YjCd == yjCd);
            return itemInfo != null ? itemInfo.Name ?? string.Empty : string.Empty;
        }

        public string FindItemNameByItemCode(string itemCd, int sinday)
        {
            var itemInfo = NoTrackingDataContext.TenMsts.FirstOrDefault(t => t.ItemCd == itemCd && t.StartDate <= sinday && sinday <= t.EndDate);
            return itemInfo != null ? itemInfo.Name ?? string.Empty : string.Empty;
        }

        public string FindKijyoComment(string commentCode)
        {
            string kijyoComment = string.Empty;
            var kijyoCommentInfo = NoTrackingDataContext.M01KijyoCmt.FirstOrDefault(i => i.CmtCd == commentCode);
            if (kijyoCommentInfo != null)
            {
                kijyoComment = kijyoCommentInfo.Cmt ?? string.Empty;
            }
            return kijyoComment;
        }

        public string FindKinkiComment(string commentCode)
        {
            string kinkiComment = string.Empty;
            var kinkiCommentInfo = NoTrackingDataContext.M01KinkiCmt.FirstOrDefault(i => i.CmtCd == commentCode);
            if (kinkiCommentInfo != null)
            {
                kinkiComment = kinkiCommentInfo.Cmt ?? string.Empty;
            }
            return kinkiComment;
        }

        public string FindOTCItemName(int serialNum)
        {
            string oTCITemName = string.Empty;
            var oTCITemNameInfo = NoTrackingDataContext.M38OtcMain.FirstOrDefault(i => i.SerialNum == serialNum);
            if (oTCITemNameInfo != null)
            {
                oTCITemName = oTCITemNameInfo.TradeName ?? string.Empty;
            }
            return oTCITemName;
        }

        public string FindSuppleItemName(string seibunCd)
        {
            string suppleItemName = string.Empty;
            var suppleItemNameInfo = NoTrackingDataContext.M41SuppleIngres.FirstOrDefault(i => i.SeibunCd == seibunCd);
            if (suppleItemNameInfo != null)
            {
                suppleItemName = suppleItemNameInfo.Seibun ?? string.Empty;
            }
            return suppleItemName;
        }

        public string GetOTCComponentInfo(string seibunCd)
        {
            var otcComponentInfo = NoTrackingDataContext.M38IngCode.FirstOrDefault(i => i.SeibunCd == seibunCd);
            return otcComponentInfo != null ? otcComponentInfo.Seibun ?? string.Empty : string.Empty;
        }

        public string GetSupplementComponentInfo(string seibunCd)
        {
            var supplementComponentInfo = NoTrackingDataContext.M41SuppleIngres.FirstOrDefault(i => i.SeibunCd == seibunCd);
            return supplementComponentInfo != null ? supplementComponentInfo.Seibun ?? string.Empty : string.Empty;
        }

        public string GetUsageDosage(string yjCd)
        {
            var dosageInfo =
                  (from dosageDrug in NoTrackingDataContext.DosageDrugs.Where(d => d.YjCd == yjCd)
                   join dosageDosage in NoTrackingDataContext.DosageDosages.Where(d => !string.IsNullOrEmpty(d.UsageDosage))
                   on dosageDrug.DoeiCd equals dosageDosage.DoeiCd
                   select new
                   {
                       UsageDosage = dosageDosage.UsageDosage == null ? string.Empty : dosageDosage.UsageDosage.Replace("；", Environment.NewLine)
                   }
                  ).ToList().FirstOrDefault();
            return dosageInfo != null ? dosageInfo.UsageDosage : string.Empty;
        }

        public bool IsNoMasterData()
        {
            return NoTrackingDataContext.M56ExEdIngredients.Count() == 0;
        }
    }
}
