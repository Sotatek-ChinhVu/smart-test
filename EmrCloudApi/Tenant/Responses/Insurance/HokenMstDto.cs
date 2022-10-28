using Domain.Models.Insurance;

namespace EmrCloudApi.Tenant.Responses.Insurance
{
    public class HokenMstDto
    {
        public HokenMstDto(HokenMstModel hokenMstModel)
        {
            FutanKbn = hokenMstModel.FutanKbn;
            FutanRate = hokenMstModel.FutanRate;
            StartDate = hokenMstModel.StartDate;
            EndDate = hokenMstModel.EndDate;
            HokenNo = hokenMstModel.HokenNo;
            HokenEdaNo = hokenMstModel.HokenEdaNo;
            HokenSName = hokenMstModel.HokenSName;
            Houbetu = hokenMstModel.Houbetu;
            HokenSbtKbn = hokenMstModel.HokenSbtKbn;
            CheckDigit = hokenMstModel.CheckDigit;
            AgeStart = hokenMstModel.AgeStart;
            AgeEnd = hokenMstModel.AgeEnd;
            JyuKyuCheckDigit = hokenMstModel.JyuKyuCheckDigit;
            FutansyaCheckFlag = hokenMstModel.FutansyaCheckFlag;
            JyukyusyaCheckFlag = hokenMstModel.JyukyusyaCheckFlag;
            TokusyuCheckFlag = hokenMstModel.TokusyuCheckFlag;
        }

        public int FutanKbn { get; private set; }

        public int FutanRate { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public string HokenSName { get; private set; }

        public string Houbetu { get; private set; }

        public int HokenSbtKbn { get; private set; }

        public int CheckDigit { get; private set; }

        public int AgeStart { get; private set; }

        public int AgeEnd { get; private set; }

        public int FutansyaCheckFlag { get; private set; }

        public int JyukyusyaCheckFlag { get; private set; }

        public int JyuKyuCheckDigit { get; private set; }

        public int TokusyuCheckFlag { get; private set; }

        public string SelectedValueMaster
        {
            get
            {
                string result = string.Empty;
                if (HokenEdaNo == 0)
                {
                    result = HokenNo.ToString().PadLeft(3, '0');
                }
                else
                {
                    result = HokenNo.ToString().PadLeft(3, '0') + HokenEdaNo;
                }

                return result;
            }
        }

        public string DisplayTextMaster
        {
            get
            {
                string DisplayText = SelectedValueMaster + " " + HokenSName;
                return DisplayText;
            }
        }
    }
}
