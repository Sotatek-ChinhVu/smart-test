using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareDisk.Service
{
    public interface IP44WelfareDiskService
    {
        CommonExcelReportingModel GetDataP44WelfareDisk(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
