using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReceiptList.Model;

namespace Reporting.ReceiptList.Service
{
    public interface IImportCSVCoReportService
    {
        CommonExcelReportingModel GetImportCSVCoReportServiceReportingData(List<ReceiptInputCsvModel> receiptListModels, CoFileType fileType, bool outputTitle = false);
    }
}
