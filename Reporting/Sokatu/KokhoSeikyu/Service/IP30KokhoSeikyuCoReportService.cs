using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP30KokhoSeikyuCoReportService
{
    CommonReportingRequestModel GetP30KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
