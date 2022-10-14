using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using static Helper.Constants.OrderInfConst;

namespace Domain.Models.OrdInfDetails
{
    public class OrdInfDetailModel
    {
        public int HpId { get; private set; }
        public long RaiinNo { get; private set; }
        public long RpNo { get; private set; }
        public long RpEdaNo { get; private set; }
        public int RowNo { get; private set; }
        public long PtId { get; private set; }
        public int SinDate { get; private set; }
        public int SinKouiKbn { get; private set; }
        public string ItemCd { get; private set; }
        public string ItemName { get; private set; }
        public double Suryo { get; private set; }
        public string UnitName { get; private set; }
        public int UnitSbt { get; private set; }
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
        public int JissiKbn { get; private set; }
        public DateTime JissiDate { get; private set; }
        public int JissiId { get; private set; }
        public string JissiMachine { get; private set; }
        public string ReqCd { get; private set; }
        public string Bunkatu { get; private set; }
        public string CmtName { get; private set; }
        public string CmtOpt { get; private set; }
        public string FontColor { get; private set; }
        public int CommentNewline { get; private set; }
        public string MasterSbt { get; private set; }
        public int InOutKbn { get; private set; }
        public double Yakka { get; private set; }
        public bool IsGetPriceInYakka { get; private set; }
        public double Ten { get; private set; }
        public int BunkatuKoui { get; private set; }
        public int AlternationIndex { get; private set; }
        public int KensaGaichu { get; private set; }
        public int RefillSetting { get; private set; }
        public int CmtCol1 { get; private set; }
        public double OdrTermVal { get; private set; }
        public double CnvTermVal { get; private set; }
        public string YjCd { get; private set; }
        public List<YohoSetMstModel> YohoSets { get; private set; }
        public int Kasan1 { get; private set; }
        public int Kasan2 { get; private set; }


        public OrdInfDetailModel(int hpId, long raiinNo, long rpNo, long rpEdaNo, int rowNo, long ptId, int sinDate, int sinKouiKbn, string itemCd, string itemName, double suryo, string unitName, int unitSbt, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string kokuji1, string kokuji2, int isNodspRece, string ipnCd, string ipnName, int jissiKbn, DateTime jissiDate, int jissiId, string jissiMachine, string reqCd, string bunkatu, string cmtName, string cmtOpt, string fontColor, int commentNewline, string masterSbt, int inOutKbn, double yakka, bool isGetPriceInYakka, int refillSetting, int cmtCol1, double ten, int bunkatuKoui, int alternationIndex, int kensaGaichu, double odrTermVal, double cnvTermVal, string yjCd, List<YohoSetMstModel> yohoSets, int kasan1, int kasan2)
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
            YohoSets = yohoSets;
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

