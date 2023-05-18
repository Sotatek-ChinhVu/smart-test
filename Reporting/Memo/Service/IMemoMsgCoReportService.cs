using Reporting.Mappers.Common;

namespace Reporting.Memo.Service;

public interface IMemoMsgCoReportService
{
    CommonReportingRequestModel GetMemoMsgReportingData(string reportName, string title, List<string> listMessage);
}
