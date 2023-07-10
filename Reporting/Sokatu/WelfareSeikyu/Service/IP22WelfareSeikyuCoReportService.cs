using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service;

public interface IP22WelfareSeikyuCoReportService
{
    CommonReportingRequestModel GetP22WelfareSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType, int welfareType);
}
