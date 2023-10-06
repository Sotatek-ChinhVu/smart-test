using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP22KokhoSeikyuCoReportService
{
    CommonReportingRequestModel GetP22KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
