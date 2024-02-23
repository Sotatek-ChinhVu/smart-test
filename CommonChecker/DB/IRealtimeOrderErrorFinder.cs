using Domain.Common;
using Domain.Models.PatientInfor;

namespace CommonChecker.DB
{
    public interface IRealtimeOrderErrorFinder : IRepositoryBase
    {
        bool IsNoMasterData(int hpId);

        string FindItemName(int hpId, string yjCd, int sinday);

        Dictionary<string, string> FindItemNameDic(int hpId, List<string> yjCdList, int sinday);

        string FindComponentName(int hpId, string conponentCode);

        Dictionary<string, string> FindComponentNameDic(int hpId, List<string> conponentCodeList);

        string FindAnalogueName(int hpId, string analogueCode);

        Dictionary<string, string> FindAnalogueNameDic(int hpId, List<string> analogueCodeList);

        string FindDrvalrgyName(int hpId, string drvalrgyCode);

        Dictionary<string, string> FindDrvalrgyNameDic(int hpId, List<string> drvalrgyCodeList);

        string FindItemNameByItemCode(int hpId, string itemCd, int sinday);

        Dictionary<string, string> FindItemNameByItemCodeDic(int hpId, List<string> itemCdList, int sinday);

        string FindFoodName(int hpId, string foodCode);

        Dictionary<string, string> FindFoodNameDic(int hpId, List<string> foodCodeList);

        string FindAgeComment(int hpId, string commentCode);

        string FindDiseaseName(int hpId, string byotaiCd);

        Dictionary<string, string> FindDiseaseNameDic(int hpId, List<string> byotaiCdList);

        string FindDiseaseComment(int hpId, string commentCode);

        string FindOTCItemName(int hpId, int serialNum);

        Dictionary<string, string> FindOTCItemNameDic(int hpId, List<string> serialNumList);

        string FindKinkiComment(int hpId, string commentCode);

        Dictionary<string, string> FindKinkiCommentDic(int hpId, List<string> commentCodeList);

        string FindKijyoComment(int hpId, string commentCode);

        Dictionary<string, string> FindKijyoCommentDic(int hpId, List<string> commentCodeList);

        string FindSuppleItemName(int hpId, string seibunCd);

        Dictionary<string, string> FindSuppleItemNameDic(int hpId, List<string> seibunCdList);

        string GetOTCComponentInfo(int hpId, string seibunCd);

        Dictionary<string, string> GetOTCComponentInfoDic(int hpId, List<string> seibunCdList);

        string GetSupplementComponentInfo(int hpId, string seibunCd);

        Dictionary<string, string> GetSupplementComponentInfoDic(int hpId, List<string> seibunCdList);

        string FindClassName(int hpId, string classCd);

        string FindIppanNameByIppanCode(string ippanCode);

        string GetUsageDosage(int hpId, string yjCd);

        Dictionary<string, string> GetUsageDosageDic(int hpId, List<string> yjCdList);

        PatientInforModel? FindPatientByPtId(int hpId, long ptId);
    }
}
