using Reporting.Mappers.Common;

namespace Reporting.ReceiptList.Mapper;

public class ReceiptListMapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly List<Dictionary<string, CellModel>> _tableFieldData;

    public ReceiptListMapper(Dictionary<string, string> singleFieldData, List<Dictionary<string, CellModel>> tableFieldData)
    {
        _singleFieldData = singleFieldData;
        _tableFieldData = tableFieldData;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.ReceiptList;
    }

    public override string GetRowCountFieldName()
    {
        return "lsSinYm";
    }

    public override string GetJobName()
    {
        return "レセチェック一覧";
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
}