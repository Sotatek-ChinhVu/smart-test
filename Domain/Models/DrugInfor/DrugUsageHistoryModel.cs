using Helper.Constants;
using Helper.Extension;
using System.Globalization;

namespace Domain.Models.DrugInfor;

public class DrugUsageHistoryModel
{
    public DrugUsageHistoryModel(int sinKouiKbnUsage, int sinDate, long raiinNo, long rpNo, long rpEdaNo, int odrKouiKbn, int daysCnt, int rowNo, int sinKouiKbn, string itemCd, string itemName, double suryo, int unitSbt, string unitName)
    {
        SinKouiKbnUsage = sinKouiKbnUsage;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        RpNo = rpNo;
        RpEdaNo = rpEdaNo;
        OdrKouiKbn = odrKouiKbn;
        DaysCnt = daysCnt;
        RowNo = rowNo;
        SinKouiKbn = sinKouiKbn;
        ItemCd = itemCd;
        ItemName = itemName;
        Suryo = suryo;
        UnitSbt = unitSbt;
        UnitName = unitName;
    }

    public int SinKouiKbnUsage { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public long RpNo { get; private set; }

    public long RpEdaNo { get; private set; }

    public int OdrKouiKbn { get; private set; }

    public int DaysCnt { get; private set; }

    public int RowNo { get; private set; }

    public int SinKouiKbn { get; private set; }

    public string ItemCd { get; private set; }

    public string ItemName { get; private set; }

    public double Suryo { get; private set; }

    public int UnitSbt { get; private set; }

    public string UnitName { get; private set; }

    #region extend Param
    public int EndDate
    {
        get
        {
            DateTime.TryParseExact(SinDate.AsString(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime outputDate);

            int result = outputDate.AddDays(DaysCnt - 1).ToString("yyyyMMdd").AsInteger();
            return result;
        }
    }

    public double Quantity
    {
        get
        {

            if (string.IsNullOrEmpty(UnitName) && OdrKouiKbn != 10)
            {
                return 0;
            }
            else
            {
                return Suryo;
            }
        }
    }
    #endregion
}
