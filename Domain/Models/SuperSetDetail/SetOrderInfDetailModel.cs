using Domain.Models.OrdInfDetails;
using Helper.Constants;

namespace Domain.Models.SuperSetDetail;

public class SetOrderInfDetailModel
{
    public SetOrderInfDetailModel(int sinKouiKbn, string itemCd, string itemName, string displayItemName, double suryo, string unitName, int unitSBT, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string kokuji1, string kokuji2, int isNodspRece, string ipnCd, string ipnName, string bunkatu, string cmtName, string cmtOpt, string fontColor, int commentNewline, string centerItemCd1, string centerItemCd2, int kasan1, int kasan2, List<YohoSetMstModel> yohoSets)
    {
        SinKouiKbn = sinKouiKbn;
        ItemCd = itemCd;
        ItemName = itemName;
        DisplayItemName = displayItemName;
        Suryo = suryo;
        UnitName = unitName;
        UnitSBT = unitSBT;
        TermVal = termVal;
        KohatuKbn = kohatuKbn;
        SyohoKbn = syohoKbn;
        SyohoLimitKbn = syohoLimitKbn;
        DrugKbn = drugKbn;
        YohoKbn = yohoKbn;
        Kokuji1 = kokuji1;
        Kokuji2 = kokuji2;
        IsNodspRece = isNodspRece;
        IpnCd = ipnCd;
        IpnName = ipnName;
        Bunkatu = bunkatu;
        CmtName = cmtName;
        CmtOpt = cmtOpt;
        FontColor = fontColor;
        CommentNewline = commentNewline;
        CenterItemCd1 = centerItemCd1;
        CenterItemCd2 = centerItemCd2;
        Kasan1 = kasan1;
        Kasan2 = kasan2;
        YohoSets = yohoSets;
    }

    public SetOrderInfDetailModel(int hpId, int setCd, long rpNo, long rpEdaNo, int rowNo, int sinKouiKbn, string itemCd, string itemName, string displayItemName, double suryo, string unitName, int unitSBT, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string kokuji1, string kokuji2, int isNodspRece, string ipnCd, string ipnName, string bunkatu, string cmtName, string cmtOpt, string fontColor, int commentNewline, string masterSbt, int inOutKbn, double yakka, bool isGetPriceInYakka, double ten, int bunkatuKoui, int alternationIndex, int kensaGaichu, double odrTermVal, double cnvTermVal, string yjCd, string centerItemCd1, string centerItemCd2, int kasan1, int kasan2, List<YohoSetMstModel> yohoSets)
    {
        HpId = hpId;
        SetCd = setCd;
        RpNo = rpNo;
        RpEdaNo = rpEdaNo;
        RowNo = rowNo;
        SinKouiKbn = sinKouiKbn;
        ItemCd = itemCd;
        ItemName = itemName;
        DisplayItemName = displayItemName;
        Suryo = suryo;
        UnitName = unitName;
        UnitSBT = unitSBT;
        TermVal = termVal;
        KohatuKbn = kohatuKbn;
        SyohoKbn = syohoKbn;
        SyohoLimitKbn = syohoLimitKbn;
        DrugKbn = drugKbn;
        YohoKbn = yohoKbn;
        Kokuji1 = kokuji1;
        Kokuji2 = kokuji2;
        IsNodspRece = isNodspRece;
        IpnCd = ipnCd;
        IpnName = ipnName;
        Bunkatu = bunkatu;
        CmtName = cmtName;
        CmtOpt = cmtOpt;
        FontColor = fontColor;
        CommentNewline = commentNewline;
        MasterSbt = masterSbt;
        InOutKbn = inOutKbn;
        Yakka = yakka;
        IsGetPriceInYakka = isGetPriceInYakka;
        Ten = ten;
        BunkatuKoui = bunkatuKoui;
        AlternationIndex = alternationIndex;
        KensaGaichu = kensaGaichu;
        OdrTermVal = odrTermVal;
        CnvTermVal = cnvTermVal;
        YjCd = yjCd;
        CenterItemCd1 = centerItemCd1;
        CenterItemCd2 = centerItemCd2;
        Kasan1 = kasan1;
        Kasan2 = kasan2;
        YohoSets = yohoSets;
    }

    public int HpId { get; private set; }

    public int SetCd { get; private set; }

    public long RpNo { get; private set; }

    public long RpEdaNo { get; private set; }

    public int RowNo { get; private set; }

