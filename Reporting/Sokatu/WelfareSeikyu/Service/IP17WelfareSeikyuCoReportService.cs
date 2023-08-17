using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service;

public interface IP17WelfareSeikyuCoReportService
{
    CommonReportingRequestModel GetP17WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
