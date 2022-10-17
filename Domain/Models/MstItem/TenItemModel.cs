using Helper.Constants;

namespace Domain.Models.MstItem
{
    public class TenItemModel
    {
        public TenItemModel(int hpId, string itemCd, int rousaiKbn, string kanaName1, string name, int kohatuKbn, int madokuKbn, int kouseisinKbn, string odrUnitName, int endDate, int drugKbn, string masterSbt, int buiKbn, int isAdopted, double ten, int tenId, string kensaMstCenterItemCd1, string kensaMstCenterItemCd2, int cmtCol1, string ipnNameCd, int sinKouiKbn, string yjCd, string cnvUnitName, int startDate, int yohoKbn)
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
            YjCd = String.Empty;
            CnvUnitName = String.Empty;
            YohoKbn = 0;
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
