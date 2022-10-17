using Domain.Types;

namespace Domain.Models.SuperSetDetail;

public class SetGroupOrderInfModel
{
    public SetGroupOrderInfModel(int kouiCode, GroupKoui groupKouiCode, string groupName, bool isShowInOut, int inOutKbn, string inOutName, bool isShowSikyu, int sikyuKbn, int tosekiKbn, string sikyuName, bool isShowSantei, int santeiKbn, string santeiName, int syohoSbt, bool isDrug, bool isKensa, List<SetOrderInfModel> setOrdInfModels)
    {
        KouiCode = kouiCode;
        GroupKouiCode = groupKouiCode;
        GroupName = groupName;
        IsShowInOut = isShowInOut;
        InOutKbn = inOutKbn;
        InOutName = inOutName;
        IsShowSikyu = isShowSikyu;
        SikyuKbn = sikyuKbn;
        TosekiKbn = tosekiKbn;
        SikyuName = sikyuName;
        IsShowSantei = isShowSantei;
        SanteiKbn = santeiKbn;
        SanteiName = santeiName;
        SyohoSbt = syohoSbt;
        IsDrug = isDrug;
        IsKensa = isKensa;
        ListSetOrdInfModels = setOrdInfModels;
    }
    public string GUID { get; } = Guid.NewGuid().ToString();
    public int KouiCode { get; private set; }
    public GroupKoui GroupKouiCode { get; private set; }
    public string GroupName { get; private set; }
    public bool IsShowInOut { get; private set; }
    public int InOutKbn { get; private set; }
    public string InOutName { get; private set; }
    public bool IsShowSikyu { get; private set; }
    public int SikyuKbn { get; private set; }
    public int TosekiKbn { get; private set; }
    public string SikyuName { get; private set; }
    public bool IsShowSantei { get; private set; }
    public int SanteiKbn { get; private set; }
    public string SanteiName { get; private set; }
    public int SyohoSbt { get; private set; }
    public bool IsDrug { get; private set; }
    public bool IsKensa { get; private set; }
    public List<SetOrderInfModel> ListSetOrdInfModels { get; private set; }
}
