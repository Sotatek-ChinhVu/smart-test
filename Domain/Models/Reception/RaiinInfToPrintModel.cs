using Domain.Models.CalculateModel;
using Helper.Common;
using Helper.Constants;
using Helper.Enum;
using Helper.Extension;

namespace Domain.Models.Reception;

public class RaiinInfToPrintModel
{
    public RaiinInfToPrintModel(PrintMode batchPrintingMode, string nameBinding, string tantoIdDisplay, int receInfKaId, int raiinInfKaId, long ptId, long ptNum, string houbetuForPrintPrescription, int hokenKbnForPrintPrescription, string receInfHoubetu, string hokensyaNo, int uketukeNo, int receInfSinYm, int raiinInfSinDate, int receInfTantoId, int raiinInfTantoId, string kaDisplay, int uketukeSbt, string uketsukeDisplay, int receInfHonkeKbn, int hokenPid, int hokenId, int hokenSbtCd, int hokenKbn, string receSbt, string hokenSbtStr, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, int kohi1HokenSbtKbn, int kohi2HokenSbtKbn, int kohi3HokenSbtKbn, int kohi4HokenSbtKbn, string kohi1Houbetu, string kohi2Houbetu, string kohi3Houbetu, string kohi4Houbetu, int raiinInfStatus, long raiinNo)
    {
        BatchPrintingMode = batchPrintingMode;
        NameBinding = nameBinding;
        TantoIdDisplay = tantoIdDisplay;
        ReceInfKaId = receInfKaId;
        RaiinInfKaId = raiinInfKaId;
        PtId = ptId;
        PtNum = ptNum;
        HoubetuForPrintPrescription = houbetuForPrintPrescription;
        HokenKbnForPrintPrescription = hokenKbnForPrintPrescription;
        ReceInfHoubetu = receInfHoubetu;
        HokensyaNo = hokensyaNo;
        UketukeNo = uketukeNo;
        ReceInfSinYm = receInfSinYm;
        RaiinInfSinDate = raiinInfSinDate;
        ReceInfTantoId = receInfTantoId;
        RaiinInfTantoId = raiinInfTantoId;
        KaDisplay = kaDisplay;
        UketukeSbt = uketukeSbt;
        UketsukeDisplay = uketsukeDisplay;
        ReceInfHonkeKbn = receInfHonkeKbn;
        HokenPid = hokenPid;
        HokenId = hokenId;
        HokenSbtCd = hokenSbtCd;
        HokenKbn = hokenKbn;
        ReceSbt = receSbt;
        HokenSbtStr = hokenSbtStr;
        Kohi1Id = kohi1Id;
        Kohi2Id = kohi2Id;
        Kohi3Id = kohi3Id;
        Kohi4Id = kohi4Id;
        Kohi1HokenSbtKbn = kohi1HokenSbtKbn;
        Kohi2HokenSbtKbn = kohi2HokenSbtKbn;
        Kohi3HokenSbtKbn = kohi3HokenSbtKbn;
        Kohi4HokenSbtKbn = kohi4HokenSbtKbn;
        Kohi1Houbetu = kohi1Houbetu;
        Kohi2Houbetu = kohi2Houbetu;
        Kohi3Houbetu = kohi3Houbetu;
        Kohi4Houbetu = kohi4Houbetu;
        RaiinInfStatus = raiinInfStatus;
        RaiinNo = raiinNo;
    }

