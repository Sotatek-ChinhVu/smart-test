using Reporting.Mappers.Common;

namespace Reporting.Sokatu.Syaho.Mapper;

public class SyahoMapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<string, bool> _visibleFieldList;
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<string, string> _fileNamePageMap;

    public SyahoMapper(Dictionary<string, string> singleFieldData, Dictionary<string, bool> visibleFieldList, Dictionary<int, Dictionary<string, string>> setFieldData, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData, Dictionary<string, string> fileNamePageMap)
    {
        _singleFieldData = singleFieldData;
        _visibleFieldList = visibleFieldList;
        _setFieldData = setFieldData;
        _listTextData = listTextData;
        _extralData = extralData;
        _fileNamePageMap = fileNamePageMap;
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

    public override Dictionary<string, bool> GetVisibleFieldData()
    {
        return _visibleFieldList;
    }

    public override Dictionary<string, bool> GetVisibleAtPrint()
    {
        return new();
    }

    public override Dictionary<string, string> GetSystemConfigList()
    {
        return new();
    }

    public override Dictionary<string, string> GetFileNamePageMap()
    {
        return _fileNamePageMap;
    }

    public override Dictionary<string, bool> GetWrapFieldData()
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

    public override Dictionary<int, List<ListTextObject>> GetListTextData()
    {
        return _listTextData;
    }

    public override Dictionary<int, Dictionary<string, string>> GetSetFieldData()
    {
        return _setFieldData;
    }
}
