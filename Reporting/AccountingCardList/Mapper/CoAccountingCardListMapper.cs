using Reporting.Mappers.Common;

namespace Reporting.AccountingCardList.Mapper;

public class CoAccountingCardListMapper : CommonReportingRequest
{
    Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, string> _extralData;

    public CoAccountingCardListMapper(Dictionary<int, Dictionary<string, string>> setFieldData, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData)
    {
        _setFieldData = setFieldData;
        _listTextData = listTextData;
        _extralData = extralData;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.AccountingCardList;
    }

    public override string GetRowCountFieldName()
    {
        return string.Empty;
    }

    public override Dictionary<string, string> GetExtralData()
    {
        return _extralData;
    }

    public override string GetJobName()
    {
        return "会計カード一覧";
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        return new();
    }

    public override Dictionary<int, Dictionary<string, string>> GetSetFieldData()
    {
        return _setFieldData;
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

    public override Dictionary<int, List<ListTextObject>> GetListTextData()
    {
        return _listTextData;
    }

    public override Dictionary<string, string> GetFileNamePageMap()
    {
        return new();
    }
}
