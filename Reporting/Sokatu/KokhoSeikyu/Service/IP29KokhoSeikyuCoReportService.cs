using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP29KokhoSeikyuCoReportService
{
    CommonReportingRequestModel GetP29KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
