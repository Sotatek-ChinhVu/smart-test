using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP24WelfareSyomeiSofuCoReportService
    {
        CommonReportingRequestModel GetP24WelfareSyomeiSofuReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
