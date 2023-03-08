using Helper.Constants;
using System.Text.Json.Serialization;

namespace Domain.Models.MstItem
{
    public class TenItemModel
    {
        [JsonConstructor]
        public TenItemModel(int hpId, string itemCd, int rousaiKbn, string kanaName1, string name, int kohatuKbn, int madokuKbn, int kouseisinKbn, string odrUnitName, int endDate, int drugKbn, string masterSbt, int buiKbn, int isAdopted, double ten, int tenId, string kensaMstCenterItemCd1, string kensaMstCenterItemCd2, int cmtCol1, string ipnNameCd, int sinKouiKbn, string yjCd, string cnvUnitName, int startDate, int yohoKbn, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, int cmtCol2, int cmtCol3, int cmtCol4, string ipnCd, string minAge, string maxAge, string santeiItemCd, double odrTermVal, double cnvTermVal, double defaultValue, string kokuji1, string kokuji2)
        {
            HpId = hpId;
            ItemCd = itemCd;
            RousaiKbn = rousaiKbn;
            KanaName1 = kanaName1;
            Name = name;
            KohatuKbn = kohatuKbn;
            MadokuKbn = madokuKbn;
            KouseisinKbn = kouseisinKbn;
            OdrUnitName = odrUnitName;
            EndDate = endDate;
            DrugKbn = drugKbn;
            MasterSbt = masterSbt;
            BuiKbn = buiKbn;
            Ten = ten;
            TenId = tenId;
            KensaMstCenterItemCd1 = kensaMstCenterItemCd1;
            KensaMstCenterItemCd2 = kensaMstCenterItemCd2;
            IsAdopted = isAdopted;
            CmtCol1 = cmtCol1;
            IpnNameCd = ipnNameCd;
            SinKouiKbn = sinKouiKbn;
            YjCd = yjCd;
            CnvUnitName = cnvUnitName;
            StartDate = startDate;
            YohoKbn = yohoKbn;
            CmtColKeta1 = cmtColKeta1;
            CmtColKeta2 = cmtColKeta2;
            CmtColKeta3 = cmtColKeta3;
            CmtColKeta4 = cmtColKeta4;
            CmtCol2 = cmtCol2;
            CmtCol3 = cmtCol3;
            CmtCol4 = cmtCol4;
            IpnCD = ipnCd;
            MinAge = minAge;
            MaxAge = maxAge;
            SanteiItemCd = santeiItemCd;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            DefaultValue = defaultValue;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
        }

        public TenItemModel(int hpId, string itemCd, int rousaiKbn, string kanaName1, string name, int kohatuKbn, int madokuKbn, int kouseisinKbn, string odrUnitName, int endDate, int drugKbn, string masterSbt, int buiKbn, int isAdopted, double ten, int tenId, string kensaMstCenterItemCd1, string kensaMstCenterItemCd2, int cmtCol1, string ipnNameCd, int sinKouiKbn, string yjCd, string cnvUnitName, int startDate, int yohoKbn, int cmtColKeta1, int cmtColKeta2, int cmtColKeta3, int cmtColKeta4, int cmtCol2, int cmtCol3, int cmtCol4, string ipnCd, string minAge, string maxAge, string santeiItemCd, double odrTermVal, double cnvTermVal, double defaultValue, string kokuji1, string kokuji2, int modeStatus)
        {
            HpId = hpId;
            ItemCd = itemCd;
            RousaiKbn = rousaiKbn;
            KanaName1 = kanaName1;
            Name = name;
            KohatuKbn = kohatuKbn;
            MadokuKbn = madokuKbn;
            KouseisinKbn = kouseisinKbn;
            OdrUnitName = odrUnitName;
            EndDate = endDate;
            DrugKbn = drugKbn;
            MasterSbt = masterSbt;
            BuiKbn = buiKbn;
            Ten = ten;
            TenId = tenId;
            KensaMstCenterItemCd1 = kensaMstCenterItemCd1;
            KensaMstCenterItemCd2 = kensaMstCenterItemCd2;
            IsAdopted = isAdopted;
            CmtCol1 = cmtCol1;
            IpnNameCd = ipnNameCd;
            SinKouiKbn = sinKouiKbn;
            YjCd = yjCd;
            CnvUnitName = cnvUnitName;
            StartDate = startDate;
            YohoKbn = yohoKbn;
            CmtColKeta1 = cmtColKeta1;
            CmtColKeta2 = cmtColKeta2;
            CmtColKeta3 = cmtColKeta3;
            CmtColKeta4 = cmtColKeta4;
            CmtCol2 = cmtCol2;
            CmtCol3 = cmtCol3;
            CmtCol4 = cmtCol4;
            IpnCD = ipnCd;
            MinAge = minAge;
            MaxAge = maxAge;
            SanteiItemCd = santeiItemCd;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            DefaultValue = defaultValue;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            ModeStatus = modeStatus;
        }

        public TenItemModel()
        {
            HpId = 0;
            ItemCd = string.Empty;
            RousaiKbn = 0;
            KanaName1 = string.Empty;
            Name = string.Empty;
            KohatuKbn = 0;
            MadokuKbn = 0;
            KouseisinKbn = 0;
            OdrUnitName = string.Empty;
            EndDate = 0;
            DrugKbn = 0;
            MasterSbt = string.Empty;
            BuiKbn = 0;
            Ten = 0;
            TenId = 0;
            KensaMstCenterItemCd1 = string.Empty;
            KensaMstCenterItemCd2 = string.Empty;
            IsAdopted = 0;
            CmtCol1 = 0;
            IpnNameCd = string.Empty;
            YjCd = string.Empty;
            CnvUnitName = string.Empty;
            YohoKbn = 0;
            IpnCD = string.Empty;
            MinAge = string.Empty;
            MaxAge = string.Empty;
            SanteiItemCd = string.Empty;
            CnvTermVal = 0;
            OdrTermVal = 0;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
        }

