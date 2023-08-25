using Reporting.Mappers.Common;
using Reporting.Structs;

namespace Reporting.Sokatu.WelfareDisk.Service
{
    public interface IP43Amakusa41DiskService
    {
        CommonExcelReportingModel GetDataP43Amakusa41Disk(int hpId, int seikyuYm, SeikyuType seikyuType);
    }
}
