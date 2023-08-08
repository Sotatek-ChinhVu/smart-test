using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP42KokhoSeikyuCoReportService
{
    CommonReportingRequestModel GetP42KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
