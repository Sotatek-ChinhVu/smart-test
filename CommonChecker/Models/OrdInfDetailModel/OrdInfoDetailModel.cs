
using CommonChecker.Types;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace CommonChecker.Models.OrdInfDetailModel
{
    public class OrdInfoDetailModel : IOdrInfoDetailModel
    {
        public int HpId { get; set; }
        public long RaiinNo { get; set; }
        public long RpNo { get; set; }
        public long RpEdaNo { get; set; }
        public int RowNo { get; set; }
        public long PtId { get; set; }
        public int SinDate { get; set; }
        public int SinKouiKbn { get; set; }
        public string ItemCd { get; set; }
        public string ItemName { get; set; }
        public double Suryo { get; set; }
        public string UnitName { get; set; }
        public int UnitSbt { get; set; }
        public double TermVal { get; set; }
        public int KohatuKbn { get; set; }
        public int SyohoKbn { get; set; }
        public int SyohoLimitKbn { get; set; }
        public int DrugKbn { get; set; }
        public int YohoKbn { get; set; }
        public string Kokuji1 { get; set; }
        public string Kokuji2 { get; set; }
        public int IsNodspRece { get; set; }
        public string IpnCd { get; set; }
        public string IpnName { get; set; }
        public int JissiKbn { get; set; }
        public DateTime JissiDate { get; set; }
        public int JissiId { get; set; }
        public string JissiMachine { get; set; }
        public string ReqCd { get; set; }
        public string Bunkatu { get; set; }
        public string CmtName { get; set; }
        public string CmtOpt { get; set; }
        public string FontColor { get; set; }
        public int CommentNewline { get; set; }
        public string MasterSbt { get; set; }
        public int InOutKbn { get; set; }
        public double Yakka { get; set; }
        public bool IsGetPriceInYakka { get; set; }
        public double Ten { get; set; }
        public int BunkatuKoui { get; set; }
        public int AlternationIndex { get; set; }
        public int KensaGaichu { get; set; }
        public int RefillSetting { get; set; }
        public int CmtCol1 { get; set; }
        public double OdrTermVal { get; set; }
        public double CnvTermVal { get; set; }
        public string YjCd { get; set; }
        public int Kasan1 { get; set; }
        public int Kasan2 { get; set; }


        public OrdInfoDetailModel(int hpId, long raiinNo, long rpNo, long rpEdaNo, int rowNo, long ptId, int sinDate, int sinKouiKbn, string itemCd, string itemName, double suryo, string unitName, int unitSbt, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string kokuji1, string kokuji2, int isNodspRece, string ipnCd, string ipnName, int jissiKbn, DateTime jissiDate, int jissiId, string jissiMachine, string reqCd, string bunkatu, string cmtName, string cmtOpt, string fontColor, int commentNewline, string masterSbt, int inOutKbn, double yakka, bool isGetPriceInYakka, int refillSetting, int cmtCol1, double ten, int bunkatuKoui, int alternationIndex, int kensaGaichu, double odrTermVal, double cnvTermVal, string yjCd, int kasan1, int kasan2)
        {
            HpId = hpId;
            RaiinNo = raiinNo;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            RowNo = rowNo;
            PtId = ptId;
            SinDate = sinDate;
            SinKouiKbn = sinKouiKbn;
            ItemCd = itemCd;
            ItemName = itemName;
            Suryo = suryo;
            UnitName = unitName;
            UnitSbt = unitSbt;
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
            JissiKbn = jissiKbn;
            JissiDate = jissiDate;
            JissiId = jissiId;
            JissiMachine = jissiMachine;
            ReqCd = reqCd;
            Bunkatu = bunkatu;
            CmtName = cmtName;
            CmtOpt = cmtOpt;
            FontColor = fontColor;
            CommentNewline = commentNewline;
            MasterSbt = masterSbt;
            InOutKbn = inOutKbn;
            Yakka = yakka;
            IsGetPriceInYakka = isGetPriceInYakka;
            RefillSetting = refillSetting;
            CmtCol1 = cmtCol1;
            Ten = ten;
            BunkatuKoui = bunkatuKoui;
            AlternationIndex = alternationIndex;
            KensaGaichu = kensaGaichu;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            YjCd = yjCd;
            Kasan1 = kasan1;
            Kasan2 = kasan2;
        }

        public bool IsSpecialItem
        {
            get => MasterSbt == "S" && SinKouiKbn == 20 && DrugKbn == 0 && ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu;
        }

        public bool IsDrugUsage
        {
            get => YohoKbn > 0 || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai;
        }

        public bool IsDrug
        {
            get => (SinKouiKbn == 20 && DrugKbn > 0) || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai
                || (SinKouiKbn == 20 && ItemCd.StartsWith("Z"));
        }

        public bool IsInjection
        {
            get => SinKouiKbn == 30;
        }

        public bool Is820Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment820Pattern);

        public bool Is830Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment830Pattern);

        public bool Is831Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment831Pattern);

        public bool Is850Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment850Pattern);

        public bool Is851Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment851Pattern);

        public bool Is852Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment852Pattern);

        public bool Is853Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment853Pattern);

        public bool Is840Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment840Pattern) && ItemCd != ItemCdConst.GazoDensibaitaiHozon;

        public bool Is842Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment842Pattern);

        public bool Is880Cmt => ItemCd != null && ItemCd.StartsWith(ItemCdConst.Comment880Pattern);

        public bool IsShohoComment => SinKouiKbn == 100;

        public bool IsShohoBiko => SinKouiKbn == 101;

        public bool IsStandardUsage
        {
            get => YohoKbn == 1 || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai;
        }

        public bool IsInjectionUsage
        {
            get => (SinKouiKbn >= 31 && SinKouiKbn <= 34) || (SinKouiKbn == 30 && ItemCd.StartsWith("Z") && MasterSbt == "S");
        }

        public bool IsSuppUsage
        {
            get => YohoKbn == 2;
        }

        public string DisplayedUnit
        {
            get => Suryo.AsDouble() != 0 ? UnitName.AsString() : "";
        }

        public string DisplayedQuantity
        {
            get
            {
                // If item don't have UniName => No quantity displayed
                if (string.IsNullOrEmpty(DisplayedUnit))
                {
                    return string.Empty;
                }
                return Suryo.AsDouble() != 0 && ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu ? Suryo.AsDouble().AsString() : "";
            }
        }

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

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(ItemCd) &&
                       string.IsNullOrEmpty(ItemName.Trim()) &&
                       SinKouiKbn == 0;
            }
        }

        public string DisplayItemName
        {
            get
            {
                if (ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu)
                {
                    return ItemName + TenUtils.GetBunkatu(BunkatuKoui, Bunkatu);
                }
                else if (Is840Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is842Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is830Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is831Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is850Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is851Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is852Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is853Cmt)
                {
                    return "" + ItemName;
                }
                else if (Is880Cmt)
                {
                    return "" + ItemName;
                }
                else if (string.IsNullOrEmpty(ItemCd) && !IsShohoComment && !IsShohoBiko)
                {
                    return "" + ItemName;
                }
                return ItemName;
            }
        }

        public bool IsUsage
        {
            get => IsStandardUsage || IsSuppUsage || IsInjectionUsage;
        }

        public ReleasedDrugType ReleasedType
        {
            get => CIUtil.SyohoToSempatu(SyohoKbn, SyohoLimitKbn);
        }
    }
}
