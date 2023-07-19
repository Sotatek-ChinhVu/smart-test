using Reporting.Mappers.Common;

namespace Reporting.ReceTarget.Mapper;

public class ReceTargetMapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, string> _extralData;
    private readonly string _formFileName;

    public ReceTargetMapper(Dictionary<string, string> singleFieldData, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData, string formFileName)
    {
        _singleFieldData = singleFieldData;
        _listTextData = listTextData;
        _extralData = extralData;
        _formFileName = formFileName;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.ReceTarget;
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
        return "レセ対象患者一覧";
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

    public override Dictionary<string, string> GetFileNamePageMap()
    {
        var fileName = new Dictionary<string, string>
        {
            { "1", _formFileName }
        };
        return fileName;
    }
}
