using Reporting.Mappers.Common;

namespace Reporting.Statistics.Sta1010.Mapper;

public class Sta1010Mapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly Dictionary<string, string> _extralData;
    private readonly string _rowCountFieldName;
    private readonly string _formFileName;

    public Sta1010Mapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData, Dictionary<string, string> extralData, string rowCountFieldName, string formFileName)
    {
        _singleFieldData = singleFieldData;
        _tableFieldData = tableFieldData;
        _extralData = extralData;
        _rowCountFieldName = rowCountFieldName;
        _formFileName = formFileName;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.Sta1010;
    }

    public override string GetRowCountFieldName()
    {
        return _rowCountFieldName;
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
}
