using Reporting.Mappers.Common;

namespace Reporting.Sokatu.KoukiSeikyu.Mapper;

public class P45KoukiSeikyuMapper : CommonReportingRequest
{
    private readonly Dictionary<int, Dictionary<string, string>> _singleFieldDataM;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, string> _extralData;
    private readonly string _formFileNameP1;
    private readonly string _formFileNameP2;
    private readonly Dictionary<string, bool> _visibleFieldData;

    public P45KoukiSeikyuMapper(Dictionary<int, Dictionary<string, string>> singleFieldDataM, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData, string formFileNameP1, string formFileNameP2, Dictionary<string, string> singleFieldData, Dictionary<string, bool> visibleFieldData)
    {
        _singleFieldDataM = singleFieldDataM;
        _listTextData = listTextData;
        _extralData = extralData;
        _formFileNameP1 = formFileNameP1;
        _formFileNameP2 = formFileNameP2;
        _singleFieldData = singleFieldData;
        _visibleFieldData = visibleFieldData;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.KoukiSeikyu;
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
            { "1", _formFileNameP1 }, { "2", _formFileNameP2 }, { "3", _formFileNameP1 },{ "4", _formFileNameP1 }
        };
        return fileName;
    }

    public override Dictionary<string, bool> GetVisibleFieldData()
    {
        return _visibleFieldData;
    }
}
