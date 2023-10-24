using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareSeikyu.Service
{
    public interface IP24WelfareSyomeiListCoReportService
    {
        CommonReportingRequestModel GetP24WelfareSyomeiListReportingData(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
