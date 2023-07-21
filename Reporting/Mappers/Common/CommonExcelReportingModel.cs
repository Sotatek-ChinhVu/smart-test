namespace Reporting.Mappers.Common;

public class CommonExcelReportingModel
{
    public CommonExcelReportingModel(string fileName, List<string> data)
    {
        FileName = fileName;
        Data = data;
    }

    public string FileName { get; private set; }

    public List<string> Data { get; private set; }
}
