using Reporting.Mappers.Common;

namespace Reporting.Memo.Mapper;

public class MemoMsgMapper : CommonReportingRequest
{
    private readonly string _jobName;
    private readonly Dictionary<string, string> _extralData;

    public MemoMsgMapper(string jobName, Dictionary<string, string> extralData)
    {
        _jobName = jobName;
        _extralData = extralData;
    }

    public override int GetReportType()
    {
        return (int)CoReportType.MemoMsg;
    }

    public override string GetRowCountFieldName()
    {
        return string.Empty;
    }

    public override Dictionary<string, string> GetSingleFieldData()
    {
        return new();
    }

    public override List<Dictionary<string, CellModel>> GetTableFieldData()
    {
        return new();
    }

    public override Dictionary<string, string> GetExtralData()
    {
        return _extralData;
    }
    public override Dictionary<string, string> GetFileNamePageMap()
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

    public override string GetJobName()
    {
        return _jobName;
    }
}
