using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.IsuranceMst
{
    public class HokenInfModel
    {
        public HokenInfModel(int hpId, long ptId, int hokenId, int hokenKbn, string hokensyaNo, int honkeKbn, int startDate, int endDate, int sinday)
        {
            HpId = hpId;
            PtId = ptId;
            HokenId = hokenId;
            HokenKbn = hokenKbn;
            HokensyaNo = hokensyaNo;
            HonkeKbn = honkeKbn;
            StartDate = startDate;
            EndDate = endDate;
            Sinday = sinday;
        }

        public int HpId { get; private set; }

        public long PtId { get; private set; }

        public int HokenId { get; private set; }

        public int HokenKbn { get; private set; }

        public string HokensyaNo { get; private set; }

        public int HonkeKbn { get; private set; }
        
        public int StartDate { get; private set; }
        
        public int EndDate { get; private set; }


        public int Sinday { get; set; }

        public bool IsExpirated
        {
            get
            {
                return !(StartDate <= Sinday && EndDate >= Sinday);
            }
        }


        public string HokenSentaku
        {
            get
            {
                string result = string.Empty;
                result += HokenId.ToString().PadLeft(2, '0');
                result += " ";
                if (HokenKbn == 0)
                {
                    result += "";
                }
                if (string.IsNullOrEmpty(HokensyaNo) && HokenKbn < 10)
                {
                    if (HokenKbn == 1)
                    {
                        result += "";
                    }
                    return result;
                }
                switch (HokenKbn)
                {
                    case 1:
                        result += "社保";
                        break;
                    case 2:
                        if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("39"))
                        {
                            result += "後期";
                        }
                        else if (HokensyaNo.Length == 8 &&
                            HokensyaNo.StartsWith("67"))
                        {
                            result += "退職";
                        }
                        else
                        {
                            result += "国保";
                        }
                        break;
                    case 11:
                        result += "労災（短期給付）";
                        break;
                    case 12:
                        result += "労災（傷病年金）";
                        break;
                    case 13:
                        result += "労災（アフターケア）";
                        break;
                    case 14:
                        result += "自賠責";
                        break;
                }

                if (HonkeKbn != 0)
                {
                    result += "(";
                    if (HonkeKbn == 1)
                    {
                        result += "本人";
                    }
                    else
                    {
                        result += "家族";
                    }
                    result += ")";

                }

                if (!string.IsNullOrEmpty(HokensyaNo))
                {
                    result += " ";
                    result += HokensyaNo;
                }

                if (IsExpirated)
                {
                    result = "×" + result;
                }
                return result;
            }
        }
    }
}
