using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP20KokhoSeikyuCoReportService
{
    CommonReportingRequestModel Get20KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
