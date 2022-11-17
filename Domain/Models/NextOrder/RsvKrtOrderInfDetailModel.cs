using Domain.Models.OrdInfDetails;

namespace Domain.Models.NextOrder
{
    public class RsvKrtOrderInfDetailModel
    {
        public RsvKrtOrderInfDetailModel(int hpId, long ptId, long rsvkrtNo, long rpNo, long rpEdaNo, int rowNo, int rsvDate, int sinKouiKbn, string itemCd, string itemName, double suryo, string unitName, int unitSbt, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string kokuji1, string kokuji2, int isNodspRece, string ipnCd, string ipnName, string bunkatu, string cmtName, string cmtOpt, string fontColor, int commentNewline, string masterSbt, int inOutKbn, double yakka, bool isGetPriceInYakka, double ten, int bunkatuKoui, int alternationIndex, int kensaGaichu, int refillSetting, int cmtCol1, double odrTermVal, double cnvTermVal, string yjCd, List<YohoSetMstModel> yohoSets, int kasan1, int kasan2)
        {
            HpId = hpId;
            PtId = ptId;
            RsvkrtNo = rsvkrtNo;
            RpNo = rpNo;
            RpEdaNo = rpEdaNo;
            RowNo = rowNo;
            RsvDate = rsvDate;
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
            RefillSetting = refillSetting;
            CmtCol1 = cmtCol1;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            YjCd = yjCd;
            YohoSets = yohoSets;
            Kasan1 = kasan1;
            Kasan2 = kasan2;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public long RsvkrtNo { get; private set; }

        public long RpNo { get; private set; }

        public long RpEdaNo { get; private set; }

        public int RowNo { get; private set; }

        public int RsvDate { get; private set; }

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
    }
}
