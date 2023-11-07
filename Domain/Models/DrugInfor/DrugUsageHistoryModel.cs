using Helper.Constants;
using Helper.Extension;
using System.Globalization;

namespace Domain.Models.DrugInfor;

public class DrugUsageHistoryModel
{
    public DrugUsageHistoryModel(int sinKouiKbnUsage, int sinDate, long raiinNo, long rpNo, long rpEdaNo, int odrKouiKbn, int daysCnt, int rowNo, int sinKouiKbn, string itemCd, string itemName, double suryo, string unitName)
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

    public string UnitName { get; private set; }


    #region extend Param
    public int EndDate
    {
        get
        {
            DateTime.TryParseExact(SinDate.AsString(), "yyyyMMdd",
                                   CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime outputDate);

            int result = outputDate.AddDays(DaysCnt - 1).ToString("yyyyMMdd").AsInteger();
            return result;
        }
    }

    public int KouiKbn
    {
        get
        {
            //行為コードが0ならオーダー属性の行為コードを取得　
            //行為コードが30ならオーダー属性の行為コードを取得
            if (SinKouiKbn == 0 || SinKouiKbn == 30)
            {
                return OdrKouiKbn;
            }
            //行為コードが20なら用法で絞ったオーダー明細の行為コードを取得
            else if (SinKouiKbn == 20)
            {
                return SinKouiKbnUsage == -1 ? OdrKouiKbn : SinKouiKbnUsage;
            }
            else if (ItemCd == ItemCdConst.TouyakuTokuSyo2Syoho || ItemCd == ItemCdConst.TouyakuTokuSyo2Syohosen)
            {
                return 80;
            }
            else if (ItemCd == ItemCdConst.SyosaiKihon)
            {
                return 9;
            }
            else if (ItemCd == ItemCdConst.JikanKihon)
            {
                return 10;
            }
            else
            {
                return SinKouiKbn;
            }
        }
    }

    public string KanjiName
    {
        get
        {
            var quantity = Quantity.AsInteger();
            if (ItemCd == ItemCdConst.SyosaiKihon)
            {
                if (SyosaiConst.ReceptionShinDict.ContainsKey(quantity))
                {
                    return SyosaiConst.ReceptionShinDict[quantity];
                }
                else
                {
                    return string.Empty;
                }
            }
            else if (ItemCd == ItemCdConst.JikanKihon)
            {
                if (JikanConst.JikanKotokuDict.ContainsKey(quantity))
                {
                    return JikanConst.JikanKotokuDict[quantity];
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return ItemName;
            }
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

    public int ShinOrJikanSortIndex
    {
        get
        {
            if (ItemCd == ItemCdConst.SyosaiKihon)
            {
                switch (Quantity.AsInteger())
                {
                    case SyosaiConst.Unspecified:
                        return 0;
                    case SyosaiConst.Syosin:
                        return 1;
                    case SyosaiConst.Syosin2:
                        return 2;
                    case SyosaiConst.Saisin:
                        return 3;
                    case SyosaiConst.Saisin2:
                        return 4;
                    case SyosaiConst.SaisinDenwa:
                        return 5;
                    case SyosaiConst.SaisinDenwa2:
                        return 6;
                    case SyosaiConst.None:
                        return 7;
                    case SyosaiConst.Jihi:
                        return 8;
                }
            }
            else if (ItemCd == ItemCdConst.JikanKihon)
            {
                return Quantity.AsInteger();
            }
            return 0;
        }
    }
    #endregion
}
