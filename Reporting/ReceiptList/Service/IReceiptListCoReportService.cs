using Reporting.Mappers.Common;
using Reporting.ReceiptList.Model;

namespace Reporting.ReceiptList.Service;

public interface IReceiptListCoReportService
{
    CommonReportingRequestModel GetReceiptListReportingData(int hpId, int seikyuYm, List<ReceiptInputModel> receiptListModels);
}
