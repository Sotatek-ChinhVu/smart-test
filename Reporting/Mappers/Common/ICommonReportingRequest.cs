using Reporting.Mappers.Common;

namespace Reporting.Mappers
{
    public interface ICommonReportingRequest
    {
        CommonReportingRequestModel GetData();

        List<string> GetFormNameList();

        Dictionary<string, string> GetSingleFieldData();

        Dictionary<string, bool> GetVisibleFieldData();

        Dictionary<string, bool> GetWrapFieldData();

        List<Dictionary<string, string>> GetTableFieldData();
    }
}
