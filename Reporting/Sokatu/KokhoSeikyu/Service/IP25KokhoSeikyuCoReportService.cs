using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP25KokhoSeikyuCoReportService
{
    CommonReportingRequestModel GetP25KokhoSeikyuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}