using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service;

public interface IP14WelfareSeikyuCoReportService
{
    CommonReportingRequestModel GetP14WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int welfareType);
}
