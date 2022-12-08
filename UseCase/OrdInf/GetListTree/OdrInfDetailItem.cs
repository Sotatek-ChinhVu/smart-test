using Domain.Models.OrdInfDetails;

namespace UseCase.OrdInfs.GetListTrees
{
    public class OdrInfDetailItem
    {
        public OdrInfDetailItem(int hpId, long raiinNo, long rpNo, long rpEdaNo, int rowNo, long ptId, int sinDate, int sinKouiKbn, string itemCd, string itemName, double suryo, string unitName, int unitSbt, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string kokuji1, string kokuji2, int isNodspRece, string ipnCd, string ipnName, int jissiKbn, DateTime jissiDate, int jissiId, string jissiMachine, string reqCd, string bunkatu, string cmtName, string cmtOpt, string fontColor, int commentNewline, double yakka, bool isGetPriceInYakka, double ten, int bunkatuKoui, int alternationIndex, int kensaGaichu, double odrTermVal, double cnvTermVal, string yjCd, string masterSbt, List<YohoSetMstModel> yohoSets, int kasan1, int kasan2, string cnvUnitName, string odrUnitName)
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
        }

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
        public double Yakka { get; private set; }
        public bool IsGetPriceInYakka { get; private set; }
        public double Ten { get; private set; }
        public int BunkatuKoui { get; private set; }
        public int AlternationIndex { get; private set; }
        public int KensaGaichu { get; private set; }
        public double OdrTermVal { get; private set; }
        public double CnvTermVal { get; private set; }
        public string YjCd { get; private set; }
        public string MasterSbt { get; private set; }
        public List<YohoSetMstModel> YohoSets { get; private set; }
        public int Kasan1 { get; private set; }
        public int Kasan2 { get; private set; }
        public string CnvUnitName { get; private set; }
        public string OdrUnitName { get; private set; }
    }
}
