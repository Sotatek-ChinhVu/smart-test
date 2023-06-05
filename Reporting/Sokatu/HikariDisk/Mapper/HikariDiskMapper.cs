using Reporting.Mappers.Common;

namespace Reporting.Sokatu.HikariDisk.Mapper;

public class HikariDiskMapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly string _formFileName;
    private readonly Dictionary<string, bool> _visibleFieldList;

    public HikariDiskMapper(Dictionary<string, string> singleFieldData, Dictionary<string, bool> visibleFieldList, string formFileName)
    {
        _singleFieldData = singleFieldData;
        _formFileName = formFileName;
        _visibleFieldList = visibleFieldList;
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
        return _visibleFieldList;
    }

    public override Dictionary<string, bool> GetWrapFieldData()
    {
        return new();
    }
}
