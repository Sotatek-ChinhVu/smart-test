using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.Syaho.Service;

public interface ISyahoCoReportService
{
    CommonReportingRequestModel GetSyahoPrintData(int hpId, int seikyuYm, SeikyuType seikyuType);
}