    public RaiinInfToPrintModel(PrintMode batchPrintingMode, ReceInfModel receInf)
    {
        BatchPrintingMode = batchPrintingMode;
        TantoIdDisplay = string.Empty;
        HoubetuForPrintPrescription = string.Empty;
        HokensyaNo = string.Empty;
        KaDisplay = string.Empty;
        NameBinding = string.Empty;
        ReceInfKaId = receInf.KaId;
        ReceInfHoubetu = receInf.Houbetu;
        ReceInfSinYm = receInf.SinYm;
        ReceInfTantoId = receInf.TantoId;
        PtId = receInf.PtId;
        UketsukeDisplay = string.Empty;
        ReceInfHonkeKbn = receInf.HonkeKbn;
        HokenId = receInf.HokenId;
        HokenKbn = receInf.HokenKbn;
        ReceSbt = receInf.ReceSbt ?? string.Empty.PadRight(4, '*');
        Kohi1Id = receInf.Kohi1Id;
        Kohi2Id = receInf.Kohi2Id;
        Kohi3Id = receInf.Kohi3Id;
        Kohi4Id = receInf.Kohi4Id;
        Kohi1HokenSbtKbn = receInf.Kohi1HokenSbtKbn;
        Kohi2HokenSbtKbn = receInf.Kohi2HokenSbtKbn;
        Kohi3HokenSbtKbn = receInf.Kohi3HokenSbtKbn;
        Kohi4HokenSbtKbn = receInf.Kohi4HokenSbtKbn;
        Kohi1Houbetu = receInf.Kohi1Houbetu;
        Kohi2Houbetu = receInf.Kohi2Houbetu;
        Kohi3Houbetu = receInf.Kohi3Houbetu;
        Kohi4Houbetu = receInf.Kohi4Houbetu;
        HokenSbtCd = receInf.HokenSbtCd;
        string hokenSbtCd = HokenSbtCd.AsString();
        hokenSbtCd = hokenSbtCd.PadRight(3, '*');
        HokenSbtStr = ConvertHokenSbtLeftToReceSbtLeftQuery(hokenSbtCd[0].AsInteger()) + hokenSbtCd[2] + ReceSbt[3];
    }

    public RaiinInfToPrintModel ChangeParams(string kaDisplay, string tantoIdDisplay, long ptNum, string nameBinding)
    {
        KaDisplay = kaDisplay;
        TantoIdDisplay = tantoIdDisplay;
        NameBinding = nameBinding;
        PtNum = ptNum;
        return this;
    }

    public PrintMode BatchPrintingMode { get; private set; }

    public string NameBinding { get; private set; }

    public string TantoIdDisplay { get; private set; }

    public int ReceInfKaId { get; private set; }

    public int RaiinInfKaId { get; private set; }

    public long PtNum { get; private set; }

    public long PtId { get; private set; }

    public string HoubetuForPrintPrescription { get; private set; }

    public int HokenKbnForPrintPrescription { get; private set; }

    public string ReceInfHoubetu { get; private set; }

    public string HokensyaNo { get; private set; }

    public int UketukeNo { get; private set; }

    public int ReceInfSinYm { get; private set; }

    public int RaiinInfSinDate { get; private set; }

    public int ReceInfTantoId { get; private set; }

    public int RaiinInfTantoId { get; private set; }

    public string KaDisplay { get; private set; }

    public int UketukeSbt { get; private set; }

    public string UketsukeDisplay { get; private set; }

    public int ReceInfHonkeKbn { get; private set; }

    public int HokenPid { get; private set; }

    public int HokenId { get; private set; }

    public int HokenSbtCd { get; private set; }

    public int HokenKbn { get; private set; }

    public string ReceSbt { get; private set; }

    public string HokenSbtStr { get; private set; }

    public int Kohi1Id { get; private set; }

    public int Kohi2Id { get; private set; }

    public int Kohi3Id { get; private set; }

    public int Kohi4Id { get; private set; }

    public int Kohi1HokenSbtKbn { get; private set; }

    public int Kohi2HokenSbtKbn { get; private set; }

    public int Kohi3HokenSbtKbn { get; private set; }

    public int Kohi4HokenSbtKbn { get; private set; }

    public string Kohi1Houbetu { get; private set; }

    public string Kohi2Houbetu { get; private set; }

    public string Kohi3Houbetu { get; private set; }

    public string Kohi4Houbetu { get; private set; }

    public int RaiinInfStatus { get; private set; }

    public long RaiinNo { get; private set; }

