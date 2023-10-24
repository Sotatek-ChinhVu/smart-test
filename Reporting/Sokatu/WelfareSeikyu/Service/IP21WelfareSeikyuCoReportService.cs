using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service;

public interface IP21WelfareSeikyuCoReportService
{
    CommonReportingRequestModel GetP21WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