        public OrdInfValidationStatus Validation(int flag)
        {
            #region Validate common

            if (RowNo <= 0)
            {
                return OrdInfValidationStatus.InvalidRowNo;
            }
            if (SinKouiKbn < 0)
            {
                return OrdInfValidationStatus.InvalidSinKouiKbn;
            }
            if (ItemCd.Length > 10)
            {
                return OrdInfValidationStatus.InvalidItemCd;
            }
            if (ItemName.Length > 240)
            {
                return OrdInfValidationStatus.InvalidItemName;
            }
            if (Suryo < 0 || (ItemCd == ItemCdConst.JikanKihon && !(Suryo >= 0 && Suryo <= 7)) || (ItemCd == ItemCdConst.SyosaiKihon && !(Suryo >= 0 && Suryo <= 8)))
            {
                return OrdInfValidationStatus.InvalidSuryo;
            }
            if (UnitName.Length > 24)
            {
                return OrdInfValidationStatus.InvalidUnitName;
            }
            if (!(SyohoKbn >= 0 && SyohoKbn <= 3))
            {
                return OrdInfValidationStatus.InvalidSyohoKbn;
            }
            if (!(YohoKbn >= 0 && YohoKbn <= 2))
            {
                return OrdInfValidationStatus.InvalidYohoKbn;
            }
            if (Bunkatu.Length > 10)
            {
                return OrdInfValidationStatus.InvalidBunkatuLength;
            }
            if (CmtName.Length > 240)
            {
                return OrdInfValidationStatus.InvalidCmtName;
            }
            if (CmtOpt.Length > 38)
            {
                return OrdInfValidationStatus.InvalidCmtOpt;
            }

            if (flag == 0)
            {
                if (HpId <= 0)
                {
                    return OrdInfValidationStatus.InvalidHpId;
                }
                if (RaiinNo <= 0)
                {
                    return OrdInfValidationStatus.InvalidRaiinNo;
                }
                if (RpNo <= 0)
                {
                    return OrdInfValidationStatus.InvalidRpNo;
                }
                if (RpEdaNo <= 0)
                {
                    return OrdInfValidationStatus.InvalidRpEdaNo;
                }
                if (RowNo <= 0)
                {
                    return OrdInfValidationStatus.InvalidRowNo;
                }
                if (PtId <= 0)
                {
                    return OrdInfValidationStatus.InvalidPtId;
                }
                if (SinDate <= 0)
                {
                    return OrdInfValidationStatus.InvalidSinDate;
                }
                if (SinKouiKbn < 0)
                {
                    return OrdInfValidationStatus.InvalidSinKouiKbn;
                }
                if (ItemCd.Length > 10)
                {
                    return OrdInfValidationStatus.InvalidItemCd;
                }
                if (ItemName.Length > 240)
                {
                    return OrdInfValidationStatus.InvalidItemName;
                }
                if (Suryo < 0)
                {
                    return OrdInfValidationStatus.InvalidSuryo;
                }
                if (UnitName.Length > 24)
                {
                    return OrdInfValidationStatus.InvalidUnitName;
                }
                if (UnitSbt != 0 && UnitSbt != 1 && UnitSbt != 2)
                {
                    return OrdInfValidationStatus.InvalidUnitSbt;
                }
                if (TermVal < 0)
                {
                    return OrdInfValidationStatus.InvalidTermVal;
                }
                if (!(SyohoKbn >= 0 && SyohoKbn <= 3))
                {
                    return OrdInfValidationStatus.InvalidSyohoKbn;
                }
                if (!(SyohoLimitKbn >= 0 && SyohoLimitKbn <= 3))
                {
                    return OrdInfValidationStatus.InvalidSyohoLimitKbn;
                }
                if (!(YohoKbn >= 0 && YohoKbn <= 2))
                {
                    return OrdInfValidationStatus.InvalidYohoKbn;
                }
                if (!(IsNodspRece >= 0 && IsNodspRece <= 1))
                {
                    return OrdInfValidationStatus.InvalidIsNodspRece;
                }
                if (IpnCd.Length > 12)
                {
                    return OrdInfValidationStatus.InvalidIpnCd;
                }
                if (IpnName.Length > 120)
                {
                    return OrdInfValidationStatus.InvalidIpnName;
                }
                if (!(JissiKbn >= 0 && JissiKbn <= 1))
                {
                    return OrdInfValidationStatus.InvalidJissiKbn;
                }
                if (JissiId < 0)
                {
                    return OrdInfValidationStatus.InvalidJissiId;
                }
                if (JissiMachine.Length > 60)
                {
                    return OrdInfValidationStatus.InvalidJissiMachine;
                }
                if (ReqCd.Length > 10)
                {
                    return OrdInfValidationStatus.InvalidReqCd;
                }
                if (Bunkatu.Length > 10)
                {
                    return OrdInfValidationStatus.InvalidBunkatuLength;
                }
                if (CmtName.Length > 240)
                {
                    return OrdInfValidationStatus.InvalidCmtName;
                }
                if (CmtOpt.Length > 38)
                {
                    return OrdInfValidationStatus.InvalidCmtOpt;
                }
                if (FontColor.Length > 8)
                {
                    return OrdInfValidationStatus.InvalidFontColor;
                }
                if (!(CommentNewline >= 0 && CommentNewline <= 1))
                {
                    return OrdInfValidationStatus.InvalidCommentNewline;
                }
            }
            #endregion

            #region Validate business

            if ((!string.IsNullOrEmpty(UnitName) && Suryo == 0) || (string.IsNullOrEmpty(UnitName) && Suryo > 0 && ItemCd != ItemCdConst.Con_TouyakuOrSiBunkatu))
            {
                return OrdInfValidationStatus.InvalidSuryo;
            }
            if (!KohatuKbns.ContainsValue(KohatuKbn))
            {
                return OrdInfValidationStatus.InvalidKohatuKbn;
            }

            if (!DrugKbns.ContainsValue(DrugKbn))
            {
                return OrdInfValidationStatus.InvalidDrugKbn;
            }

            if (!string.IsNullOrWhiteSpace(DisplayedUnit))
            {
                if (string.IsNullOrEmpty(DisplayedQuantity))
                {
                    return OrdInfValidationStatus.InvalidQuantityUnit;
                }
                else if (Suryo > 0 && (Price > 0 && Suryo * Price > 999999999))
                {
                    return OrdInfValidationStatus.InvalidPrice;
                }
            }

            if (!string.IsNullOrWhiteSpace(DisplayedUnit) && (YohoKbn == 1 && Suryo > 999))
            {
                return OrdInfValidationStatus.InvalidSuryoAndYohoKbnWhenDisplayedUnitNotNull;
            }

            if (ItemCd == ItemCdConst.Con_TouyakuOrSiBunkatu && (Suryo == 0 && string.IsNullOrWhiteSpace(Bunkatu)))
            {
                return OrdInfValidationStatus.InvalidSuryoBunkatuWhenIsCon_TouyakuOrSiBunkatu;
            }

            if (ItemCd == ItemCdConst.Con_Refill && Suryo > RefillSetting)
            {
                return OrdInfValidationStatus.InvalidSuryoOfReffill;
            }

            if (Is840Cmt && (CmtCol1 > 0 && (string.IsNullOrEmpty(CmtOpt) || string.IsNullOrEmpty(CmtName))))
            {
                return OrdInfValidationStatus.InvalidCmt840;
            }

            if (Is842Cmt)
            {
                if (string.IsNullOrEmpty(CmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    return OrdInfValidationStatus.InvalidCmt842;
                }
                else if (!string.IsNullOrEmpty(CmtOpt) && CmtOpt.Length > 38)
                {
                    return OrdInfValidationStatus.InvalidCmt842CmtOptMoreThan38;
                }
            }

            if (Is830Cmt)
            {
                string fullSpace = @"　";

                if (string.IsNullOrEmpty(CmtOpt) && CmtOpt != fullSpace)
                {
                    return OrdInfValidationStatus.InvalidCmt830CmtOpt;
                }
                else if (!string.IsNullOrEmpty(CmtOpt) && CmtOpt.Length > 38)
                {
                    return OrdInfValidationStatus.InvalidCmt830CmtOptMoreThan38;
                }
            }

            if (Is831Cmt && (string.IsNullOrEmpty(CmtOpt) || string.IsNullOrEmpty(CmtName)))
            {
                return OrdInfValidationStatus.InvalidCmt831;
            }

            if (Is850Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt850(CmtOpt, ItemName);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    if (CmtName.Contains('日'))
                    {
                        return OrdInfValidationStatus.InvalidCmt850Date;
                    }
                    else
                    {
                        return OrdInfValidationStatus.InvalidCmt850OtherDate;
                    }
                }
            }

            if (Is851Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt851(CmtOpt);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    return OrdInfValidationStatus.InvalidCmt851;
                }
            }

            if (Is852Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt852(CmtOpt);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    return OrdInfValidationStatus.InvalidCmt852;
                }
            }

            if (Is853Cmt)
            {
                string cmtOpt = OdrUtil.GetCmtOpt853(CmtOpt, SinDate);
                if (string.IsNullOrEmpty(cmtOpt) || string.IsNullOrEmpty(CmtName))
                {
                    return OrdInfValidationStatus.InvalidCmt853;
                }
            }

            if (Is880Cmt && (string.IsNullOrEmpty(CmtOpt) || string.IsNullOrEmpty(CmtName)))
            {
                return OrdInfValidationStatus.InvalidCmt880;
            }
            #endregion

            return OrdInfValidationStatus.Valid;
        }
    }
}
