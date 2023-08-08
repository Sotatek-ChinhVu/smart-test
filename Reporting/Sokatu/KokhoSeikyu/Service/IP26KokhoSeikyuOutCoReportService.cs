using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSeikyu.Service;

public interface IP26KokhoSeikyuOutCoReportService
{
    CommonReportingRequestModel GetP26KokhoSeikyuOutReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
