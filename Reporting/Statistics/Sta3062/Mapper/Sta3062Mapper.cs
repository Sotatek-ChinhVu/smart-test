using Reporting.Mappers.Common;

namespace Reporting.Statistics.Sta3062.Mapper;

public class Sta3062Mapper : CommonReportingRequest
{
    private readonly Dictionary<string, string> _singleFieldData;
    private readonly Dictionary<int, List<ListTextObject>> _listTextData;
    private readonly Dictionary<string, string> _extralData;
    private readonly string _formFileName;

    public Sta3062Mapper(Dictionary<string, string> singleFieldData, Dictionary<int, List<ListTextObject>> listTextData, Dictionary<string, string> extralData, string formFileName)
    {
        _singleFieldData = singleFieldData;
        _listTextData = listTextData;
        _extralData = extralData;
        _formFileName = formFileName;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.Sta3062;
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        return _singleFieldData;
    }

    public override Dictionary<string, string> GetExtralData()
    {
        return _extralData;
    }
    public override Dictionary<int, List<ListTextObject>> GetListTextData()
    {
        return _listTextData;
    }

    //public override string GetJobName()
    //{
    //    return "カルテ３号紙";
    //}

    public override Dictionary<string, string> GetFileNamePageMap()
    {
        var fileName = new Dictionary<string, string>
    {
        { "1", _formFileName }
    };
        return fileName;
    }
}
