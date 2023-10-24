using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service;

public interface IP21WelfareSokatuCoReportService
{
    CommonReportingRequestModel GetP21WelfareSokatuCoReportService(int hpId, int seikyuYm, SeikyuType seikyuType);
}
