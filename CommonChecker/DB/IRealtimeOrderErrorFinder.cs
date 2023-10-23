using Domain.Common;

namespace CommonChecker.DB
{
    public interface IRealtimeOrderErrorFinder :IRepositoryBase
    {
        bool IsNoMasterData();

        string FindItemName(string yjCd, int sinday);

        Dictionary<string, string> FindItemNameDic(List<string> yjCdList, int sinday);

        string FindComponentName(string conponentCode);

        Dictionary<string, string> FindComponentNameDic(List<string> conponentCodeList);

        string FindAnalogueName(string analogueCode);

        Dictionary<string, string> FindAnalogueNameDic(List<string> analogueCodeList);

        string FindDrvalrgyName(string drvalrgyCode);

        Dictionary<string, string> FindDrvalrgyNameDic(List<string> drvalrgyCodeList);

        string FindItemNameByItemCode(string itemCd, int sinday);

        Dictionary<string, string> FindItemNameByItemCodeDic(List<string> itemCdList, int sinday);

        string FindFoodName(string foodCode);

        Dictionary<string, string> FindFoodNameDic(List<string> foodCodeList);

        string FindAgeComment(string commentCode);

        string FindDiseaseName(string byotaiCd);

        Dictionary<string, string> FindDiseaseNameDic(List<string> byotaiCdList);

        string FindDiseaseComment(string commentCode);

        string FindOTCItemName(int serialNum);

        Dictionary<string, string> FindOTCItemNameDic(List<string> serialNumList);

        string FindKinkiComment(string commentCode);

        Dictionary<string, string> FindKinkiCommentDic(List<string> commentCodeList);

        string FindKijyoComment(string commentCode);

        Dictionary<string, string> FindKijyoCommentDic(List<string> commentCodeList);

        string FindSuppleItemName(string seibunCd);

        Dictionary<string, string> FindSuppleItemNameDic(List<string> seibunCdList);

        string GetOTCComponentInfo(string seibunCd);

        Dictionary<string, string> GetOTCComponentInfoDic(List<string> seibunCdList);

        string GetSupplementComponentInfo(string seibunCd);

        Dictionary<string, string> GetSupplementComponentInfoDic(List<string> seibunCdList);

        string FindClassName(string classCd);

        string FindIppanNameByIppanCode(string ippanCode);

        string GetUsageDosage(string yjCd);

        Dictionary<string, string> GetUsageDosageDic(List<string> yjCdList);
    }
}
