using Domain.Models.OrdInfDetails;
using System.Text.Json.Serialization;

namespace UseCase.OrdInfs.GetListTrees
{
    public class OdrInfDetailItem
    {
        public OdrInfDetailItem(int hpId, long raiinNo, long rpNo, long rpEdaNo, int rowNo, long ptId, int sinDate, int sinKouiKbn, string itemCd, string itemName, double suryo, string unitName, int unitSbt, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string kokuji1, string kokuji2, int isNodspRece, string ipnCd, string ipnName, int jissiKbn, DateTime jissiDate, int jissiId, string jissiMachine, string reqCd, string bunkatu, string cmtName, string cmtOpt, string fontColor, int commentNewline, double yakka, bool isGetPriceInYakka, double ten, int bunkatuKoui, int alternationIndex, int kensaGaichu, double odrTermVal, double cnvTermVal, string yjCd, string masterSbt, List<YohoSetMstModel> yohoSets, int kasan1, int kasan2, string cnvUnitName, string odrUnitName, bool hasCmtName, string centerItemCd1, string centerItemCd2)
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
            Yakka = yakka;
            IsGetPriceInYakka = isGetPriceInYakka;
            Ten = ten;
            BunkatuKoui = bunkatuKoui;
            AlternationIndex = alternationIndex;
            KensaGaichu = kensaGaichu;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            YjCd = yjCd;
            MasterSbt = masterSbt;
            YohoSets = yohoSets;
            Kasan1 = kasan1;
            Kasan2 = kasan2;
            CnvUnitName = cnvUnitName;
            OdrUnitName = odrUnitName;
            HasCmtName = hasCmtName;
            CenterItemCd1 = centerItemCd1;
            CenterItemCd2 = centerItemCd2;
        }

