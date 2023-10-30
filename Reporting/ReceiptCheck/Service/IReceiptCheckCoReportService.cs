using Reporting.Mappers.Common;

namespace Reporting.ReceiptCheck.Service;

public interface IReceiptCheckCoReportService
{
    CommonReportingRequestModel GetReceiptCheckCoReportingData(int hpId, List<long> ptIds, int seikyuYm);

    bool CheckOpenReceiptCheck(int hpId, List<long> ptIds, int seikyuYm);
}
