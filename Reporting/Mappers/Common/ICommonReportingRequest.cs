using Reporting.Mappers.Common;

namespace Reporting.Mappers
{
    public interface ICommonReportingRequest
    {
        CommonReportingRequestModel GetData();

        Dictionary<string, string> GetSingleFieldData();

        Dictionary<string, bool> GetVisibleFieldData();

        Dictionary<string, bool> GetWrapFieldData();

        List<Dictionary<string, CellModel>> GetTableFieldData();

        string GetRowCountFieldName();

        int GetReportType();
    }
}
