using Reporting.Mappers.Common;

namespace Reporting.Sokatu.KokhoSeikyu.Mapper;

public class KokhoSeikyuMapper : CommonReportingRequest
{
    private readonly Dictionary<int, Dictionary<string, string>> _singleFieldDataM;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, string> _extralData;
    private readonly string _formFileName;
    private readonly Dictionary<string, bool> _visibleFieldData;

    public KokhoSeikyuMapper(Dictionary<int, Dictionary<string, string>> singleFieldDataM, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData, string formFileName, Dictionary<string, string> singleFieldData, Dictionary<string, bool> visibleFieldData)
    {
        _singleFieldDataM = singleFieldDataM;
        _listTextData = listTextData;
        _extralData = extralData;
        _formFileName = formFileName;
        _singleFieldData = singleFieldData;
        _visibleFieldData = visibleFieldData;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.KokhoSeikyu;
    }

    public override string GetRowCountFieldName()
    {
        return string.Empty;
    }

    public override List<Dictionary<string, CellModel>> GetTableFieldData()
    {
        return new();
    }

    public override Dictionary<string, string> GetExtralData()
    {
        return _extralData;
    }

    public override string GetJobName()
    {
        return string.Empty;
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        return _singleFieldData;
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new();
    }

    public override Dictionary<int, List<ListTextObject>> GetListTextData()
    {
        return _listTextData;
    }

    public override Dictionary<int, Dictionary<string, string>> GetSetFieldData()
    {
        return _singleFieldDataM;
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
        return _visibleFieldData;
    }
}
