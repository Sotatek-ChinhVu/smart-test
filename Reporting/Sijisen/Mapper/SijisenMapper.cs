using Reporting.Mappers.Common;

namespace Reporting.Sijisen.Mapper;

public class SijisenMapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly string _rowCountFieldName;
    private readonly string _jobName;

    public SijisenMapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData, string rowCountFieldName, string jobName)
    {
        _singleFieldData = singleFieldData;
        _tableFieldData = tableFieldData;
        _rowCountFieldName = rowCountFieldName;
        _jobName = jobName;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.Sijisen;
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
        return new();
    }

    public override Dictionary<string, string> GetFileNamePageMap()
    {
        return new();
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
        return _jobName;
    }
}
