using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP27KokhoSeikyuInCoReportService
{
    CommonReportingRequestModel GetP27KokhoSeikyuInReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
