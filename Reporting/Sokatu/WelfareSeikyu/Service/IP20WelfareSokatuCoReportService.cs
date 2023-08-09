using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service;

public interface IP20WelfareSokatuCoReportService
{
    CommonReportingRequestModel GetP20WelfareSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}