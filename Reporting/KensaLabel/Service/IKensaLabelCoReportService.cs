using Reporting.KensaLabel.Model;
using Reporting.Mappers.Common;

namespace Reporting.KensaLabel.Service
{
    public interface IKensaLabelCoReportService
    {
        CommonReportingRequestModel GetKensaLabelPrintData(int hpId, long ptId, long raiinNo, int sinDate, KensaPrinterModel printerModel);
    }
}
