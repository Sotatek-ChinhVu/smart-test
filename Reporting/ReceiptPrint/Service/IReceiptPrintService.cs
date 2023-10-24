using Reporting.Mappers.Common;
using System.Collections.Generic;

namespace Reporting.ReceiptPrint.Service;

public interface IReceiptPrintService
{
    CommonReportingRequestModel GetReceiptPrint(int hpId, string formName, int prefNo, int reportId, int reportEdaNo, int dataKbn, long ptId, int seikyuYm, int sinYm, int hokenId, int diskKind, int diskCnt, int welfareType, List<string> printHokensyaNos, int hokenKbn, ReseputoShubetsuModel selectedReseputoShubeusu, int departmentId, int doctorId, int printNoFrom, int printNoTo, bool includeTester, bool includeOutDrug, int sort, List<long> printPtIds);
}
