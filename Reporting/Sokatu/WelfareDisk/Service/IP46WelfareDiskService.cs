using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareDisk.Service
{
    public interface IP46WelfareDiskService
    {
        CommonExcelReportingModel GetDataP46WelfareDisk(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
