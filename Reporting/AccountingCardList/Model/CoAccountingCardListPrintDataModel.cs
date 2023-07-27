using Helper.Common;

namespace Reporting.AccountingCardList.Model;

public class CoAccountingCardListPrintDataModel
{
    public CoAccountingCardListPrintDataModel(long ptNum, string ptName, int birthday, int age, int nissu)
    {
        PtNum = ptNum.ToString();
        PtName = ptName;
        Birthday = $"{CIUtil.SDateToShowSDate(birthday)}({age}歳)";
        Nissu = nissu.ToString();
    }

    public string PtNum { get; set; }
    public string PtName { get; set; }
    public string Birthday { get; set; }
    public string Nissu { get; set; }

    public string SinId { get; set; } = string.Empty;
    public string InOut { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public string Suryo { get; set; } = string.Empty;
    public string Tensu { get; set; } = string.Empty;
    public string X { get; set; } = string.Empty;
    public string Count { get; set; } = string.Empty;
    public string Byomei { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public string Tenki { get; set; } = string.Empty;
    public bool IsBlank { get; set; } = false;
}