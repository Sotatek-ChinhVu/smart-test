using Reporting.Mappers.Common;

namespace Reporting.ReceiptCheck.Mapper;

public class CoReceiptCheckMapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;

    public CoReceiptCheckMapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData)
    {
        _singleFieldData = singleFieldData;
        _tableFieldData = tableFieldData;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.ReceiptCheck;
    }

    public override string GetRowCountFieldName()
    {
        return string.Empty;
    }

    public override string GetJobName()
    {
        return "レセプトチェックリスト";
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
        return new();
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new();
    }

    public override Dictionary<string, string> GetExtralData()
    {
        Dictionary<string, string> result = new()
        {
            { "maxRow", "45" }
        };
        return result;
    }
}
