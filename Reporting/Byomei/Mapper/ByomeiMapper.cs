using Reporting.Mappers.Common;

namespace Reporting.Byomei.Mapper;

public class ByomeiMapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;
    private readonly Dictionary<string, bool> _visibleFieldList;

    public ByomeiMapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData, Dictionary<string, bool> visibleFieldList)
    {
        _singleFieldData = singleFieldData;
        _tableFieldData = tableFieldData;
        _visibleFieldList = visibleFieldList;
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        return _singleFieldData;
    }

    public override List<Dictionary<string, CellModel>> GetTableFieldData()
    {
        return _tableFieldData;
    }

    public override Dictionary<string, bool> GetVisibleFieldData()
    {
        return _visibleFieldList;
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new Dictionary<string, bool>() { { "lsByomei", true } };
    }

    public override int GetReportType()
    {
        return (int)CoReportType.Byomei;
    }

    public override string GetRowCountFieldName()
    {
        return "lsByomei";
    }

    public override string GetJobName()
    {
        return "病名一覧";
    }
}
