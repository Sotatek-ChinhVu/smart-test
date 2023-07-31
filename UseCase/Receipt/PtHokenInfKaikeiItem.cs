using Domain.Models.Receipt;
using Helper.Common;
using Helper.Constants;

namespace UseCase.Receipt;

public class PtHokenInfKaikeiItem
{
    public PtHokenInfKaikeiItem(PtHokenInfKaikeiModel model)
    {
        HokenId = model.HokenId;
        PtId = model.PtId;
        HokenKbn = model.HokenKbn;
        Houbetu = model.Houbetu;
        HonkeKbn = model.HonkeKbn;
        HokensyaNo = model.HokensyaNo;
        HokenStartDate = model.HokenStartDate;
        HokenEndDate = model.HokenEndDate;
    }

    public int HokenId { get; private set; }

    public long PtId { get; private set; }

    public int HokenKbn { get; private set; }

    public string Houbetu { get; private set; }

    public int HonkeKbn { get; private set; }

    public string HokensyaNo { get; private set; }

    public int HokenStartDate { get; private set; }

    public int HokenEndDate { get; private set; }

    public string HokenName
    {
        get
        {
            string result = string.Empty;
            if (HokenId == 0)
            {
                return string.Empty;
            }
            result += HokenId.ToString().PadLeft(2, '0');
            result += " ";
            if (PtId == 0)
            {
                return string.Empty;
            }

            switch (HokenKbn)
            {
                case 0:
                    if (Houbetu == HokenConstant.HOUBETU_JIHI_108)
                    {
                        result += "自費";
                    }
                    else if (Houbetu == HokenConstant.HOUBETU_JIHI_109)
                    {
                        result += "自費レセ";
                    }
                    else
                    {
                        result += "自費";
                    }
                    break;
                case 1:
                    if (Houbetu == HokenConstant.HOUBETU_NASHI)
                    {
                        result += "公費";
                    }
                    else
                    {
                        result += "社保";
                    }
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

            string sBuff;
            if (HokenStartDate > 0)
            {
                sBuff = string.Format("{0, -11}", CIUtil.SDateToShowWDate(HokenStartDate));
            }
            else
            {
                sBuff = string.Format("{0, -11}", " ");
            }

            sBuff += " ～ ";

            if (HokenEndDate > 0 && HokenEndDate < 99999999)
            {
                sBuff += string.Format("{0, -11}", CIUtil.SDateToShowWDate(HokenEndDate));
            }
            else
            {
                sBuff += string.Format("{0, -11}", " ");
            }

            return result + " " + sBuff;
        }
    }
}
