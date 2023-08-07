using Reporting.Mappers.Common;

namespace Reporting.Receipt.Service
{
    public interface IReceiptCoReportService
    {
        CommonReportingRequestModel GetReceiptData(int hpId, long ptId, int sinYm, int departmentId, int doctorId, string receSbt, int printNoFrom, int printNoTo, int hokenId, int sort, bool isNoCreatingReceData = false, bool isPrintReceList = false);
    }
}
