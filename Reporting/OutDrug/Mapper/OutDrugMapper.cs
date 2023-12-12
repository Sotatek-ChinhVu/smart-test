using Reporting.Mappers.Common;

namespace Reporting.OutDrug.Mapper;

public class OutDrugMapper : CommonReportingRequest
{
    private readonly Dictionary<int, Dictionary<string, string>> _setFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<int, ReportConfigModel> _reportConfigModel;
    private readonly Dictionary<string, string> _fileNamePageMap;
    private readonly string _jobName;
    private readonly Dictionary<string, string> _extralData;

    public OutDrugMapper(Dictionary<int, Dictionary<string, string>> setFieldData, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<int, ReportConfigModel> reportConfigModel, Dictionary<string, string> fileNamePageMap, Dictionary<string, string> extralData, string jobName)
    {
        _setFieldData = setFieldData;
        _listTextData = listTextData;
        _reportConfigModel = reportConfigModel;
        _fileNamePageMap = fileNamePageMap;
        _extralData = extralData;
        _jobName = jobName;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.OutDrug;
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
        return _jobName;
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        return new();
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

    public override Dictionary<int, Dictionary<string, string>> GetSetFieldData()
    {
        return _setFieldData;
    }

    public override Dictionary<int, ReportConfigModel> GetReportConfigModelPerPage()
    {
        return _reportConfigModel;
    }

    public override Dictionary<int, List<ListTextObject>> GetListTextData()
    {
        return _listTextData;
    }

    public override Dictionary<string, string> GetFileNamePageMap()
    {
        return _fileNamePageMap;
    }
}