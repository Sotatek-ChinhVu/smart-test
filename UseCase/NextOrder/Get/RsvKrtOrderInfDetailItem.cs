using Domain.Models.NextOrder;
using Domain.Models.OrdInfDetails;

namespace UseCase.NextOrder.Get
{
    public class RsvKrtOrderInfDetailItem
    {
        public RsvKrtOrderInfDetailItem(RsvKrtOrderInfDetailModel rsvKrtOrderInfDetailModel)
        {
            HpId = rsvKrtOrderInfDetailModel.HpId;
            PtId = rsvKrtOrderInfDetailModel.PtId;
            RsvkrtNo = rsvKrtOrderInfDetailModel.RsvkrtNo;
            RpNo = rsvKrtOrderInfDetailModel.RpNo;
            RpEdaNo = rsvKrtOrderInfDetailModel.RpEdaNo;
            RowNo = rsvKrtOrderInfDetailModel.RowNo;
            RsvDate = rsvKrtOrderInfDetailModel.RsvDate;
            SinKouiKbn = rsvKrtOrderInfDetailModel.SinKouiKbn;
            ItemCd = rsvKrtOrderInfDetailModel.ItemCd;
            ItemName = rsvKrtOrderInfDetailModel.ItemName;
            Suryo = rsvKrtOrderInfDetailModel.Suryo;
            UnitName = rsvKrtOrderInfDetailModel.UnitName;
            UnitSbt = rsvKrtOrderInfDetailModel.UnitSbt;
            TermVal = rsvKrtOrderInfDetailModel.TermVal;
            KohatuKbn = rsvKrtOrderInfDetailModel.KohatuKbn;
            SyohoKbn = rsvKrtOrderInfDetailModel.SyohoKbn;
            SyohoLimitKbn = rsvKrtOrderInfDetailModel.SyohoLimitKbn;
            DrugKbn = rsvKrtOrderInfDetailModel.DrugKbn;
            YohoKbn = rsvKrtOrderInfDetailModel.YohoKbn;
            Kokuji1 = rsvKrtOrderInfDetailModel.Kokuji1;
            Kokuji2 = rsvKrtOrderInfDetailModel.Kokuji2;
            IsNodspRece = rsvKrtOrderInfDetailModel.IsNodspRece;
            IpnCd = rsvKrtOrderInfDetailModel.IpnCd;
            IpnName = rsvKrtOrderInfDetailModel.IpnName;
            Bunkatu = rsvKrtOrderInfDetailModel.Bunkatu;
            CmtName = rsvKrtOrderInfDetailModel.CmtName;
            CmtOpt = rsvKrtOrderInfDetailModel.CmtOpt;
            FontColor = rsvKrtOrderInfDetailModel.FontColor;
            CommentNewline = rsvKrtOrderInfDetailModel.CommentNewline;
            MasterSbt = rsvKrtOrderInfDetailModel.MasterSbt;
            InOutKbn = rsvKrtOrderInfDetailModel.InOutKbn;
            Yakka = rsvKrtOrderInfDetailModel.Yakka;
            IsGetPriceInYakka = rsvKrtOrderInfDetailModel.IsGetPriceInYakka;
            Ten = rsvKrtOrderInfDetailModel.Ten;
            BunkatuKoui = rsvKrtOrderInfDetailModel.BunkatuKoui;
            AlternationIndex = rsvKrtOrderInfDetailModel.AlternationIndex;
            KensaGaichu = rsvKrtOrderInfDetailModel.KensaGaichu;
            RefillSetting = rsvKrtOrderInfDetailModel.RefillSetting;
            CmtCol1 = rsvKrtOrderInfDetailModel.CmtCol1;
            OdrTermVal = rsvKrtOrderInfDetailModel.OdrTermVal;
            CnvTermVal = rsvKrtOrderInfDetailModel.CnvTermVal;
            YjCd = rsvKrtOrderInfDetailModel.YjCd;
            YohoSets = rsvKrtOrderInfDetailModel.YohoSets;
            Kasan1 = rsvKrtOrderInfDetailModel.Kasan1;
            Kasan2 = rsvKrtOrderInfDetailModel.Kasan2;
            CenterItemCd1 = rsvKrtOrderInfDetailModel.CenterItemCd1;
            CenterItemCd2 = rsvKrtOrderInfDetailModel.CenterItemCd2;
            HandanGrpKbn = rsvKrtOrderInfDetailModel.HandanGrpKbn;
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

        public string CenterItemCd1 { get; private set; }

        public string CenterItemCd2 { get; private set; }

        public int HandanGrpKbn { get; private set; }
    }
}
