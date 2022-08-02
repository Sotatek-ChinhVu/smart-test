using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.IsuranceMst
{
    public class HokenMstModel
    {
        public HokenMstModel(int hpId, int prefNo, int hokenNo, int hokenSbtKbn, int hokenKohiKbn, string houbetu, string hokenName, string hokenNameCd, int hokenEdaNo, int startDate, int endDate, int isOtherPrefValid, string hokenSname, string prefactureName)
        {
            HpId = hpId;
            PrefNo = prefNo;
            HokenNo = hokenNo;
            HokenSbtKbn = hokenSbtKbn;
            HokenKohiKbn = hokenKohiKbn;
            Houbetu = houbetu;
            HokenName = hokenName;
            HokenNameCd = hokenNameCd;
            HokenEdaNo = hokenEdaNo;
            StartDate = startDate;
            EndDate = endDate;
            IsOtherPrefValid = isOtherPrefValid;
            HokenSname = hokenSname;
            PrefactureName = prefactureName;
        }

        public int HpId { get; private set; }

        public int PrefNo { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenSbtKbn { get; private set; }

        public int HokenKohiKbn { get; private set; }

        public string Houbetu { get; private set; }

        public string HokenName { get; private set; }

        public string HokenNameCd { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int IsOtherPrefValid { get; private set; }

        public string HokenSname { get; private set; }

        public string PrefactureName { get; private set; }

        public string DisplayTextMaster { get => SetTextMaster(); }

        private string SetTextMaster()
        {
            string selectedValueMaster = string.Empty;
            if (HokenEdaNo == 0)
            {
                selectedValueMaster = HokenNo.ToString().PadLeft(3, '0');
            }
            else
            {
                selectedValueMaster = HokenNo.ToString().PadLeft(3, '0') + HokenEdaNo;
            }

            string DisplayText = selectedValueMaster + " " + HokenSname;
            if (IsOtherPrefValid == 1 && !string.IsNullOrEmpty(PrefactureName))
            {

                DisplayText += "（" + PrefactureName + "）";
            }

            return DisplayText;
        }

    }
}
