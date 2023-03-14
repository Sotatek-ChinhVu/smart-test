using Domain.Models.Receipt;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace UseCase.Receipt;

public class ReceInfItem
{
    public ReceInfItem(ReceInfModel model)
    {
        SeikyuYm = model.SeikyuYm;
        PtId = model.PtId;
        SinYm = model.SinYm;
        HokenId = model.HokenId;
        HokenId2 = model.HokenId2;
        KaId = model.KaId;
        TantoId = model.TantoId;
        ReceSbt = model.ReceSbt;
        HokenKbn = model.HokenKbn;
        HokenSbtCd = model.HokenSbtCd;
        Houbetu = model.Houbetu;
        Kohi1Id = model.Kohi1Id;
        Kohi2Id = model.Kohi2Id;
        Kohi3Id = model.Kohi3Id;
        Kohi4Id = model.Kohi4Id;
        Kohi1Houbetu = model.Kohi1Houbetu;
        Kohi2Houbetu = model.Kohi2Houbetu;
        Kohi3Houbetu = model.Kohi3Houbetu;
        Kohi4Houbetu = model.Kohi4Houbetu;
        HonkeKbn = model.HonkeKbn;
    }

    public int SeikyuYm { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int HokenId2 { get; private set; }

    public int KaId { get; private set; }

    public int TantoId { get; private set; }

    public string ReceSbt { get; private set; }

    public int HokenKbn { get; private set; }

    public int HokenSbtCd { get; private set; }

    public string Houbetu { get; private set; }

    public int Kohi1Id { get; private set; }

    public int Kohi2Id { get; private set; }

    public int Kohi3Id { get; private set; }

    public int Kohi4Id { get; private set; }

    public string Kohi1Houbetu { get; private set; }

    public string Kohi2Houbetu { get; private set; }

    public string Kohi3Houbetu { get; private set; }

    public string Kohi4Houbetu { get; private set; }

    public int HonkeKbn { get; private set; }

    #region Extend param
    public string SeikyuYmDisplay => CIUtil.SMonthToShowSMonth(SeikyuYm);

    public string SinYmDisplay => CIUtil.SMonthToShowSMonth(SinYm);

    public string HokenPatternName => GetHokenName();

    public bool HaveKohi
    {
        get => Kohi1Id > 0 ||
               Kohi2Id > 0 ||
               Kohi3Id > 0 ||
               Kohi4Id > 0;
    }

    private string GetHokenName()
    {
        if (PtId == 0 && SinYm == 0 && HokenId == 0)
        {
            return string.Empty;
        }

        string hokenName = string.Empty;

        string prefix = string.Empty;
        string postfix = string.Empty;

        if (HokenSbtCd == 0)
        {
            switch (HokenKbn)
            {
                case 0:
                    if (HokenId > 0)
                    {
                        if (Houbetu == HokenConstant.HOUBETU_JIHI_108)
                        {
                            hokenName += "自費";
                        }
                        else if (Houbetu == HokenConstant.HOUBETU_JIHI_109)
                        {
                            hokenName += "自費レセ";
                        }
                        else
                        {
                            hokenName += "自費";
                        }
                    }
                    else
                    {
                        hokenName += "自費";
                    }
                    if (HaveKohi)
                    {
                        int nomarlKohiCount = GetKohiCount();
                        prefix = GetKohiCountName(nomarlKohiCount);
                        postfix = GetKohiName();
                    }
                    break;
                case 13:
                    hokenName += "労災";
                    break;
                case 14:
                    hokenName += "自賠責";
                    break;
            }
        }
        else
        {
            if (HokenSbtCd < 0)
            {
                return hokenName;
            }
            string hokenSbtCd = HokenSbtCd.AsString().PadRight(3, '0');
            int firstNum = hokenSbtCd[0].AsInteger();
            int secondNum = hokenSbtCd[1].AsInteger();
            int thirNum = hokenSbtCd[2].AsInteger();
            switch (firstNum)
            {
                case 1:
                    hokenName += "社保";
                    break;
                case 2:
                    hokenName += "国保";
                    break;
                case 3:
                    hokenName += "後期";
                    break;
                case 4:
                    hokenName += "退職";
                    break;
                case 5:
                    hokenName += "公費";
                    break;
            }

            if (secondNum > 0)
            {

                prefix = GetKohiCountName(thirNum);


                switch (HonkeKbn)
                {
                    case 1:
                        prefix += "(本)";
                        break;
                    case 2:
                        prefix += "(家)";
                        break;
                    default:
                        break;
                }
                postfix = GetKohiName();

            }
        }

        if (!string.IsNullOrEmpty(postfix))
        {
            return hokenName + prefix + " " + postfix;
        }
        return hokenName + prefix;
    }

    private int GetKohiCount()
    {
        int result = 0;
        if (Kohi1Id > 0 && Kohi1Houbetu != HokenConstant.HOUBETU_MARUCHO)
        {
            result++;
        }
        if (Kohi2Id > 0 && Kohi1Houbetu != HokenConstant.HOUBETU_MARUCHO)
        {
            result++;
        }
        if (Kohi3Id > 0 && Kohi1Houbetu != HokenConstant.HOUBETU_MARUCHO)
        {
            result++;
        }
        if (Kohi4Id > 0 && Kohi1Houbetu != HokenConstant.HOUBETU_MARUCHO)
        {
            result++;
        }
        if (result > 0)
        {
            return result + 1;
        }
        return result;
    }

    private string GetKohiCountName(int kohicount)
    {
        if (kohicount <= 0)
        {
            return string.Empty;
        }
        if (kohicount == 1)
        {
            return "単独";
        }
        else
        {
            return kohicount + "併";
        }
    }

    private string GetKohiName()
    {
        string postfix = string.Empty;
        if (Kohi1Id > 0)
        {
            if (!string.IsNullOrEmpty(postfix))
            {
                postfix += "-";
            }
            if (Kohi1Houbetu == HokenConstant.HOUBETU_MARUCHO)
            {
                postfix += "マル長";
            }
            else
            {
                postfix += Kohi1Houbetu;
            }
        }
        if (Kohi2Id > 0)
        {
            if (!string.IsNullOrEmpty(postfix))
            {
                postfix += "-";
            }
            if (Kohi2Houbetu == HokenConstant.HOUBETU_MARUCHO)
            {
                postfix += "マル長";
            }
            else
            {
                postfix += Kohi2Houbetu;
            }
        }
        if (Kohi3Id > 0)
        {
            if (!string.IsNullOrEmpty(postfix))
            {
                postfix += "-";
            }
            if (Kohi3Houbetu == HokenConstant.HOUBETU_MARUCHO)
            {
                postfix += "マル長";
            }
            else
            {
                postfix += Kohi3Houbetu;
            }
        }
        if (Kohi4Id > 0)
        {
            if (!string.IsNullOrEmpty(postfix))
            {
                postfix += "+";
            }
            if (Kohi4Houbetu == HokenConstant.HOUBETU_MARUCHO)
            {
                postfix += "マル長";
            }
            else
            {
                postfix += Kohi4Houbetu;
            }
        }

        return postfix;
    }
    #endregion
}