    public int SinKouiKbn { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public string DisplayItemName { get; private set; }

    public double Suryo { get; private set; }

    public string UnitName { get; private set; }

    public int UnitSBT { get; private set; }

    public double TermVal { get; private set; }

    public int KohatuKbn { get; private set; }

    public int SyohoKbn { get; private set; }

    public int SyohoLimitKbn { get; private set; }

    public int DrugKbn { get; private set; }

    public int YohoKbn { get; private set; }

    public string Kokuji1 { get; private set; }

    public string Kokuji2 { get; private set; }

    public int IsNodspRece { get; private set; }

    public string IpnCd { get; private set; }

    public string IpnName { get; private set; }

    public string Bunkatu { get; private set; }

    public string CmtName { get; private set; }

    public string CmtOpt { get; private set; }

    public string FontColor { get; private set; }

    public int CommentNewline { get; private set; }

    public string MasterSbt { get; private set; } = string.Empty;

    public int InOutKbn { get; private set; }

    public double Yakka { get; private set; }

    public bool IsGetPriceInYakka { get; private set; }

    public double Ten { get; private set; }

    public int BunkatuKoui { get; private set; }

    public int AlternationIndex { get; private set; }

    public int KensaGaichu { get; private set; }

    public double OdrTermVal { get; private set; }

    public double CnvTermVal { get; private set; }

    public string YjCd { get; private set; } = string.Empty;

    public string CenterItemCd1 { get; private set; } = string.Empty;

    public string CenterItemCd2 { get; private set; } = string.Empty;

    public int Kasan1 { get; private set; }

    public int Kasan2 { get; private set; }

    public List<YohoSetMstModel> YohoSets { get; private set; }

    //Exposed properties
    public double Price
    {
        get
        {
            if (InOutKbn == 1 && IsGetPriceInYakka && SyohoKbn == 3 && Yakka > 0)
            {
                return Yakka;
            }
            return Ten;
        }
    }

    public bool IsSpecialItem
    {
        get => MasterSbt == "S"
            && SinKouiKbn == 20
            && DrugKbn == 0
            && ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu;
    }

    public bool IsYoho
    {
        get => YohoKbn > 0;
    }

    public bool IsKensa
    {
        get => SinKouiKbn == 61 || SinKouiKbn == 64;
    }

    public bool Is820Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment820Pattern);

    public bool Is830Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment830Pattern);

    public bool Is831Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment831Pattern);

    public bool Is850Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment850Pattern);

    public bool Is851Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment851Pattern);

    public bool Is852Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment852Pattern);

    public bool Is840Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment840Pattern) && ItemCd != ItemCdConst.GazoDensibaitaiHozon;

    public bool Is842Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment842Pattern);

    public bool IsShohoComment => SinKouiKbn == 100;

    public bool IsShohoBiko => SinKouiKbn == 101;

    public bool IsDrug
    {
        get => (SinKouiKbn == 20 && DrugKbn > 0) || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai
            || (SinKouiKbn == 20 && ItemCd.StartsWith("Z"));
    }

    public bool IsInjection
    {
        get => SinKouiKbn == 30;
    }

    public bool IsDrugUsage
    {
        get => YohoKbn > 0 || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai;
    }

    public bool IsStandardUsage
    {
        get => YohoKbn == 1 || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai;
    }

    public bool IsSuppUsage
    {
        get => YohoKbn == 2;
    }

    public bool IsInjectionUsage
    {
        get => (SinKouiKbn >= 31 && SinKouiKbn <= 34) || (SinKouiKbn == 30 && ItemCd.StartsWith("Z") && MasterSbt == "S");
    }

    /// <summary>
    /// Comment input with // or ..
    /// </summary>
    public bool IsNormalComment => !string.IsNullOrEmpty(ItemName) && string.IsNullOrEmpty(ItemCd);

    public bool IsComment
    {
        get => IsNormalComment || Is820Cmt || Is830Cmt || Is831Cmt || Is840Cmt || Is842Cmt || Is850Cmt || Is851Cmt || Is852Cmt;
    }

    public bool IsUsage
    {
        get => IsStandardUsage || IsSuppUsage || IsInjectionUsage;
    }

    public string DisplayedQuantity
    {
        get
        {
            // If item don't have UniName => No quantity displayed
            if (string.IsNullOrEmpty(UnitName))
            {
                return string.Empty;
            }
            return Suryo != 0 && ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu ? Suryo.ToString() : string.Empty;
        }
    }

    public string EditingQuantity
    {
        get
        {
            if (string.IsNullOrEmpty(ItemCd)) return string.Empty;

            if (ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
            {
                return Bunkatu;
            }
            else if (Is840Cmt)
            {
                return CmtOpt;
            }
            else if (Is842Cmt)
            {
                return CmtOpt;
            }
            else if (Is850Cmt)
            {
                return CmtOpt;
            }
            else if (Is851Cmt)
            {
                return CmtOpt;
            }
            else if (Is852Cmt)
            {
                return CmtOpt;
            }
            else
            {
                return DisplayedQuantity;
            }
        }
    }


}
