using Reporting.Mappers.Common;
using Reporting.Sokatu.KokhoSokatu.Service;

namespace Reporting.ReceiptPrint.Service
{
    public class ReceiptPrintService : IReceiptPrintService
    {
        private readonly IP08KokhoSokatuCoReportService _p08KokhoSokatuCoReportService;

        public ReceiptPrintService(IP08KokhoSokatuCoReportService p08KokhoSokatuCoReportService)
        {
            _p08KokhoSokatuCoReportService = p08KokhoSokatuCoReportService;
        }

        public CommonReportingRequestModel GetReceiptPrint(int hpId, int prefNo, int reportId, int reportEdaNo, int ptId, int seikyuYm, int sinYm, int hokenId)
        {
            if (prefNo == 28 && reportId == 102 && reportEdaNo == 0)
            {
                return _p08KokhoSokatuCoReportService.GetP08KokhoSokatuReportingData(hpId, seikyuYm);
            }if()

            return new();
        }
    }
}
