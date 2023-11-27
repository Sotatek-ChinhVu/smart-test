using Domain.Common;
using Reporting.Mappers.Common;

namespace Reporting.ReceiptList.Service
{
    public interface IImportCSVCoReportService : IRepositoryBase
    {
        CommonExcelReportingModel GetImportCSVCoReportServiceReportingData(int hpId, int seikyuYm, Domain.Models.Receipt.ReceiptListAdvancedSearch.ReceiptListAdvancedSearchInput receiptCsvModel, bool outputTitle = false);
    }
}
