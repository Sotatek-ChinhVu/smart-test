using Entity.Tenant;
using Helper.Extension;

namespace Reporting.PatientManagement.Models;

public class HokenMstModel 
{
    public HokenMst HokenMaster { get; }

    public HokenMstModel(HokenMst hokenMaster) => HokenMaster = hokenMaster;

    public int PrefectureNumber
    {
        get { return HokenMaster.PrefNo; }
    }

    public string PrefactureName { get; set; }

    public int HokenNumber
    {
        get { return HokenMaster.HokenNo; }
    }

    public int HokenSubNumber
    {
        get { return HokenMaster.HokenSbtKbn; }
    }

    public int HokenKohiKbn
    {
        get => HokenMaster.HokenKohiKbn;
    }

    public int StartDate
    {
        get { return HokenMaster.StartDate; }
    }

    public int EndDate
    {
        get { return HokenMaster.EndDate > 0 ? HokenMaster.EndDate : 99999999; }
    }

    public int SortNumber
    {
        get { return HokenMaster.SortNo; }
    }

    public int HokenEdaNo
    {
        get { return HokenMaster.HokenEdaNo; }
    }

    public string HoubetsuNumber
    {
        get { return HokenMaster.Houbetu??string.Empty; }
    }

    public string HokenName
    {
        get { return HokenMaster.HokenName ?? string.Empty; }
    }

    public string HokenShortName
    {
        get { return HokenMaster.HokenSname ?? string.Empty; }
    }

    public string HokenNameCode
    {
        get { return HokenMaster.HokenNameCd ?? string.Empty; }
    }

    public int CheckDigit
    {
        get { return HokenMaster.CheckDigit; }
    }

    public int JyuKyuCheckDigit
    {
        get { return HokenMaster.JyukyuCheckDigit; }
    }

    public int FutansyaCheckFlag
    {
        get { return HokenMaster.IsFutansyaNoCheck; }
    }

    public int JyukyusyaCheckFlag
    {
        get { return HokenMaster.IsJyukyusyaNoCheck; }
    }

    public int TokusyuCheckFlag
    {
        get { return HokenMaster.IsTokusyuNoCheck; }
    }

    public int MoneyLimitListFlag
    {
        get { return HokenMaster.IsLimitList; }
    }

    public int IsLimitListSum
    {
        get { return HokenMaster.IsLimitListSum; }
    }

    public int OtherPrefectureValidFlag
    {
        get { return HokenMaster.IsOtherPrefValid; }
    }

    public int AgeStart
    {
        get { return HokenMaster.AgeStart; }
    }

    public int AgeEnd
    {
        get { return HokenMaster.AgeEnd; }
    }

    public double EnTenPrice
    {
        get { return HokenMaster.EnTen; }
    }

    public int SeikyuYmFlag
    {
        get { return HokenMaster.SeikyuYm; }
    }

    public int ReceSpKbn
    {
        get { return HokenMaster.ReceSpKbn; }
    }

    public int ReceSeikyuKbn
    {
        get { return HokenMaster.ReceSeikyuKbn; }
    }

    public int ReceFutanRound
    {
        get { return HokenMaster.ReceFutanRound; }
    }

    public int ReceKisai
    {
        get { return HokenMaster.ReceKisai; }
    }

    public int ReceZeroKisai
    {
        get { return HokenMaster.ReceZeroKisai; }
    }

    public int ReceFutanKbn
    {
        get { return HokenMaster.ReceFutanKbn; }
    }

    public int ReceTenKisai
    {
        get { return HokenMaster.ReceTenKisai; }
    }

    public int KogakuTotalKbn
    {
        get { return HokenMaster.KogakuTotalKbn; }
    }

    public int CalcSpKbn
    {
        get { return HokenMaster.CalcSpKbn; }
    }

    public int KogakuTekiyo
    {
        get { return HokenMaster.KogakuTekiyo; }
    }

    public int FutanYusen
    {
        get { return HokenMaster.FutanYusen; }
    }

    public int LimitKbn
    {
        get { return HokenMaster.LimitKbn; }
    }

    public int CountKbn
    {
        get { return HokenMaster.CountKbn; }
    }

    public int FutanKbn
    {
        get { return HokenMaster.FutanKbn; }
    }

    public int FutanRate
    {
        get { return HokenMaster.FutanRate; }
    }

    public int KaiFutangaku
    {
        get { return HokenMaster.KaiFutangaku; }
    }

    public int KaiLimitFutan
    {
        get { return HokenMaster.KaiLimitFutan; }
    }

    public int DayLimitFutan
    {
        get { return HokenMaster.DayLimitFutan; }
    }

    public int DayLimitCount
    {
        get { return HokenMaster.DayLimitCount; }
    }

    public int MonthLimitFutan
    {
        get { return HokenMaster.MonthLimitFutan; }
    }

    public int MonthLimitSyokan
    {
        get { return HokenMaster.MonthSpLimit; }
    }

    public int MonthLimitCount
    {
        get { return HokenMaster.MonthLimitCount; }
    }

    public int LimitFutan
    {
        get
        {
            if (KaiLimitFutan > 0)
            {
                return KaiLimitFutan;
            }
            if (DayLimitFutan > 0)
            {
                return DayLimitFutan;
            }
            if (MonthLimitFutan > 0)
            {
                return MonthLimitFutan;
            }
            return 0;
        }
    }

    public string LimitFutanWatermark
    {
        get
        {
            if (LimitFutan <= 0)
            {
                return "0";
            }
            else
            {
                return (string)new MoneyConverter().Convert(LimitFutan, null, null, null);
            }
        }
    }

    public string HokenShortNameDisplay
    {
        get { return string.Format("{0} {1}", HokenMaster.SortNo, HokenMaster.HokenSname); }
    }

    #region Custom expose
    public string SelectedValueMaster
    {
        get
        {
            string result = string.Empty;
            if (HokenMaster.HokenEdaNo == 0)
            {
                result = HokenMaster.HokenNo.ToString().PadLeft(3, '0');
            }
            else
            {
                result = HokenMaster.HokenNo.ToString().PadLeft(3, '0') + HokenMaster.HokenEdaNo;
            }

            return result;
        }
    }

    public string DisplayTextMaster
    {
        get
        {
            string DisplayText = SelectedValueMaster + " " + HokenShortName;
            if (OtherPrefectureValidFlag == 1 && !string.IsNullOrEmpty(PrefactureName))
            {

                DisplayText += "（" + PrefactureName + "）";
            }
            return DisplayText;
        }
    }

    public string DisplayTextMasterWithoutHokenNo
    {
        get
        {
            string DisplayText = HokenShortName;
            if (OtherPrefectureValidFlag == 1 && !string.IsNullOrEmpty(PrefactureName))
            {

                DisplayText += "（" + PrefactureName + "）";
            }
            return DisplayText;
        }
    }

    public string HoubetuDisplayText
    {
        get
        {
            return HokenNameCode + "(" + HoubetsuNumber.AsString().PadLeft(2, '0') + ")";
        }
    }

    public bool IsEmptyModel => HokenMaster == null;
    #endregion

}
