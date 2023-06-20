using Helper.Common;
using Reporting.Mappers.Common;

namespace Reporting.Statistics.Sta9000.Mapper;

public class Sta9000Mapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly string _formFileName;

    public Sta9000Mapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData, Dictionary<string, string> extralData, string formFileName)
    {
        _singleFieldData = singleFieldData;
        _tableFieldData = tableFieldData;
        _extralData = extralData;
        _formFileName = formFileName;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.Sta2001;
    }

    public override string GetRowCountFieldName()
    {
        return string.Empty;
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        return _singleFieldData;
    }

    public override List<Dictionary<string, CellModel>> GetTableFieldData()
    {
        return _tableFieldData;
    }

    public override Dictionary<string, string> GetExtralData()
    {
        return _extralData;
    }
    public override Dictionary<string, string> GetFileNamePageMap()
    {
        var fileName = new Dictionary<string, string>
        {
            { "1", _formFileName }
        };
        return fileName;
    }

    public override Dictionary<string, bool> GetVisibleFieldData()
    {
        return new();
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new();
    }

    public override string GetJobName()
    {
        return "PatientManagement" + CIUtil.GetJapanDateTimeNow().ToString("yyyyMMddHHmmss");
    }
}