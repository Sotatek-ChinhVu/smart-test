﻿using Domain.Models.OrdInfDetails;

namespace UseCase.SuperSetDetail.GetSuperSetDetailToDoTodayOrder
{
    public class SetOrderInfDetailItem
    {
        public SetOrderInfDetailItem(int hpId, int setCd, long rpNo, long rpEdaNo, int rowNo, int sinKouiKbn, string itemCd, string itemName, string displayItemName, double suryo, string unitName, int unitSBT, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string kokuji1, string kokuji2, int isNodspRece, string ipnCd, string ipnName, string bunkatu, string cmtName, string cmtOpt, string fontColor, int commentNewline, string masterSbt, int inOutKbn, double yakka, bool isGetPriceInYakka, double ten, int bunkatuKoui, int kensaGaichu, double odrTermVal, double cnvTermVal, string yjCd, string centerItemCd1, string centerItemCd2, int kasan1, int kasan2, List<YohoSetMstModel> yohoSets, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, int cmtCol1, int cmtCol2, int cmtCol3, int cmtCol4)
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
            KensaGaichu = kensaGaichu;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            YjCd = yjCd;
            CenterItemCd1 = centerItemCd1;
            CenterItemCd2 = centerItemCd2;
            Kasan1 = kasan1;
            Kasan2 = kasan2;
            YohoSets = yohoSets;
            CmtCol1 = cmtCol1;
            CmtCol2 = cmtCol2;
            CmtCol3 = cmtCol3;
            CmtCol4 = cmtCol4;
            CmtColKeta1 = cmtColKeta1;
            CmtColKeta2 = cmtColKeta2;
            CmtColKeta3 = cmtColKeta3;
            CmtColKeta4 = cmtColKeta4;
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

        public string MasterSbt { get; private set; }

        public int InOutKbn { get; private set; }

        public double Yakka { get; private set; }

        public bool IsGetPriceInYakka { get; private set; }

        public double Ten { get; private set; }

        public int BunkatuKoui { get; private set; }

        public int KensaGaichu { get; private set; }

        public double OdrTermVal { get; private set; }

        public double CnvTermVal { get; private set; }

        public string YjCd { get; private set; }

        public string CenterItemCd1 { get; private set; } = string.Empty;

        public string CenterItemCd2 { get; private set; } = string.Empty;

        public int Kasan1 { get; private set; }

        public int Kasan2 { get; private set; }

        public List<YohoSetMstModel> YohoSets { get; private set; }

        public int CmtColKeta1 { get; private set; }

        public int CmtColKeta2 { get; private set; }

        public int CmtColKeta3 { get; private set; }

        public int CmtColKeta4 { get; private set; }

        public int CmtCol1 { get; private set; }

        public int CmtCol2 { get; private set; }

        public int CmtCol3 { get; private set; }

        public int CmtCol4 { get; private set; }
    }
}
