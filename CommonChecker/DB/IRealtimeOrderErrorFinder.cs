using Domain.Common;

namespace CommonChecker.DB
{
    public interface IRealtimeOrderErrorFinder : IRepositoryBase
    {
        bool IsNoMasterData(int hpId);

        string FindItemName(string yjCd, int sinday);

        Dictionary<string, string> FindItemNameDic(List<string> yjCdList, int sinday);

        string FindComponentName(int hpId, string conponentCode);

        Dictionary<string, string> FindComponentNameDic(int hpId, List<string> conponentCodeList);

        string FindAnalogueName(int hpId, string analogueCode);

        Dictionary<string, string> FindAnalogueNameDic(int hpId, List<string> analogueCodeList);

        string FindDrvalrgyName(int hpId, string drvalrgyCode);

        Dictionary<string, string> FindDrvalrgyNameDic(int hpId, List<string> drvalrgyCodeList);

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

        string FindClassName(int hpId, string classCd);

        string FindIppanNameByIppanCode(string ippanCode);

        string GetUsageDosage(int hpId, string yjCd);

        Dictionary<string, string> GetUsageDosageDic(int hpId, List<string> yjCdList);
    }
}
