using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP24KokhoSeikyuCoReportService
{
    CommonReportingRequestModel GetP24KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
