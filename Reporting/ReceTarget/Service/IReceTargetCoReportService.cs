using Reporting.Mappers.Common;

namespace Reporting.ReceTarget.Service;

public interface IReceTargetCoReportService
{
    CommonReportingRequestModel GetReceTargetPrintData(int hpId, int seikyuYm);
}
