using Reporting.Mappers.Common;

namespace Reporting.ReceiptPrint.Service;

public interface IReceiptPrintService
{
    CommonReportingRequestModel GetReceiptPrint(int hpId, string formName, int prefNo, int reportId, int reportEdaNo, int dataKbn, int ptId, int seikyuYm, int sinYm, int hokenId, int diskKind, int diskCnt, int welfareType, List<string> printHokensyaNos, int hokenKbn, ReseputoShubetsuModel selectedReseputoShubeusu, int departmentId, int doctorId, int printNoFrom, int printNoTo, int sort);
}
