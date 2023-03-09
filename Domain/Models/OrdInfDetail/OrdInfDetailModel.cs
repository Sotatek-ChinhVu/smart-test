using Domain.Types;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using System.Xml.Linq;
using static Helper.Constants.OrderInfConst;

namespace Domain.Models.OrdInfDetails
{
    public class OrdInfDetailModel : IOdrInfDetailModel
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
        public string CnvUnitName { get; private set; }
        public string OdrUnitName { get; private set; }
        public string CenterItemCd1 { get; private set; }
        public string CenterItemCd2 { get; private set; }
        public bool IsDummy { get; private set; }
        public int SinYm
        {
            get
            {
                return SinDate / 100;
            }
        }

        public OrdInfDetailModel(int hpId, long raiinNo, long rpNo, long rpEdaNo, int rowNo, long ptId, int sinDate, int sinKouiKbn, string itemCd, string itemName, double suryo, string unitName, int unitSbt, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string kokuji1, string kokuji2, int isNodspRece, string ipnCd, string ipnName, int jissiKbn, DateTime jissiDate, int jissiId, string jissiMachine, string reqCd, string bunkatu, string cmtName, string cmtOpt, string fontColor, int commentNewline, string masterSbt, int inOutKbn, double yakka, bool isGetPriceInYakka, int refillSetting, int cmtCol1, double ten, int bunkatuKoui, int alternationIndex, int kensaGaichu, double odrTermVal, double cnvTermVal, string yjCd, List<YohoSetMstModel> yohoSets, int kasan1, int kasan2, string cnvUnitName, string odrUnitName, string centerItemCd1, string centerItemCd2)
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
            CnvUnitName = cnvUnitName;
            OdrUnitName = odrUnitName;
            CenterItemCd1 = centerItemCd1;
            CenterItemCd2 = centerItemCd2;
        }

        public OrdInfDetailModel(int hpId, string itemCd, int sinDate)
        {

            ItemName = string.Empty;
            UnitName = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            IpnCd = string.Empty;
            IpnName = string.Empty;

            JissiMachine = string.Empty;
            ReqCd = string.Empty;
            Bunkatu = string.Empty;
            CmtName = string.Empty;
            CmtOpt = string.Empty;
            FontColor = string.Empty;
            MasterSbt = string.Empty;
            YjCd = string.Empty;
            YohoSets = new();
            CnvUnitName = string.Empty;
            OdrUnitName = string.Empty;
            CenterItemCd1 = string.Empty;
            CenterItemCd2 = string.Empty;
            HpId = hpId;
            ItemCd = itemCd;
            SinDate = sinDate;
        }
        public OrdInfDetailModel(int hpId, string itemCd, int sinDate, int suryo)
        {

            ItemName = string.Empty;
            UnitName = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            IpnCd = string.Empty;
            IpnName = string.Empty;

            JissiMachine = string.Empty;
            ReqCd = string.Empty;
            Bunkatu = string.Empty;
            CmtName = string.Empty;
            CmtOpt = string.Empty;
            FontColor = string.Empty;
            MasterSbt = string.Empty;
            YjCd = string.Empty;
            YohoSets = new();
            CnvUnitName = string.Empty;
            OdrUnitName = string.Empty;
            CenterItemCd1 = string.Empty;
            CenterItemCd2 = string.Empty;
            HpId = hpId;
            ItemCd = itemCd;
            SinDate = sinDate;
            Suryo = suryo;
        }

        public OrdInfDetailModel(string itemCd, int sinKouiKbn)
        {

            ItemName = string.Empty;
            UnitName = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            IpnCd = string.Empty;
            IpnName = string.Empty;

            JissiMachine = string.Empty;
            ReqCd = string.Empty;
            Bunkatu = string.Empty;
            CmtName = string.Empty;
            CmtOpt = string.Empty;
            FontColor = string.Empty;
            MasterSbt = string.Empty;
            YjCd = string.Empty;
            YohoSets = new();
            CnvUnitName = string.Empty;
            OdrUnitName = string.Empty;
            CenterItemCd1 = string.Empty;
            CenterItemCd2 = string.Empty;
            HpId = 0;
            ItemCd = itemCd;
            SinDate = 0;
            SinKouiKbn = sinKouiKbn;
        }

        public OrdInfDetailModel(string itemCd, int sinKouiKbn, int drugKbn)
        {

            ItemName = string.Empty;
            UnitName = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            IpnCd = string.Empty;
            IpnName = string.Empty;

            JissiMachine = string.Empty;
            ReqCd = string.Empty;
            Bunkatu = string.Empty;
            CmtName = string.Empty;
            CmtOpt = string.Empty;
            FontColor = string.Empty;
            MasterSbt = string.Empty;
            YjCd = string.Empty;
            YohoSets = new();
            CnvUnitName = string.Empty;
            OdrUnitName = string.Empty;
            CenterItemCd1 = string.Empty;
            CenterItemCd2 = string.Empty;
            HpId = 0;
            ItemCd = itemCd;
            SinDate = 0;
            SinKouiKbn = sinKouiKbn;
            DrugKbn = drugKbn;
        }

        public OrdInfDetailModel(int sinKouiKbn, int suryo)
        {

            ItemName = string.Empty;
            UnitName = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            IpnCd = string.Empty;
            IpnName = string.Empty;
            JissiMachine = string.Empty;
            ReqCd = string.Empty;
            Bunkatu = string.Empty;
            CmtName = string.Empty;
            CmtOpt = string.Empty;
            FontColor = string.Empty;
            MasterSbt = string.Empty;
            YjCd = string.Empty;
            YohoSets = new();
            CnvUnitName = string.Empty;
            OdrUnitName = string.Empty;
            CenterItemCd1 = string.Empty;
            CenterItemCd2 = string.Empty;
            ItemCd = string.Empty;
            SinKouiKbn = sinKouiKbn;
            Suryo = suryo;
        }


        public OrdInfDetailModel(int hpId, int sinDate, long raiinNo, long rpEdaNo, long rpNo, long ptId, string itemCd, double suryo, string itemName)
        {
            ItemName = itemName;
            UnitName = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            IpnCd = string.Empty;
            IpnName = string.Empty;
            JissiMachine = string.Empty;
            ReqCd = string.Empty;
            Bunkatu = string.Empty;
            CmtName = string.Empty;
            CmtOpt = string.Empty;
            FontColor = string.Empty;
            MasterSbt = string.Empty;
            YjCd = string.Empty;
            YohoSets = new();
            CnvUnitName = string.Empty;
            OdrUnitName = string.Empty;
            CenterItemCd1 = string.Empty;
            CenterItemCd2 = string.Empty;
            HpId = hpId;
            ItemCd = itemCd;
            SinDate = sinDate;
            Suryo = suryo;
            RaiinNo = raiinNo;
            RpEdaNo = rpEdaNo;
            RpNo = rpNo;
            PtId = ptId;
        }

        public OrdInfDetailModel(int hpId, long ptId, int sinDate, long raiinNo, string itemCd, string itemName, int sinKouiKbn, long rpNo, int rpEdaNo, int rowNo
                )
        {
            HpId = hpId;
            PtId = ptId;
            SinDate = sinDate;
            RaiinNo = raiinNo;
            ItemCd = itemCd;
            ItemName = itemName;
            SinDate = sinDate;
            SinKouiKbn = sinKouiKbn;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            RowNo = rowNo;
            ItemName = string.Empty;
            UnitName = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            IpnCd = string.Empty;
            IpnName = string.Empty;
            JissiMachine = string.Empty;
            ReqCd = string.Empty;
            Bunkatu = string.Empty;
            CmtName = string.Empty;
            CmtOpt = string.Empty;
            FontColor = string.Empty;
            MasterSbt = string.Empty;
            YjCd = string.Empty;
            YohoSets = new();
            CnvUnitName = string.Empty;
            OdrUnitName = string.Empty;
            CenterItemCd1 = string.Empty;
            CenterItemCd2 = string.Empty;
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
            get => SinKouiKbn == 30 && MasterSbt != "S";
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

                return ItemName;
            }
        }

        public bool HasCmtName => Is830Cmt || Is831Cmt || Is840Cmt || Is842Cmt || Is850Cmt || Is851Cmt || Is852Cmt || Is853Cmt || Is880Cmt;

        public bool IsCommentMaster => Is820Cmt || Is830Cmt || Is831Cmt || Is840Cmt || Is842Cmt || Is850Cmt || Is851Cmt || Is852Cmt || Is853Cmt || Is880Cmt;

        public bool IsNormalComment => !string.IsNullOrEmpty(ItemName) && string.IsNullOrEmpty(ItemCd);

        public bool IsDrugOrInjection
        {
            get => DrugKbn == 1 || DrugKbn == 4 || DrugKbn == 6;
        }

        public bool IsShugi => SinKouiKbn >= 80 && SinKouiKbn <= 89;

        public OrdInfValidationStatus Validation(int flag)
        {
            #region Validate common
            if (flag == 0)
            {
                if (RaiinNo <= 0)
                {
                    return OrdInfValidationStatus.InvalidRaiinNo;
                }
                if (PtId <= 0)
                {
                    return OrdInfValidationStatus.InvalidPtId;
                }
                if (SinDate <= 0)
                {
                    return OrdInfValidationStatus.InvalidSinDate;
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
            }
            #endregion

            return OrdInfValidationStatus.Valid;
        }

        public OrdInfDetailModel ChangeOrdInfDetail(string itemCd, string itemName, int sinKouiKbn, int kohatuKbn, int drugKbn, int unitSBT, string unitName, double termVal, double suryo, int yohoKbn, string ipnCd,
        string ipnName, string kokuji1, string kokuji2, int syohoKbn, int syohoLimitKbn)
        {
            ItemCd = itemCd;
            ItemName = itemName;
            SinKouiKbn = sinKouiKbn;
            KohatuKbn = kohatuKbn;
            DrugKbn = drugKbn;
            UnitSbt = unitSBT;
            UnitName = unitName;
            TermVal = termVal;
            Suryo = suryo;
            YohoKbn = yohoKbn;
            IpnCd = ipnCd;
            IpnName = ipnName;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            SyohoKbn = syohoKbn;
            SyohoLimitKbn = syohoLimitKbn;
            return this;
        }

        public OrdInfDetailModel ChangeSuryo(double suryo)
        {
            Suryo = suryo;
            return this;
        }

        public OrdInfDetailModel ChangeIsDummy(bool isDummy)
        {
            IsDummy = isDummy;
            return this;
        }
    }
}
