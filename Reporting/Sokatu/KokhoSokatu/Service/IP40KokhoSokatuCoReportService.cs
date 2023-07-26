using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.KokhoSokatu.Service;

public interface IP40KokhoSokatuCoReportService
{
    CommonReportingRequestModel GetP40KokhoSokatuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
