using Reporting.Mappers.Common;

namespace Reporting.Sokatu.AfterCareSeikyu.Mapper;

public class AfterCareSeikyuMapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly string _formFileName;
    private readonly Dictionary<string, bool> _visibleAtPrint;

    public AfterCareSeikyuMapper(Dictionary<string, string> singleFieldData, Dictionary<string, bool> visibleAtPrint, string formFileName)
    {
        _singleFieldData = singleFieldData;
        _formFileName = formFileName;
        _visibleAtPrint = visibleAtPrint;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.KokhoSokatu;
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
        return new();
    }

    public override Dictionary<string, string> GetExtralData()
    {
        return new();
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

    public override Dictionary<string, bool> GetVisibleAtPrint()
    {
        return _visibleAtPrint;
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new();
    }
}
