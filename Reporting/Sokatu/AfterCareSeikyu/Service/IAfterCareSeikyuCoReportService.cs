using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.AfterCareSeikyu.Service;

public interface IAfterCareSeikyuCoReportService
{
    CommonReportingRequestModel GetAfterCareSeikyuPrintData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
