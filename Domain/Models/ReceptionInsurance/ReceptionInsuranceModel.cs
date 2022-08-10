using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ReceptionInsurance
{
    public class ReceptionInsuranceModel
    {
        public ReceptionInsuranceModel(int hokenKbn, string kigo, string bango, int startDate, int endDate, int confirmDate, string edaNo, string hokensyaNo, string rousaiKofuNo, int sinday, int isHokenInf, int isKohi, string futansyaNo, string jyukyusyaNo)
        {
            HokenKbn = hokenKbn;
            Kigo = kigo;
            Bango = bango;
            StartDate = startDate;
            EndDate = endDate;
            ConfirmDate = confirmDate;
            EdaNo = edaNo;
            HokensyaNo = hokensyaNo;
            RousaiKofuNo = rousaiKofuNo;
            Sinday = sinday;
            IsHokenInf = isHokenInf;
            IsKohi = isKohi;
            FutansyaNo = futansyaNo;
            JyukyusyaNo = jyukyusyaNo;
        }

        public int HokenKbn { get; private set; }

        public string Kigo { get; private set; }

        public string Bango { get; private set; }

        public int StartDate { get; private set; }

        public int EndDate { get; private set; }

        public int ConfirmDate { get; private set; }

        public string EdaNo{ get; private set; }

        public string HokensyaNo { get; private set; }

        public string RousaiKofuNo { get; private set; }

        public int Sinday { get; private set; }

        public int IsHokenInf { get; private set; }

        public int IsKohi { get; private set; }

        public string FutansyaNo { get; private set; }

        public string JyukyusyaNo { get; private set; }

        public string KohiKbnName
        {
            get
            {
                string result = "公費";
                if (!(StartDate <= Sinday && EndDate >= Sinday))
                {
                    result = "×" + result;
                }
                return result;
            }
        }

        public string RodoBango
        {
            get
            {
                if (HokenKbn == 11)
                {
                    return RousaiKofuNo;
                }
                return "";
            }
        }

        public string NenkinBango
        {
            get
            {
                if (HokenKbn == 12)
                {
                    return RousaiKofuNo;
                }
                return "";
            }
        }

        public string KenkoKanriBango
        {
            get
            {
                if (HokenKbn == 13)
                {
                    return RousaiKofuNo;
                }
                return "";
            }
        }

        public string HokenKbnName { get => GetHokenKbnName(); }

        public string KigoBango { get => GetKigoBango(); }

        public string EdaNoDisplay { get => GetEdaNoDisplay(); }

        public string HokenNoDisplay { get => GetHokenNoDisplay(); }

        public bool IsExpirated
        {
            get
            {
                return !(StartDate <= Sinday && EndDate >= Sinday);
            }
        }

        private string GetEdaNoDisplay()
        {
            if (string.IsNullOrEmpty(EdaNo))
            {
                return string.Empty;
            }
            return "(" + EdaNo.PadLeft(2, '0') + ")";
        }

        private string GetHokenNoDisplay()
        {
            string result = string.Empty;
            switch (HokenKbn)
            {
                case 1:
                case 2:
                    result = HokensyaNo;
                    break;
                case 11:
                    result = RodoBango;
                    break;
                case 12:
                    result = NenkinBango;
                    break;
                case 13:
                    result = KenkoKanriBango;
                    break;
                case 14:
                    result = "";
                    break;
            }
            return result;
        }

        private string GetKigoBango()
        {

            string result = string.Empty;
            switch (HokenKbn)
            {
                case 1:
                case 2:
                    result = Kigo + "・" + Bango;
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                    result = "";
                    break;
            }
            return result;
        }

        private string GetHokenKbnName()
        {
            string result = string.Empty;
            switch (HokenKbn)
            {
                case 0:
                    result = "自費";
                    break;
                case 1:
                    result = "社保";
                    break;
                case 2:
                    if (HokensyaNo.Length == 8 &&
                        HokensyaNo.StartsWith("39"))
                    {
                        result = "後期";
                    }
                    else if (HokensyaNo.Length == 8 &&
                        HokensyaNo.StartsWith("67"))
                    {
                        result = "退職";
                    }
                    else
                    {
                        result = "国保";
                    }
                    break;
                case 11:
                case 12:
                case 13:
                    result = "労災";
                    break;
                case 14:
                    result = "自賠";
                    break;
            }
            if (!string.IsNullOrEmpty(result))
            {
                if (!IsExpirated)
                {
                    return result;
                }
                result = "×" + result;
            }
            return result;
        }

    }
}
