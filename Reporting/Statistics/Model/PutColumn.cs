namespace Reporting.Statistics.Model;

public struct PutColumn
{
    public string ColName { get; set; }

    public string JpName { get; set; }

    public bool IsTotal { get; set; }

    public string CsvColName { get; set; }

    public PutColumn(string colName, string jpName, bool isTotal = true, string? csvColName = null)
    {
        ColName = colName;
        JpName = jpName;
        IsTotal = isTotal;
        CsvColName = csvColName ?? colName;
    }
}
