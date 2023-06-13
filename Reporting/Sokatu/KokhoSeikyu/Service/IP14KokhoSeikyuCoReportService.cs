using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP14KokhoSeikyuCoReportService
{
    CommonReportingRequestModel GetP14KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
