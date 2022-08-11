using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public class KohiInfModel
    {
        public KohiInfModel(string futansyaNo, string jyukyusyaNo, int hokenId, int startDate, int endDate, int confirmDate, int rate, int gendoGaku, int sikakuDate, int kofuDate, string tokusyuNo, int hokenSbtKbn, string houbetu, int hokenNo, int hokenEdaNo, int prefNo)
        {
            FutansyaNo = futansyaNo;
            JyukyusyaNo = jyukyusyaNo;
            HokenId = hokenId;
            StartDate = startDate;
            EndDate = endDate;
            ConfirmDate = confirmDate;
            Rate = rate;
            GendoGaku = gendoGaku;
            SikakuDate = sikakuDate;
            KofuDate = kofuDate;
            TokusyuNo = tokusyuNo;
            HokenSbtKbn = hokenSbtKbn;
            Houbetu = houbetu;
            HokenNo = hokenNo;
            HokenEdaNo = hokenEdaNo;
            PrefNo = prefNo;
        }

        public string FutansyaNo { get; private set; }

        public string JyukyusyaNo { get; private set; }

        public int HokenId { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int ConfirmDate { get; private set; }

        public int Rate { get; private set; }

        public int GendoGaku { get; private set; }

        public int SikakuDate { get; private set; }

        public int KofuDate { get; private set; }

        public string TokusyuNo { get; private set; }

        public int HokenSbtKbn { get; private set; }

        public string Houbetu { get; private set; }

        public int HokenNo { get; private set; }

        public int HokenEdaNo { get; private set; }

        public int PrefNo { get; private set; }

    }
}
