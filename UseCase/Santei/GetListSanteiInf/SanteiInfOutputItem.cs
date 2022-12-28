using Helper.Common;

namespace UseCase.Santei.GetListSanteiInf;

public class SanteiInfOutputItem
{
    public SanteiInfOutputItem(long id, long ptId, string itemCd, int seqNo, int alertDays, int alertTerm, string itemName, int lastOdrDate, int kisanDate, int dayCount, int santeiItemCount, double santeiItemSum, int currentMonthSanteiItemCount, double currentMonthSanteiItemSum, List<SanteiInfDetailOutputItem> listSanteInfDetails)
    {
        Id = id;
        PtId = ptId;
        ItemCd = itemCd;
        SeqNo = seqNo;
        AlertDays = alertDays;
        AlertTerm = alertTerm;
        ItemName = itemName;
        LastOdrDate = lastOdrDate;
        KisanDate = kisanDate;
        DayCount = dayCount;
        SanteiItemCount = santeiItemCount;
        SanteiItemSum = santeiItemSum;
        CurrentMonthSanteiItemCount = currentMonthSanteiItemCount;
        CurrentMonthSanteiItemSum = currentMonthSanteiItemSum;
        ListSanteInfDetails = listSanteInfDetails;
    }

    public long Id { get; private set; }

    public long PtId { get; private set; }

    public string ItemCd { get; private set; }

    public int SeqNo { get; private set; }

    public int AlertDays { get; private set; }

    public int AlertTerm { get; private set; }

    public string ItemName { get; private set; }

    public int LastOdrDate { get; private set; }

    public int KisanDate { get; private set; }

    public int DayCount { get; private set; }

    public int SanteiItemCount { get; private set; }

    public double SanteiItemSum { get; private set; }

    public int CurrentMonthSanteiItemCount { get; private set; }

    public double CurrentMonthSanteiItemSum { get; private set; }

    public List<SanteiInfDetailOutputItem> ListSanteInfDetails { get; private set; }

    #region Extend param
    public string FixedItem
    {
        get
        {
            if (PtId == 0 || PtId == 99999999)
            {
                return "●";
            }
            return string.Empty;
        }
    }

    public string DayCountDisplay
    {
        get
        {
            string dayCountDisplay = string.Empty;
            if (DayCount != 0)
            {
                switch (AlertTerm)
                {
                    case 2:
                        dayCountDisplay = DayCount + "日";
                        break;
                    case 3:
                        dayCountDisplay = DayCount + "週";
                        break;
                    case 4:
                        dayCountDisplay = DayCount + "ヶ月";
                        break;
                    case 5:
                        dayCountDisplay = DayCount + "週";
                        break;
                    case 6:
                        dayCountDisplay = DayCount + "ヶ月";
                        break;
                }
            }
            return dayCountDisplay;
        }
    }

    public string SanteiItemCountDisplay
    {
        get
        {
            if (KisanDate == 0 || SanteiItemCount == 0) return string.Empty;
            return SanteiItemCount.ToString() + "(" + CurrentMonthSanteiItemCount + ")";
        }
    }

    public string SanteiItemSumDisplay
    {
        get
        {
            if (KisanDate == 0 || SanteiItemSum == 0) return string.Empty;
            return SanteiItemSum.ToString() + "(" + CurrentMonthSanteiItemSum + ")";
        }
    }

    public string LastOdrDateDisplay
    {
        get
        {
            return CIUtil.SDateToShowSDate(LastOdrDate);
        }
    }

    public string KisanDateDisplay
    {
        get
        {
            return CIUtil.SDateToShowSDate(KisanDate);
        }
    }

    public bool IsOutDate
    {
        get
        {
            return AlertDays != 0 && DayCount > AlertDays;
        }
    }

    public bool IsSettedItemCd
    {
        get
        {
            return !string.IsNullOrEmpty(ItemCd);
        }
    }

    #endregion
}
