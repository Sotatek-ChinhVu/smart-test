using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public class HokenMstModel
    {
        public HokenMstModel(int futanKbn, int futanRate, int startDate, int endDate, int hokenNo, int hokenEdaNo, string hokenShortName, string houbetu)
        {
            FutanKbn = futanKbn;
            FutanRate = futanRate;
            StartDate = startDate;
            EndDate = endDate;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            HokenShortName = hokenShortName;
            Houbetu = houbetu;
        }

        public HokenMstModel()
        {
            HokenShortName = string.Empty;
            Houbetu = string.Empty;
        }

        public int FutanKbn { get; private set; }

        public int FutanRate { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public string HokenShortName { get; private set; }

        public string Houbetu { get; private set; }

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
                string DisplayText = SelectedValueMaster + " " + HokenShortName;
                return DisplayText;
            }
        }
    }
}
