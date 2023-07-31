using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP23KokhoSeikyuCoReportService
{
    CommonReportingRequestModel GetP23KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
