using Domain.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.InputItem
{
    public class InputItemModel
    {
        public InputItemModel(int hpId, string itemCd, string rousaiKbnDisplay, string kanaName1, string name, string kohatuKbnDisplay, string kubunToDisplay, string kouseisinKbnDisplay, string tenDisplay, string odrUnitName, string kensaCenterItemCDDisplay, int endDate, int drugKbn, string masterSbt, int buiKbn)
        {
            HpId = hpId;
            ItemCd = itemCd;
            RousaiKbnDisplay = rousaiKbnDisplay;
            KanaName1 = kanaName1;
            Name = name;
            KohatuKbnDisplay = kohatuKbnDisplay;
            KubunToDisplay = kubunToDisplay;
            KouseisinKbnDisplay = kouseisinKbnDisplay;
            TenDisplay = tenDisplay;
            OdrUnitName = odrUnitName;
            KensaCenterItemCDDisplay = kensaCenterItemCDDisplay;
            EndDate = endDate;
            DrugKbn = drugKbn;
            MasterSbt = masterSbt;
            BuiKbn = buiKbn;
        }

        public int HpId { get; private set; }

        public string ItemCd { get; private set; }

        public string RousaiKbnDisplay { get; private set; }

        public string KanaName1 { get; private set; }

        public string Name { get; private set; }

        public string KohatuKbnDisplay { get; private set; }

        public string KubunToDisplay { get; private set; }

        public string KouseisinKbnDisplay { get; private set; }

        public string TenDisplay { get; private set; }

        public string OdrUnitName { get; private set; }

        public string KensaCenterItemCDDisplay { get; private set; }

        public int EndDate { get; private set; }

        public int DrugKbn { get; private set; }

        public string MasterSbt { get; private set; }

        public int BuiKbn { get; private set; }

        public string KouiName { get => BuildKouiName(ItemCd, DrugKbn, MasterSbt, BuiKbn); }

        private static string BuildKouiName(string itemCd, int drugKbn, string masterSbt, int buiKbn)
        {
            string rs = "";
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
