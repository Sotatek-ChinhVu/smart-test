using Reporting.Mappers.Common;

namespace Reporting.ReceiptCheck.Mapper;

public class CoReceiptCheckMapper : CommonReportingRequest
{
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, string> _extralData;

    public CoReceiptCheckMapper(Dictionary<int, Dictionary<string, string>> setFieldData, Dictionary<string, string> singleFieldData, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData)
    {
        _setFieldData = setFieldData;
        _singleFieldData = singleFieldData;
        _listTextData = listTextData;
        _extralData = extralData;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.ReceiptCheck;
    }

    public override string GetRowCountFieldName()
    {
        return string.Empty;
    }

    public override Dictionary<string, string> GetExtralData()
    {
        return _extralData;
    }

    public override Dictionary<int, List<ListTextObject>> GetListTextData()
    {
        return _listTextData;
    }

    public override Dictionary<int, Dictionary<string, string>> GetSetFieldData()
    {
        return _setFieldData;
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
}
