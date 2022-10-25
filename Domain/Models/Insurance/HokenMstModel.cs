using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public class HokenMstModel
    {
        [JsonConstructor]
        public HokenMstModel(int futanKbn, int futanRate, int startDate, int endDate, int hokenNo, int hokenEdaNo, string hokenSName, string houbetu, int hokenSbtKbn, int checkDigit, int ageStart, int ageEnd, int futansyaCheckFlag, int jyukyusyaCheckFlag, int jyuKyuCheckDigit, int tokusyuCheckFlag)
        {
            FutanKbn = futanKbn;
            FutanRate = futanRate;
            StartDate = startDate;
            EndDate = endDate;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            HokenSName = hokenSName;
            Houbetu = houbetu;
            HokenSbtKbn = hokenSbtKbn;
            CheckDigit = checkDigit;
            AgeStart = ageStart;
            AgeEnd = ageEnd;
            FutansyaCheckFlag = futansyaCheckFlag;
            JyukyusyaCheckFlag = jyukyusyaCheckFlag;
            JyuKyuCheckDigit = jyuKyuCheckDigit;
            TokusyuCheckFlag = tokusyuCheckFlag;
        }

        public HokenMstModel()
        {
            HokenSName = string.Empty;
            Houbetu = string.Empty;
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
