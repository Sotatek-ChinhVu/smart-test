using Reporting.Mappers.Common;

namespace Reporting.Karte1.Mapper;

public class Karte1Mapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _extralData;
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<int, ReportConfigModel> _reportConfigModelPerPage;

    public Karte1Mapper(Dictionary<string, string> extralData, Dictionary<string, string> singleFieldData, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<int, ReportConfigModel> reportConfigModelPerPage)
    {
        _extralData = extralData;
        _singleFieldData = singleFieldData;
        _listTextData = listTextData;
        _reportConfigModelPerPage = reportConfigModelPerPage;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.Karte1;
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
        return "カルテ１号紙";
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

    public override Dictionary<int, List<ListTextObject>> GetListTextData()
    {
        return _listTextData;
    }

    public override Dictionary<int, ReportConfigModel> GetReportConfigModelPerPage()
    {
        return _reportConfigModelPerPage;
    }

    public override Dictionary<string, string> GetFileNamePageMap()
    {
        var fileName = new Dictionary<string, string>
        {
            { "1", "fmKarte1.rse" },
            { "2", "fmKarte1_2.rse" },
        };
        return fileName;
    }
}