    public string Houbetu
    {
        get
        {
            if (BatchPrintingMode == PrintMode.PrintPrescription)
            {
                return HoubetuForPrintPrescription;
            }
            return ReceInfHoubetu;
        }
    }

    public string HokenKbnName
    {
        get
        {
            if (BatchPrintingMode != PrintMode.PrintPrescription ||
                (HokenKbnForPrintPrescription == 0 &&
                string.IsNullOrEmpty(HoubetuForPrintPrescription)))
            {
                return string.Empty;
            }

            string result = string.Empty;

            if (string.IsNullOrEmpty(HoubetuForPrintPrescription))
            {
                result = "公費";
                return result;
            }

            if (Houbetu == HokenConstant.HOUBETU_NASHI)
            {
                result = "公費";
                return result;
            }

            switch (HokenKbnForPrintPrescription)
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
            return result;
        }
    }

    public int SinDate
    {
        get
        {
            if (BatchPrintingMode != PrintMode.PrintPrescription)
            {
                return ReceInfSinYm;
            }
            return RaiinInfSinDate;
        }
    }

    public string SinDateBinding
    {
        get
        {
            if (SinDate == 0)
            {
                return string.Empty;
            }
            return CIUtil.SDateToShowSDate(SinDate);
        }
    }

    public string SinDateYMBinding
    {
        get
        {
            if (SinDate == 0)
            {
                return string.Empty;
            }
            if (BatchPrintingMode != PrintMode.PrintPrescription)
            {
                return CIUtil.SMonthToShowSMonth(SinDate);
            }
            return CIUtil.SMonthToShowSMonth(SinDate / 100);
        }
    }

    public int TantoId
    {
        get
        {
            if (BatchPrintingMode != PrintMode.PrintPrescription)
            {
                return ReceInfTantoId;
            }
            return RaiinInfTantoId;
        }
    }

    public int KaId
    {
        get
        {
            if (BatchPrintingMode != PrintMode.PrintPrescription)
            {
                return ReceInfKaId;
            }
            return RaiinInfKaId;
        }
    }

    public int UketsukeSbt
    {
        get
        {
            if (BatchPrintingMode == PrintMode.PrintPrescription)
            {
                return (UketukeSbt).AsInteger();
            }
            return 0;
        }
    }

    public string HokenName
    {
        get
        {
            return GetHokenName();
        }
    }

    public string ReceSbtBinding
    {
        get
        {
            return ReceSbt ?? string.Empty.PadRight(4, '*');
        }
    }

    public bool HaveKohi
    {
        get => Kohi1Id > 0 || Kohi2Id > 0 || Kohi3Id > 0 || Kohi4Id > 0;
    }

    public string ConvertHokenSbtLeftToReceSbtLeftQuery(int hokenSbtLeft)
    {
        string sbt;
        switch (hokenSbtLeft)
        {
            // 社保
            case 1:
            // 国保
            case 2:
                sbt = "11";
                break;
            // 公費
            case 5:
                sbt = "12";
                break;
            // 後期
            case 3:
                sbt = "13";
                break;
            // 退職
            case 4:
                sbt = "14";
                break;
            default:
                sbt = "00";
                break;
        }
        return sbt;
    }

    public string GetHokenName()
    {
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
                        prefix += GetKohiCountName(nomarlKohiCount);
                        postfix = GetKohiName();
                    }
                    break;
                case 11:
                case 12:
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

                prefix += GetKohiCountName(thirNum);
                switch (ReceInfHonkeKbn)
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
        if (Kohi1Id > 0 && Kohi1HokenSbtKbn != 2)
        {
            result++;
        }
        if (Kohi2Id > 0 && Kohi2HokenSbtKbn != 2)
        {
            result++;
        }
        if (Kohi3Id > 0 && Kohi3HokenSbtKbn != 2)
        {
            result++;
        }
        if (Kohi4Id > 0 && Kohi4HokenSbtKbn != 2)
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
}