        public TenItemModel(int hpId, string itemCd, string minAge, string maxAge, string santeiItemCd, int startDate, int endDate)
        {
            HpId = hpId;
            ItemCd = itemCd;
            MinAge = minAge;
            MaxAge = maxAge;
            SanteiItemCd = santeiItemCd;
            IpnCD = string.Empty;
            YjCd = string.Empty;
            CnvUnitName = string.Empty;
            IpnNameCd = string.Empty;
            KensaMstCenterItemCd2 = string.Empty;
            KensaMstCenterItemCd1 = string.Empty;
            MasterSbt = string.Empty;
            OdrUnitName = string.Empty;
            Name = string.Empty;
            KanaName1 = string.Empty;
            StartDate = startDate;
            EndDate = endDate;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
        }

        public int HpId { get; private set; }

        public string ItemCd { get; private set; }

        public int RousaiKbn { get; private set; }

        public string KanaName1 { get; private set; }

        public string Name { get; private set; }

        public int KohatuKbn { get; private set; }

        public int MadokuKbn { get; private set; }

        public int KouseisinKbn { get; private set; }

        public string OdrUnitName { get; private set; }

        public int EndDate { get; private set; }

        public int DrugKbn { get; private set; }

        public string MasterSbt { get; private set; }

        public int BuiKbn { get; private set; }

        public int IsAdopted { get; private set; }

        public double Ten { get; private set; }

        public int TenId { get; private set; }

        public string KensaMstCenterItemCd1 { get; private set; }

        public string KensaMstCenterItemCd2 { get; private set; }

        public int CmtCol1 { get; private set; }

        public string IpnNameCd { get; private set; }

        public int SinKouiKbn { get; private set; }

        public string YjCd { get; private set; }

        public string CnvUnitName { get; private set; }

        public int StartDate { get; private set; }

        public int YohoKbn { get; private set; }

        public int CmtColKeta1 { get; private set; }

        public int CmtColKeta2 { get; private set; }

        public int CmtColKeta3 { get; private set; }

        public int CmtColKeta4 { get; private set; }

        public int CmtCol2 { get; private set; }

        public int CmtCol3 { get; private set; }

        public int CmtCol4 { get; private set; }

        public string IpnCD { get; private set; }

        public string MaxAge { get; private set; }

        public string MinAge { get; private set; }

        public string SanteiItemCd { get; private set; }

        public double OdrTermVal { get; private set; }

        public double CnvTermVal { get; private set; }

        public double DefaultValue { get; private set; }

        public string Kokuji1 { get; private set; }

        public string Kokuji2 { get; private set; }

        public int HandanGrpKbn { get; private set; }

        public int ModeStatus { get; private set; }

        public string RousaiKbnDisplay
        {
            get
            {
                if (RousaiKbn == 1) return "〇";
                return "";
            }
        }

        public string KouseisinKbnDisplay
        {
            get
            {
                return KouseisinKbn switch
                {
                    1 => "抗不",
                    2 => "睡眠",
                    3 => "うつ",
                    4 => "抗精",
                    _ => "",
                };
            }
        }

        public string KubunToDisplay
        {
            get
            {
                return MadokuKbn switch
                {
                    1 => "麻",
                    2 => "毒",
                    3 => "覚",
                    5 => "向",
                    _ => "",
                };
            }
        }

        public string KohatuKbnDisplay
        {
            get
            {
                if (KohatuKbn == 1)
                {
                    return "後";
                }
                else if (KohatuKbn == 2)
                {
                    return "〇";
                }
                return string.Empty;
            }
        }

        public string KensaCenterItemCDDisplay
        {
            get
            {
                if (!String.IsNullOrEmpty(KensaMstCenterItemCd1) && !String.IsNullOrEmpty(KensaMstCenterItemCd2))
                {
                    return string.Format("{0}/{1}", KensaMstCenterItemCd1, KensaMstCenterItemCd1);
                }
                return string.Empty;
            }
        }

        public string TenDisplay
        {
            get
            {
                if (Ten == 0) return Ten.ToString();

                if (new[] { 1, 2, 4 }.Contains(TenId))
                {
                    return "￥" + Ten.ToString();
                }
                if (new[] { 5, 6 }.Contains(TenId))
                {
                    return Ten.ToString() + "%";
                }

                return Ten.ToString();
            }
        }

        public string KouiName { get => BuildKouiName(ItemCd, DrugKbn, MasterSbt, BuiKbn, SinKouiKbn); }

        private static string BuildKouiName(string itemCd, int drugKbn, string masterSbt, int buiKbn, int sinKouiKbn)
        {
            string rs = "";
            var sinKouiCollection = new SinkouiCollection();
            var itemKoui = sinKouiCollection.FirstOrDefault(x => x.SinKouiCd == sinKouiKbn);
            if (itemKoui != null)
            {
                rs = itemKoui.SinkouiName;
            }
            if (itemCd == ItemCdConst.GazoDensibaitaiHozon)
            {
                rs = "フィルム";
            }
            if (drugKbn > 0)
            {
                switch (drugKbn)
                {
                    case 1:
                        rs = "内用";
                        break;
                    case 3:
                        rs = "薬剤他";
                        break;
                    case 4:
                        rs = "注射";
                        break;
                    case 6:
                        rs = "外用";
                        break;
                    case 8:
                        rs = "歯科薬";
                        break;
                }

            }
            else if (masterSbt == "T")
            {
                rs = "特材";
            }
            if (buiKbn > 0)
            {
                rs = "部位";
            }
            return rs;
        }

    }
}
