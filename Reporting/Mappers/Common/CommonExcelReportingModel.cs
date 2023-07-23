namespace Reporting.Mappers.Common;

public class CommonExcelReportingModel
{
    public CommonExcelReportingModel()
    {
    }

    public CommonExcelReportingModel(string fileName, string sheetName, List<string> data)
    {
        FileName = fileName;
        SheetName = sheetName;
        Data = data;
    }

    public string FileName { get; private set; }

    public string SheetName { get; private set; }

    public List<string> Data { get; private set; }

    public string JobName { get; private set; } = string.Empty;
}
