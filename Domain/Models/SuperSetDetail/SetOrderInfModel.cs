using Domain.Models.OrdInfs;

namespace Domain.Models.SuperSetDetail;

public class SetOrderInfModel
{
    public SetOrderInfModel()
    {
        Id = 0;
        HpId = 0;
        SetCd = 0;
        RpNo = 0;
        RpEdaNo = 0;
        OdrKouiKbn = 0;
        RpName = String.Empty;
        InoutKbn = 0;
        SikyuKbn = 0;
        SyohoSbt = 0;
        SanteiKbn = 0;
        TosekiKbn = 0;
        DaysCnt = 0;
        SortNo = 0;
        IsDeleted = 0;
        CreateDate = DateTime.MinValue;
        CreateId = 0;
        CreateName = string.Empty;
        GroupKoui = GroupKoui.From(0);
        OrdInfDetails = new List<SetOrderInfDetailModel>();
    }

    public SetOrderInfModel(long id, int hpId, int setCd, long rpNo, long rpEdaNo, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, int isDeleted, DateTime createDate, int createId, string createName, GroupKoui groupKoui, List<SetOrderInfDetailModel> ordInfDetails)
    {
        Id = id;
        HpId = hpId;
        SetCd = setCd;
        RpNo = rpNo;
        RpEdaNo = rpEdaNo;
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
        CreateDate = createDate;
        CreateId = createId;
        CreateName = createName;
        GroupKoui = groupKoui;
        OrdInfDetails = ordInfDetails;
    }

    public SetOrderInfModel(long id, long rpNo, long rpEdaNo, int odrKouiKbn, string rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, int isDeleted, List<SetOrderInfDetailModel> ordInfDetails)
    {
        Id = id;
        RpNo = rpNo;
        RpEdaNo = rpEdaNo;
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
        OrdInfDetails = ordInfDetails;
    }

    public long Id { get; private set; }

    public int HpId { get; private set; }

    public int SetCd { get; private set; }

    public long RpNo { get; private set; }

    public long RpEdaNo { get; private set; }

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

    public DateTime CreateDate { get; private set; }

    public int CreateId { get; private set; }

    public string CreateName { get; private set; } = string.Empty;

    public GroupKoui GroupKoui { get; private set; } = GroupKoui.From(0);

    /// <summary>
    /// 自己注射 - Self-Injection
    /// </summary>
    public bool IsSelfInjection => OdrKouiKbn == 28;

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

    /// <summary>
    /// 検査 - Inspection
    /// </summary>
    public bool IsKensa
    {
        get
        {
            return OdrKouiKbn >= 60 && OdrKouiKbn <= 64;
        }
    }

    public bool IsShohoComment
    {
        get => OdrKouiKbn == 100;
    }

    public bool IsShohoBiko
    {
        get => OdrKouiKbn == 101;
    }

    public bool IsShohosenComment
    {
        get => IsShohoComment || IsShohoBiko;
    }

    public List<SetOrderInfDetailModel> OrdInfDetails { get; private set; }
}
