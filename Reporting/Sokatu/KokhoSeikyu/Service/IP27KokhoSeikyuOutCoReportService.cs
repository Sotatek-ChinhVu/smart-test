using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP27KokhoSeikyuOutCoReportService
{
    CommonReportingRequestModel GetP27KokhoSeikyuOutReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
