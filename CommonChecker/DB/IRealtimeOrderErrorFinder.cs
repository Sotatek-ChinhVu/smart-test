namespace CommonChecker.DB
{
    public interface IRealtimeOrderErrorFinder
    {
        public bool IsNoMasterData();

        public string FindItemName(string yjCd, int sinday);

        public string FindComponentName(string conponentCode);

        public string FindAnalogueName(string analogueCode);

        public string FindDrvalrgyName(string drvalrgyCode);

        public string FindItemNameByItemCode(string itemCd, int sinday);

        public string FindFoodName(string foodCode);

        public string FindAgeComment(string commentCode);

        public string FindDiseaseName(string byotaiCd);

        public string FindDiseaseComment(string commentCode);

        public string FindOTCItemName(int serialNum);

        public string FindKinkiComment(string commentCode);

        public string FindKijyoComment(string commentCode);

        public string FindSuppleItemName(string seibunCd);

        public string GetOTCComponentInfo(string seibunCd);

        public string GetSupplementComponentInfo(string seibunCd);

        public string FindClassName(string classCd);

        public string FindIppanNameByIppanCode(string ippanCode);

        public string GetUsageDosage(string yjCd);
    }
}
