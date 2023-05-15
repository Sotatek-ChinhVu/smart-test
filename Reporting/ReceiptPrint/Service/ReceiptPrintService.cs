using Reporting.Mappers.Common;
using Reporting.Sokatu.KokhoSokatu.Service;

namespace Reporting.ReceiptPrint.Service
{
    public class ReceiptPrintService : IReceiptPrintService
    {
        private readonly IP28KokhoSokatuCoReportService _p28KokhoSokatuCoReportService;

        public ReceiptPrintService(IP28KokhoSokatuCoReportService p28KokhoSokatuCoReportService)
        {
            _p28KokhoSokatuCoReportService = p28KokhoSokatuCoReportService;
        }

        public CommonReportingRequestModel GetReceiptPrint(int hpId, int prefNo, int reportId, int reportEdaNo, int ptId, int seikyuYm, int sinYm, int hokenId)
        {
            if (prefNo == 28 && reportId == 102 && reportEdaNo == 0)
            {
                return _p28KokhoSokatuCoReportService.GetP28KokhoSokatuReportingData(hpId, seikyuYm);
            }

            return new();
        }
    }
}