        public OdrInfDetailItem(int hpId, long raiinNo, long rpNo, long rpEdaNo, int rowNo, long ptId, int sinDate, int sinKouiKbn, string itemCd, string itemName, double suryo, string unitName, int unitSbt, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string kokuji1, string kokuji2, int isNodspRece, string ipnCd, string ipnName, int jissiKbn, DateTime jissiDate, int jissiId, string jissiMachine, string reqCd, string bunkatu, string cmtName, string cmtOpt, string fontColor, int commentNewline, double yakka, bool isGetPriceInYakka, double ten, int bunkatuKoui, int alternationIndex, int kensaGaichu, double odrTermVal, double cnvTermVal, string yjCd, string masterSbt, List<YohoSetMstModel> yohoSets, int kasan1, int kasan2, string cnvUnitName, string odrUnitName, bool hasCmtName, string centerItemCd1, string centerItemCd2, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, int cmtCol1, int cmtCol2, int cmtCol3, int cmtCol4)
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
            Yakka = yakka;
            IsGetPriceInYakka = isGetPriceInYakka;
            Ten = ten;
            BunkatuKoui = bunkatuKoui;
            AlternationIndex = alternationIndex;
            KensaGaichu = kensaGaichu;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            YjCd = yjCd;
            MasterSbt = masterSbt;
            YohoSets = yohoSets;
            Kasan1 = kasan1;
            Kasan2 = kasan2;
            CnvUnitName = cnvUnitName;
            OdrUnitName = odrUnitName;
            HasCmtName = hasCmtName;
            CenterItemCd1 = centerItemCd1;
            CenterItemCd2 = centerItemCd2;
            CmtCol1 = cmtCol1;
            CmtCol2 = cmtCol2;
            CmtCol3 = cmtCol3;
            CmtCol4 = cmtCol4;
            CmtColKeta1 = cmtColKeta1;
            CmtColKeta2 = cmtColKeta2;
            CmtColKeta3 = cmtColKeta3;
            CmtColKeta4 = cmtColKeta4;
        }

        [JsonPropertyName("hpId")]
        public int HpId { get; private set; }

        [JsonPropertyName("raiinNo")]
        public long RaiinNo { get; private set; }

        [JsonPropertyName("rpNo")]
        public long RpNo { get; private set; }

        [JsonPropertyName("rpEdaNo")]
        public long RpEdaNo { get; private set; }

        [JsonPropertyName("rowNo")]
        public int RowNo { get; private set; }

        [JsonPropertyName("ptId")]
        public long PtId { get; private set; }

        [JsonPropertyName("sinDate")]
        public int SinDate { get; private set; }

        [JsonPropertyName("sinKouiKbn")]
        public int SinKouiKbn { get; private set; }

        [JsonPropertyName("itemCd")]
        public string ItemCd { get; private set; }

        [JsonPropertyName("itemName")]
        public string ItemName { get; private set; }

        [JsonPropertyName("suryo")]
        public double Suryo { get; private set; }

        [JsonPropertyName("unitName")]
        public string UnitName { get; private set; }

        [JsonPropertyName("unitSbt")]
        public int UnitSbt { get; private set; }

        [JsonPropertyName("termVal")]
        public double TermVal { get; private set; }

        [JsonPropertyName("kohatuKbn")]
        public int KohatuKbn { get; private set; }

        [JsonPropertyName("syohoKbn")]
        public int SyohoKbn { get; private set; }

        [JsonPropertyName("syohoLimitKbn")]
        public int SyohoLimitKbn { get; private set; }

        [JsonPropertyName("drugKbn")]
        public int DrugKbn { get; private set; }

        [JsonPropertyName("yohoKbn")]
        public int YohoKbn { get; private set; }

        [JsonPropertyName("kokuji1")]
        public string Kokuji1 { get; private set; }

        [JsonPropertyName("kokuji2")]
        public string Kokuji2 { get; private set; }

        [JsonPropertyName("isNodspRece")]
        public int IsNodspRece { get; private set; }

        [JsonPropertyName("ipnCd")]
        public string IpnCd { get; private set; }

        [JsonPropertyName("ipnName")]
        public string IpnName { get; private set; }

        [JsonPropertyName("jissiKbn")]
        public int JissiKbn { get; private set; }

        [JsonPropertyName("jissiDate")]
        public DateTime JissiDate { get; private set; }

        [JsonPropertyName("jissiId")]
        public int JissiId { get; private set; }

        [JsonPropertyName("jissiMachine")]
        public string JissiMachine { get; private set; }

        [JsonPropertyName("reqCd")]
        public string ReqCd { get; private set; }

        [JsonPropertyName("bunkatu")]
        public string Bunkatu { get; private set; }

        [JsonPropertyName("cmtName")]
        public string CmtName { get; private set; }

        [JsonPropertyName("cmtOpt")]
        public string CmtOpt { get; private set; }

        [JsonPropertyName("fontColor")]
        public string FontColor { get; private set; }

        [JsonPropertyName("commentNewline")]
        public int CommentNewline { get; private set; }

        [JsonPropertyName("yakka")]
        public double Yakka { get; private set; }

        [JsonPropertyName("isGetPriceInYakka")]
        public bool IsGetPriceInYakka { get; private set; }

        [JsonPropertyName("ten")]
        public double Ten { get; private set; }

        [JsonPropertyName("bunkatuKoui")]
        public int BunkatuKoui { get; private set; }

        [JsonPropertyName("alternationIndex")]
        public int AlternationIndex { get; private set; }

        [JsonPropertyName("kensaGaichu")]
        public int KensaGaichu { get; private set; }

        [JsonPropertyName("odrTermVal")]
        public double OdrTermVal { get; private set; }

        [JsonPropertyName("cnvTermVal")]
        public double CnvTermVal { get; private set; }

        [JsonPropertyName("yjCd")]
        public string YjCd { get; private set; }

        [JsonPropertyName("masterSbt")]
        public string MasterSbt { get; private set; }

        [JsonPropertyName("yohoSets")]
        public List<YohoSetMstModel> YohoSets { get; private set; }

        [JsonPropertyName("kasan1")]
        public int Kasan1 { get; private set; }

        [JsonPropertyName("kasan2")]
        public int Kasan2 { get; private set; }

        [JsonPropertyName("cnvUnitName")]
        public string CnvUnitName { get; private set; }

        [JsonPropertyName("odrUnitName")]
        public string OdrUnitName { get; private set; }

        [JsonPropertyName("hasCmtName")]
        public bool HasCmtName { get; private set; }

        [JsonPropertyName("centerItemCd1")]
        public string CenterItemCd1 { get; private set; }

        [JsonPropertyName("centerItemCd2")]
        public string CenterItemCd2 { get; private set; }

        [JsonPropertyName("cmtColKeta1")]
        public int CmtColKeta1 { get; private set; }

        [JsonPropertyName("cmtColKeta2")]
        public int CmtColKeta2 { get; private set; }

        [JsonPropertyName("cmtColKeta3")]
        public int CmtColKeta3 { get; private set; }

        [JsonPropertyName("cmtColKeta4")]
        public int CmtColKeta4 { get; private set; }


        [JsonPropertyName("cmtCol1")]
        public int CmtCol1 { get; private set; }

        [JsonPropertyName("cmtCol2")]
        public int CmtCol2 { get; private set; }

        [JsonPropertyName("cmtCol3")]
        public int CmtCol3 { get; private set; }

        [JsonPropertyName("cmtCol4")]
        public int CmtCol4 { get; private set; }
    }
}
