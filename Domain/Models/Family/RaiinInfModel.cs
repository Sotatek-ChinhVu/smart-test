using Helper.Common;
using Helper.Constants;
using System.Text;

namespace Domain.Models.Family;

public class RaiinInfModel
{
    public RaiinInfModel(long ptId, int sinDate, long raiinNo, int kaId, string kaName, int tantoId, string doctorName, int hokenPid, int hokenStartDate, int hokenEndDate, int hokenSbtCd, int hokenKbn, int kohi1HokenSbtKbn, string kohi1Houbetu, int kohi2HokenSbtKbn, string kohi2Houbetu, int kohi3HokenSbtKbn, string kohi3Houbetu, int kohi4HokenSbtKbn, string kohi4Houbetu)
    {
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        KaId = kaId;
        KaName = kaName;
        TantoId = tantoId;
        DoctorName = doctorName;
        HokenPid = hokenPid;
        HokenPatternName = GetHokenName(
                hokenPid, hokenStartDate, hokenEndDate, hokenSbtCd, hokenKbn,
                kohi1HokenSbtKbn, kohi1Houbetu, kohi2HokenSbtKbn, kohi2Houbetu,
                kohi3HokenSbtKbn, kohi3Houbetu, kohi4HokenSbtKbn, kohi4Houbetu);
    }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int KaId { get; private set; }

    public string KaName { get; private set; }

    public int TantoId { get; private set; }

    public string DoctorName { get; private set; }

    public int HokenPid { get; private set; }

    public string HokenPatternName { get; private set; }

    private string GetHokenName(int hokenPid, int hokenStartDate, int hokenEndDate, int hokenSbtCd, int hokenKbn,
     int kohi1HokenSbtKbn, string kohi1Houbetu, int kohi2HokenSbtKbn, string kohi2Houbetu,
     int kohi3HokenSbtKbn, string kohi3Houbetu, int kohi4HokenSbtKbn, string kohi4Houbetu)
    {
        if (hokenPid == CommonConstants.InvalidId)
        {
            return string.Empty;
        }

        StringBuilder hokenName = new();
        hokenName.Append(hokenPid.ToString().PadLeft(3, '0') + ". ");
        if (IsExpirated())
        {
            hokenName.Append("×").Append(hokenName);
        }

        StringBuilder prefix = new();
        StringBuilder postfix = new();
        if (hokenSbtCd == 0)
        {
            switch (hokenKbn)
            {
                case 0:
                    hokenName.Append("自費");
                    break;
                case 11:
                    hokenName.Append("労災（短期給付）");
                    break;
                case 12:
                    hokenName.Append("労災（傷病年金）");
                    break;
                case 13:
                    hokenName.Append("労災（アフターケア）");
                    break;
                case 14:
                    hokenName.Append("自賠責");
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (hokenSbtCd < 0)
            {
                return hokenName.ToString();
            }

            string subHokenSbtCd = hokenSbtCd.ToString().PadRight(3, '0');
            int firstNum = Int32.Parse(subHokenSbtCd[0].ToString());
            int secondNum = Int32.Parse(subHokenSbtCd[1].ToString());
            int thirNum = Int32.Parse(subHokenSbtCd[2].ToString());
            switch (firstNum)
            {
                case 1:
                    hokenName.Append("社保");
                    break;
                case 2:
                    hokenName.Append("国保");
                    break;
                case 3:
                    hokenName.Append("後期");
                    break;
                case 4:
                    hokenName.Append("退職");
                    break;
                case 5:
                    hokenName.Append("公費");
                    break;
            }

            if (secondNum > 0)
            {
                if (thirNum == 1)
                {
                    prefix.Append("単独");
                }
                else
                {
                    prefix.Append(thirNum + "併");
                }

                if (kohi1HokenSbtKbn != CommonConstants.InvalidId)
                {
                    if (!string.IsNullOrEmpty(postfix.ToString()))
                    {
                        postfix.Append("+");
                    }
                    if (kohi1HokenSbtKbn != 2)
                    {
                        postfix.Append(kohi1Houbetu);
                    }
                    else
                    {
                        postfix.Append("マル長");
                    }
                }
                if (kohi2HokenSbtKbn != CommonConstants.InvalidId)
                {
                    if (!string.IsNullOrEmpty(postfix.ToString()))
                    {
                        postfix.Append("+");
                    }
                    if (kohi2HokenSbtKbn != 2)
                    {
                        postfix.Append(kohi2Houbetu);
                    }
                    else
                    {
                        postfix.Append("マル長");
                    }
                }
                if (kohi3HokenSbtKbn != CommonConstants.InvalidId)
                {
                    if (!string.IsNullOrEmpty(postfix.ToString()))
                    {
                        postfix.Append("+");
                    }
                    if (kohi3HokenSbtKbn != 2)
                    {
                        postfix.Append(kohi3Houbetu);
                    }
                    else
                    {
                        postfix.Append("マル長");
                    }
                }
                if (kohi4HokenSbtKbn != CommonConstants.InvalidId)
                {
                    if (!string.IsNullOrEmpty(postfix.ToString()))
                    {
                        postfix.Append("+");
                    }
                    if (kohi4HokenSbtKbn != 2)
                    {
                        postfix.Append(kohi4Houbetu);
                    }
                    else
                    {
                        postfix.Append("マル長");
                    }
                }
            }
        }

        if (!string.IsNullOrEmpty(postfix.ToString()))
        {
            hokenName = hokenName.Append(prefix).Append("(").Append(postfix).Append(")");
        }
        else
        {
            hokenName = hokenName.Append(prefix);
        }

        string sBuff = "";
        if (hokenStartDate > 0)
        {
            sBuff = string.Format("{0, -11}", CIUtil.SDateToShowWDate(hokenStartDate));
        }
        else
        {
            sBuff = string.Format("{0, -11}", " ");
        }

        sBuff += " ～ ";

        if (hokenEndDate > 0 && hokenEndDate < 99999999)
        {
            sBuff += string.Format("{0, -11}", CIUtil.SDateToShowWDate(hokenEndDate));
        }
        else
        {
            sBuff += string.Format("{0, -11}", " ");
        }

        return hokenName + " " + sBuff;

        bool IsExpirated()
        {
            return !(hokenStartDate <= SinDate && hokenEndDate >= SinDate);
        }
    }
}
