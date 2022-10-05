namespace Domain.Models.InsuranceMst
{
    public class HokenMstModel
    {
        public HokenMstModel(int hpId, int prefNo, int hokenNo, int hokenSbtKbn, int hokenKohiKbn, string houbetu, string hokenName, string hokenNameCd, int hokenEdaNo, int startDate, int endDate, int isOtherPrefValid, string hokenSname, string prefactureName, int receKisai, int futanKbn, int futanRate)
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
            ReceKisai = receKisai;
            FutanKbn = futanKbn;
            FutanRate = futanRate;
        }

        public HokenMstModel()
        {
            HpId = 0;
            PrefNo = 0;
            HokenNo = 0;
            HokenSbtKbn = 0;
            HokenKohiKbn = 0;
            Houbetu = string.Empty;
            HokenName = string.Empty;
            HokenNameCd = string.Empty;
            HokenEdaNo = 0;
            StartDate = 0;
            EndDate = 0;
            IsOtherPrefValid = 0;
            HokenSname = string.Empty;
            PrefactureName = string.Empty;
            ReceKisai = 0;
            FutanKbn = 0;
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

        public int ReceKisai { get; private set; }

        public int FutanKbn { get; private set; }

        public int FutanRate { get; private set; }

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
